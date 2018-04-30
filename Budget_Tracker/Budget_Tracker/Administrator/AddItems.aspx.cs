using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Budget_Tracker.Administrator
{
    public partial class AddItems : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            PossibleItems.Items.FindByValue("defaultValue").Attributes.Add("disabled", "disabled");
        }

        protected void PossibleItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change the GridView to show all current items
            changeGridView();

            // Generate the form
            generateForm();
        }

        protected void changeGridView()
        {
            // Gets the default connection string/path to our database from the web.config file
            string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Creates a connection to our database
            SqlConnection con = new SqlConnection(dbstring);

            // Query
            // Fill in the parameters in our prepared SQL statement
            string sqlStr = string.Format("SELECT * FROM {0}", PossibleItems.SelectedValue);

            // Open the database connection
            con.Open();

            // Create an executable SQL command containing our SQL statement and the database connection
            SqlCommand sqlCmd = new SqlCommand(sqlStr, con);

            // Adapt the data from the SQL query in order to populate the GridView
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
            DataTable datatable = new DataTable();
            adapter.Fill(datatable);
            ItemNamesList.DataSource = datatable;
            ItemNamesList.DataBind();
            con.Close();
        }

        protected void generateForm()
        {
            // if the administrator wants to create a new purpose
            if (PossibleItems.SelectedValue.Equals("Purpose"))
            {
                ItemName.Text = "Add a new purpose!";
                PurposePanel.Visible = true;
                PaymentMethodPanel.Visible = false;
                CurrencyPanel.Visible = false;
            }

            // if the administrator wants to create a new currency
            else if (PossibleItems.SelectedValue.Equals("Currency"))
            {
                ItemName.Text = "Add a new currency!";
                CurrencyPanel.Visible = true;
                PaymentMethodPanel.Visible = false;
                PurposePanel.Visible = false;
            }

            // if the administrator wants to create a new payment method
            else if (PossibleItems.SelectedValue.Equals("PaymentMethod"))
            {
                ItemName.Text = "Add a new payment method!";
                PaymentMethodPanel.Visible = true;
                CurrencyPanel.Visible = false;
                PurposePanel.Visible = false;
            }
        }

        protected void addCurrency(object sender, EventArgs e)
        {
            // Gets the default connection string/path to our database from the web.config file
            string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Creates a connection to our database
            SqlConnection con = new SqlConnection(dbstring);

            // query
            string sqlStr = "INSERT INTO Currency(CurrencyName, CurrencySymbol) VALUES (@newItemName, @newItemSymbol)";

            // Open the database connection
            con.Open();

            // Create an executable SQL command containing our SQL statement and the database connection
            SqlCommand sqlCmd = new SqlCommand(sqlStr, con);

            // Add parameter to the query
            sqlCmd.Parameters.AddWithValue("@newItemName", currencyNameTB.Text);
            sqlCmd.Parameters.AddWithValue("@newItemSymbol", currencySymbolTB.Text);

            sqlCmd.ExecuteNonQuery();

            con.Close();
        }

        protected void addPaymentMethod(object sender, EventArgs e)
        {
            // Gets the default connection string/path to our database from the web.config file
            string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Creates a connection to our database
            SqlConnection con = new SqlConnection(dbstring);

            // query
            string sqlStr = "INSERT INTO PaymentMethod(PaymentMethodName) VALUES (@newItem)";

            // Open the database connection
            con.Open();

            // Create an executable SQL command containing our SQL statement and the database connection
            SqlCommand sqlCmd = new SqlCommand(sqlStr, con);

            // Add parameter to the query
            sqlCmd.Parameters.AddWithValue("@newItem", PMNameTB.Text);

            sqlCmd.ExecuteNonQuery();

            con.Close();
        }

        protected void addPurpose(object sender, EventArgs e)
        {
            // Gets the default connection string/path to our database from the web.config file
            string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Creates a connection to our database
            SqlConnection con = new SqlConnection(dbstring);

            // query
            string sqlStr = "INSERT INTO Purpose(PurposeName) VALUES (@newItem)";

            // Open the database connection
            con.Open();

            // Create an executable SQL command containing our SQL statement and the database connection
            SqlCommand sqlCmd = new SqlCommand(sqlStr, con);

            // Add parameter to the query
            sqlCmd.Parameters.AddWithValue("@newItem", purposeNameTB.Text);

            sqlCmd.ExecuteNonQuery();

            con.Close();
        }

    }
}