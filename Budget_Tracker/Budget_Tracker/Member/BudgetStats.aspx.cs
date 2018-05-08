using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Budget_Tracker.Member
{
    public partial class BudgetStats : System.Web.UI.Page
    {
        /**
         * DEFAULT CURRENCY TO CONVERT TO IS EUR
         * Conversion values will be written in raw in the code
         * If we add a new currency, there won't be a conversion value, and as such will just count 1 to 1.
         * 
         * Values were taken the: 05/05/2018.
         */
        const double USDtoEUR = 0.834895283;
        const double DKKtoEUR = 0.134068319;
        const double GBPtoEUR = 1.13006917;


        // Pseudo of the user you want to see the budget of
        string pseudo;

        // user's new amount of money after every change
        Dictionary<int, MoneyAmount> moneyAmount = new Dictionary<int, MoneyAmount>();

        protected void Page_Load(object sender, EventArgs e)
        {
            // if there is a GET parameter AND the user going on the page is an administrator
            if (User.IsInRole("Administrator") && Request.QueryString["pseudo"] != null)
            {
                // then we have access to the user of the parameters' budget
                pseudo = Request.QueryString["pseudo"];
            }
            else
            {
                // we access the budget of the current user
                pseudo = User.Identity.Name;
            }

            getCurrentMoneyAmount();

            changeTotalAmountLabel();

            populateLineChart();

            populatePieChart();
        }

        // Money to calculate the user's current money amount after converting to a single currency
        protected void getCurrentMoneyAmount()
        {
            // Gets the default connection string/path to our database from the web.config file
            string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Creates a connection to our database
            SqlConnection con = new SqlConnection(dbstring);

            // query
            string sqlStr = "SELECT Amount, Currency, Purpose, Date FROM ChangeView WHERE (Pseudo = @username) ORDER BY Date";

            // Open the database connection
            con.Open();

            // Create an executable SQL command containing our SQL statement and the database connection
            SqlCommand sqlCmd = new SqlCommand(sqlStr, con);

            // Fill in the parameters in our prepared SQL statement
            sqlCmd.Parameters.AddWithValue("@username", pseudo);

            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

            int i = 0;
            while (sqlReader.Read())
            {
                if (sqlReader["Currency"].Equals("DKK"))
                {
                    moneyAmount.Add(i, new MoneyAmount(Convert.ToDouble(sqlReader["Amount"]) * DKKtoEUR, Convert.ToDateTime(sqlReader["Date"]), sqlReader["Purpose"].ToString()));
                }
                else if (sqlReader["Currency"].Equals("USD"))
                {
                    moneyAmount.Add(i, new MoneyAmount(Convert.ToDouble(sqlReader["Amount"]) * USDtoEUR, Convert.ToDateTime(sqlReader["Date"]), sqlReader["Purpose"].ToString()));
                }
                else if (sqlReader["Currency"].Equals("GBP"))
                {
                    moneyAmount.Add(i, new MoneyAmount(Convert.ToDouble(sqlReader["Amount"]) * GBPtoEUR, Convert.ToDateTime(sqlReader["Date"]), sqlReader["Purpose"].ToString()));
                }
                else
                {
                    moneyAmount.Add(i, new MoneyAmount(Convert.ToDouble(sqlReader["Amount"]), Convert.ToDateTime(sqlReader["Date"]), sqlReader["Purpose"].ToString()));
                }
                ++i;
            }

            con.Close();
        }

        protected void changeTotalAmountLabel()
        {
            double total = 0;
            foreach (MoneyAmount a in moneyAmount.Values)
            {
                total += a.getAmount();
            }
            if (total == 0)
            {
                TotalAmount.Text = "0 EUR";
            }
            else
            {
                TotalAmount.Text = total.ToString("#.##") + " EUR";
            }

            if (total >= 0)
            {
                TotalAmount.CssClass = "PositiveAmountLabel";
            }
            else
            {
                TotalAmount.CssClass = "NegativeAmountLabel";
            }
        }


        protected void populateLineChart()
        {
            //group by date all the income/expenditure
            List<MoneyAmount> monAm = new List<MoneyAmount>();

            string prevDate = "";
            for (int i = 0, j = -1; i < moneyAmount.Keys.Count; ++i)
            {
                //if it's still the same date, add the amount to the previous
                if (moneyAmount[i].getDate().ToString() == prevDate && j > 0)
                {
                    monAm[j].addAmount(moneyAmount[i].getAmount());
                }
                else //if it's a new date, add a new item to the list
                {
                    prevDate = moneyAmount[i].getDate().ToString();
                    monAm.Add(new MoneyAmount(moneyAmount[i]));
                    ++j;
                }
            }

            // the Y axis of the chart, corresponds to each date of change
            string[] yAxisDate = new string[monAm.Count];
            // the X axis of the chart, corresponds to the new amount of money after each date (i.e. if a user starts with 250€ on date1 and spends 20€ on date2, the new amount will be 230€)
            // this is needed so we can see the progression of a user's budget over time on the chart
            decimal[] xAxisAmountEachDate = new decimal[monAm.Count];

            // if the user has any income/expenditure
            if (xAxisAmountEachDate.Count() != 0)
            {
                // fill the first item of the array because we need to add the amounts to the previous values
                xAxisAmountEachDate[0] = Convert.ToDecimal(moneyAmount[0].getAmount());
                // convert to decimal and with only 2 numbers after the comma
                xAxisAmountEachDate[0] = Convert.ToDecimal(xAxisAmountEachDate[0].ToString("#.##"));
                yAxisDate[0] = moneyAmount[0].getDate().ToShortDateString().ToString();
                for (int i = 1; i < monAm.Count; ++i)
                {
                    // add the amount to the previous value
                    xAxisAmountEachDate[i] = xAxisAmountEachDate[i - 1] + Convert.ToDecimal(monAm[i].getAmount());
                    yAxisDate[i] = monAm[i].getDate().ToShortDateString().ToString();
                    xAxisAmountEachDate[i] = Convert.ToDecimal(xAxisAmountEachDate[i].ToString("#.##"));
                }
            }

            // title of the chart
            BudgetLineChart.ChartTitle = string.Format("{0}'s budget over time", pseudo);

            // Y dimension of the chart (and not x like the name suggests)
            BudgetLineChart.Series.Add(new AjaxControlToolkit.LineChartSeries { Name = pseudo + "'s budget", Data = xAxisAmountEachDate.ToArray() });
            // X dimension of the chart (and not Y like the name suggests)
            BudgetLineChart.CategoriesAxis = string.Join(",", yAxisDate.ToArray());

            // Width of the chart
            BudgetLineChart.ChartWidth = (moneyAmount.Values.Count * 75).ToString();

            // add a little euro sign for when the user hover on a point on the chart
            BudgetLineChart.AreaDataLabel = "€";
        }

        protected void populatePieChart()
        {
            // list all amounts per purpose
            Dictionary<string, double> amountPerPurpose = new Dictionary<string, double>();

            foreach (MoneyAmount amount in moneyAmount.Values)
            {
                // we only want the spendings
                if (amount.getAmount() < 0)
                {
                    // if the purpose already exists
                    if (amountPerPurpose.ContainsKey(amount.getPurpose()))
                    {
                        // then we just add the values
                        amountPerPurpose[amount.getPurpose()] += amount.getAmount();
                    }
                    // else we add the purpose
                    else
                    {
                        amountPerPurpose.Add(amount.getPurpose(), amount.getAmount());
                    }
                }
            }

            // add the values to the pie chart
            foreach (KeyValuePair<string, double> entry in amountPerPurpose)
            {
                BudgetPieChart.PieChartValues.Add(new AjaxControlToolkit.PieChartValue
                {
                    Category = entry.Key,
                    Data = Convert.ToDecimal(entry.Value.ToString("#.##"))
                });
            }

            // title
            BudgetPieChart.ChartTitle = string.Format("{0}'s spending per purpose", pseudo);
        }
    }
}