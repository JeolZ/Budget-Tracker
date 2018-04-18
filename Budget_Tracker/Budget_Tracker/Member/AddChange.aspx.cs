using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace Budget_Tracker.Member
{
    public partial class AddChange : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // initialize the date textbox to today's date by default
            ChangeDateTB.Text = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));
        }

        protected void ConfirmButton_Click(object sender, EventArgs e)
        {
            double amount = Convert.ToDouble(MoneyAmount.Text);
            // if the change is an expenditure, then the amount is negative
            if (ExpenditureBTN.Checked)
            {
                amount = amount * (-1);
            }

            string currency = ChangeCurrencyDD.SelectedValue;

            // get the date as a string from the textbox
            string dateString = ChangeDateTB.Text;
            // split the date string on every '-' or '/' found
            string[] splitDateString = dateString.Split(new char[] { '-', '/' });

            // get the time as a string from the textbox
            string timeString = ChangeTimeTB.Text;
            // split the date string on every ':'
            string[] splitTimeString = timeString.Split(new char[] { ':' });

            // convert the strings for time and date into a DateTime
            DateTime date = new DateTime(Convert.ToInt32(splitDateString[2]), Convert.ToInt32(splitDateString[1]), Convert.ToInt32(splitDateString[0]), Convert.ToInt32(splitTimeString[2]), Convert.ToInt32(splitTimeString[1]), Convert.ToInt32(splitTimeString[0]));

            string paymentMethod = ChangePaymentMethodDD.SelectedValue;
            string purpose = ChangePurposeDD.SelectedValue;
            string recipient = ChangeRecipientTB.Text;
            string comment = ChangeCommentTB.Text;

            // pseudo of the owner of the change
            string pseudo = User.Identity.Name;

            // get the primary key for the foreign keys (currency, paymentMethod and purpose)

            // Gets the default connection string/path to our database from the web.config file
            string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Creates a connection to our database
            SqlConnection con = new SqlConnection(dbstring);

            // The SQL statement to get the Primary Keys. By using prepared statements,
            // we automatically get some protection against SQL injection.
            string sqlStr = "SELECT CurrencyID, PurposeID, PaymentMethodID FROM Currency, Purpose, PaymentMethod WHERE CurrencyName = @Currency AND PurposeName = @Purpose AND PaymentMethodName = @PaymentMethod;";

            // Open the database connection
            con.Open();

            // Create an executable SQL command containing our SQL statement and the database connection
            SqlCommand sqlCmd = new SqlCommand(sqlStr, con);

            // Fill in the parameters in our prepared SQL statement
            sqlCmd.Parameters.AddWithValue("@Currency", currency);
            sqlCmd.Parameters.AddWithValue("@Purpose", purpose);
            sqlCmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

            int currencyInt = 0;
            int purposeInt = 0;
            int paymentMethodInt = 0;

            // Reads the data from the query
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();
            while (sqlReader.Read())
            {
                currencyInt = Convert.ToInt32(sqlReader["CurrencyID"]);
                purposeInt = Convert.ToInt32(sqlReader["PurposeID"]);
                paymentMethodInt = Convert.ToInt32(sqlReader["PaymentMethodID"]);
            }
            con.Close();

            con.Open();
            // Insert data into the DB
            string sqlStr2 = "INSERT INTO Change(ChangeAmountMoney, ChangeDate, ChangeComment, ChangeRecipient, CurrencyID, PaymentMethodID, PurposeId, Pseudo) VALUES(@amount, @date, @comment, @recipient, @currencyid, @paymentmethodid, @purposeid, @pseudo);";
            SqlCommand sqlCmd2 = new SqlCommand(sqlStr2, con);
            sqlCmd2.Parameters.AddWithValue("@amount", amount);
            sqlCmd2.Parameters.AddWithValue("@date", date);
            sqlCmd2.Parameters.AddWithValue("@comment", comment);
            sqlCmd2.Parameters.AddWithValue("@recipient", recipient);
            sqlCmd2.Parameters.AddWithValue("@currencyid", currencyInt);
            sqlCmd2.Parameters.AddWithValue("@paymentmethodid", paymentMethodInt);
            sqlCmd2.Parameters.AddWithValue("@purposeid", purposeInt);
            sqlCmd2.Parameters.AddWithValue("@pseudo", pseudo);

            // Execute the SQL command
            sqlCmd2.ExecuteNonQuery();

            // Close the connection to the database
            con.Close();

            if (ExpenditureBTN.Checked)
            {
                ResultLabel.Text = "Expenditure added!";
            }
            else
            {
                ResultLabel.Text = "Income added!";
            }
        }
    }
}