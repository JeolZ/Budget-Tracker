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
        double amount;
        string currency;
        DateTime date;
        string paymentMethod;
        string purpose;
        string recipient;
        string comment;
        string pseudo;

        protected void Page_PreRender(object sender, EventArgs e)
        {

            if (Request.QueryString["id"] != null)
            {
                // Fill the pseudo attribute
                getPseudoFromIDParameter();

                // we fill the attributes only if the user going on the page is the owner of the change or an administrator
                if (User.IsInRole("Administrator") || pseudo.Equals(User.Identity.Name))
                {
                    // Fill the rest of the attributes
                    fillAttributes();
                }

                else
                {
                    // Redirect to the home page
                    Response.Redirect("../index.aspx");
                }

                // Fill the form
                FillForm();
            }

            // initialize the date textbox to today's date by default
            ChangeDateTB.Text = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));

            ExpenditureBTN.Checked = true;
        }

        protected void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (MoneyAmount.Text == "")
            {
                ResultLabel.Text = "Please insert at least an amount!";
            }
            else
            {
                // if there is a GET parameter, then we are modifying a currently existing change
                if (Request.QueryString["id"] != null)
                {
                    updateChange();
                }
                // otherwise, we add a new one
                else
                {
                    insertChange();
                }
            }
        }

        protected void insertChange()
        {
            amount = Convert.ToDouble(MoneyAmount.Text);
            // if the change is an expenditure, then the amount is negative
            if (ExpenditureBTN.Checked)
            {
                amount = amount * (-1);
            }

            currency = ChangeCurrencyDD.SelectedValue;

            // get the date as a string from the textbox
            string dateString = ChangeDateTB.Text;
            // split the date string on every '-' or '/' found
            string[] splitDateString = dateString.Split(new char[] { '-', '/' });

            // get the time as a string from the textbox
            string timeString = ChangeTimeTB.Text;
            // split the date string on every ':'
            string[] splitTimeString = timeString.Split(new char[] { ':' });

            // convert the strings for time and date into a DateTime
            date = new DateTime(Convert.ToInt32(splitDateString[2]), Convert.ToInt32(splitDateString[1]), Convert.ToInt32(splitDateString[0]), Convert.ToInt32(splitTimeString[0]), Convert.ToInt32(splitTimeString[1]), Convert.ToInt32(splitTimeString[2]));

            paymentMethod = ChangePaymentMethodDD.SelectedValue;
            purpose = ChangePurposeDD.SelectedValue;
            recipient = ChangeRecipientTB.Text;
            comment = ChangeCommentTB.Text;

            // pseudo of the owner of the change
            pseudo = User.Identity.Name;

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

        protected void updateChange()
        {
            amount = Convert.ToDouble(MoneyAmount.Text);
            // if the change is an expenditure, then the amount is negative
            if (ExpenditureBTN.Checked)
            {
                amount = amount * (-1);
            }

            currency = ChangeCurrencyDD.SelectedValue;

            // get the date as a string from the textbox
            string dateString = ChangeDateTB.Text;
            // split the date string on every '-' or '/' found
            string[] splitDateString = dateString.Split(new char[] { '-', '/' });

            // get the time as a string from the textbox
            string timeString = ChangeTimeTB.Text;
            // split the date string on every ':'
            string[] splitTimeString = timeString.Split(new char[] { ':' });

            // convert the strings for time and date into a DateTime
            date = new DateTime(Convert.ToInt32(splitDateString[2]), Convert.ToInt32(splitDateString[1]), Convert.ToInt32(splitDateString[0]), Convert.ToInt32(splitTimeString[0]), Convert.ToInt32(splitTimeString[1]), Convert.ToInt32(splitTimeString[2]));

            paymentMethod = ChangePaymentMethodDD.SelectedValue;
            purpose = ChangePurposeDD.SelectedValue;
            recipient = ChangeRecipientTB.Text;
            comment = ChangeCommentTB.Text;

            // pseudo of the owner of the change
            pseudo = User.Identity.Name;

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

            // Update data from the DB
            string sqlStr2 = "UPDATE Change SET ChangeAmountMoney = @amount, ChangeDate = @date, ChangeComment = @comment, ChangeRecipient = @recipient, CurrencyID = @currencyid, PaymentMethodID = @paymentmethodid, PurposeId = @purposeid WHERE ChangeId = @id;";
            SqlCommand sqlCmd2 = new SqlCommand(sqlStr2, con);
            sqlCmd2.Parameters.AddWithValue("@amount", amount);
            sqlCmd2.Parameters.AddWithValue("@date", date);
            sqlCmd2.Parameters.AddWithValue("@comment", comment);
            sqlCmd2.Parameters.AddWithValue("@recipient", recipient);
            sqlCmd2.Parameters.AddWithValue("@currencyid", currencyInt);
            sqlCmd2.Parameters.AddWithValue("@paymentmethodid", paymentMethodInt);
            sqlCmd2.Parameters.AddWithValue("@purposeid", purposeInt);
            sqlCmd2.Parameters.AddWithValue("@id", Request.QueryString["id"]);

            // Execute the SQL command
            sqlCmd2.ExecuteNonQuery();

            // Close the connection to the database
            con.Close();

            if (ExpenditureBTN.Checked)
            {
                ResultLabel.Text = "Expenditure modified!";
            }
            else
            {
                ResultLabel.Text = "Income modified!";
            }
        }

        protected void getPseudoFromIDParameter()
        {
            if (Request.QueryString["id"] != null)
            {
                // Then we fetch the pseudo of the owner of the change
                // Gets the default connection string/path to our database from the web.config file
                string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                // Creates a connection to our database
                SqlConnection con = new SqlConnection(dbstring);

                // Query
                // Fill in the parameters in our prepared SQL statement
                string sqlStr = "SELECT Pseudo FROM Change WHERE ChangeID = @id";

                // Open the database connection
                con.Open();

                // Create an executable SQL command containing our SQL statement and the database connection
                SqlCommand sqlCmd = new SqlCommand(sqlStr, con);

                // Fill in the parameters in our prepared SQL statement
                sqlCmd.Parameters.AddWithValue("@id", Request.QueryString["id"]);


                // Reads the data from the query
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    pseudo = sqlReader["Pseudo"].ToString();
                }

                con.Close();
            }
        }

        protected void fillAttributes()
        {
            if (Request.QueryString["id"] != null)
            {
                // Gets the default connection string/path to our database from the web.config file
                string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                // Creates a connection to our database
                SqlConnection con = new SqlConnection(dbstring);

                // Query
                // Fill in the parameters in our prepared SQL statement
                string sqlStr = "SELECT * FROM ChangeView WHERE ID = @id";

                // Open the database connection
                con.Open();

                // Create an executable SQL command containing our SQL statement and the database connection
                SqlCommand sqlCmd = new SqlCommand(sqlStr, con);

                // Fill in the parameters in our prepared SQL statement
                sqlCmd.Parameters.AddWithValue("@id", Request.QueryString["id"]);


                // Reads the data from the query
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    pseudo = sqlReader["Pseudo"].ToString();
                    amount = Convert.ToDouble(sqlReader["Amount"].ToString());
                    currency = sqlReader["Currency"].ToString();
                    date = Convert.ToDateTime(sqlReader["Date"]);
                    paymentMethod = sqlReader["Payment Method"].ToString();
                    purpose = sqlReader["Purpose"].ToString();
                    recipient = sqlReader["Recipient"].ToString();
                    comment = sqlReader["Comment"].ToString();
                }

                con.Close();
            }
        }

        protected void FillForm()
        {
            // if amount is negative, then it's an expenditure, if positive (>= 0) then income
            if (amount < 0)
            {
                ExpenditureBTN.Checked = true;
            }
            else
            {
                IncomeBTN.Checked = true;
            }

            // Fill the Amount, Recipient, Comment, Date and Time TextBoxes
            MoneyAmount.Text = amount.ToString();
            ChangeRecipientTB.Text = recipient;
            ChangeCommentTB.Text = comment;
            ChangeDateTB.Text = date.ToShortDateString();
            ChangeTimeTB.Text = date.ToLongTimeString();
        }

        /**
         * When the user wants to modify a change, this trigger allows the DropDownList to be changed to the correct selected item
         */
        protected void ChangePurposeDD_DataBound(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                ChangePurposeDD.SelectedIndex = ChangePurposeDD.Items.IndexOf(ChangePurposeDD.Items.FindByText(purpose));
            }
        }

        protected void ChangePaymentMethodDD_DataBound(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                ChangePaymentMethodDD.SelectedIndex = ChangePaymentMethodDD.Items.IndexOf(ChangePaymentMethodDD.Items.FindByText(paymentMethod));
            }
        }

        protected void ChangeCurrencyDD_DataBound(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                ChangeCurrencyDD.SelectedIndex = ChangeCurrencyDD.Items.IndexOf(ChangeCurrencyDD.Items.FindByText(currency));
            }
        }
    }
}