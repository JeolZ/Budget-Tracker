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
            // the Y axis of the chart, corresponds to each date of change
            string[] yAxisDate = new string[moneyAmount.Values.Count];
            // the X axis of the chart, corresponds to the new amount of money after each date (i.e. if a user starts with 250€ on date1 and spends 20€ on date2, the new amount will be 230€)
            // this is needed so we can see the progression of a user's budget over time on the chart
            decimal[] xAxisAmountEachDate = new decimal[moneyAmount.Keys.Count];

            // if the user has any income/expenditure
            if (xAxisAmountEachDate.Count() != 0)
            {
                xAxisAmountEachDate[0] = Convert.ToDecimal(moneyAmount[0].getAmount());
                xAxisAmountEachDate[0] = Convert.ToDecimal(xAxisAmountEachDate[0].ToString("#.##"));
                yAxisDate[0] = moneyAmount[0].getDate().ToShortDateString().ToString();
                for (int i = 1; i < moneyAmount.Keys.Count; ++i)
                {
                    xAxisAmountEachDate[i] = xAxisAmountEachDate[i - 1] + Convert.ToDecimal(moneyAmount[i].getAmount());
                    yAxisDate[i] = moneyAmount[i].getDate().ToShortDateString().ToString();
                    xAxisAmountEachDate[i] = Convert.ToDecimal(xAxisAmountEachDate[i].ToString("#.##"));
                }
            }

            BudgetLineChart.ChartTitle = string.Format("{0}'s budget over time", pseudo);

            BudgetLineChart.Series.Add(new AjaxControlToolkit.LineChartSeries { Name = pseudo + "'s budget", Data = xAxisAmountEachDate.ToArray() });
            BudgetLineChart.CategoriesAxis = string.Join(",", yAxisDate.ToArray());
            BudgetLineChart.ChartWidth = (moneyAmount.Values.Count * 100).ToString();

            BudgetLineChart.AreaDataLabel = "€";
        }

        protected void populatePieChart()
        {
            Dictionary<string, double> amountPerPurpose = new Dictionary<string, double>();

            foreach (MoneyAmount amount in moneyAmount.Values)
            {
                if (amount.getAmount() < 0)
                {

                    if (amountPerPurpose.ContainsKey(amount.getPurpose()))
                    {
                        amountPerPurpose[amount.getPurpose()] += amount.getAmount();
                    }
                    else
                    {
                        amountPerPurpose.Add(amount.getPurpose(), amount.getAmount());
                    }
                }
            }

            foreach (KeyValuePair<string, double> entry in amountPerPurpose)
            {
                if (entry.Key != "None")
                {
                    BudgetPieChart.PieChartValues.Add(new AjaxControlToolkit.PieChartValue
                    {
                        Category = entry.Key,
                        Data = Convert.ToDecimal(entry.Value.ToString("#.##"))
                    });
                }
            }

            BudgetPieChart.ChartTitle = string.Format("{0}'s spending per purpose", pseudo);
        }
    }
}