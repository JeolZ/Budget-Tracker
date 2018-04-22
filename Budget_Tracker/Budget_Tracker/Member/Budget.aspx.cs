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
    public partial class Budget : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // Gets the default connection string/path to our database from the web.config file
            string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Creates a connection to our database
            SqlConnection con = new SqlConnection(dbstring);

            string sqlStr = "SELECT Amount, Currency, Date, [Payment Method], Purpose, Recipient, Comment FROM ChangeView WHERE (Pseudo = @username)";

            // Open the database connection
            con.Open();

            // Create an executable SQL command containing our SQL statement and the database connection
            SqlCommand sqlCmd = new SqlCommand(sqlStr, con);


            // Fill in the parameters in our prepared SQL statement
            sqlCmd.Parameters.AddWithValue("@username", User.Identity.Name);

            // Adapt the data from the SQL query in order to populate the GridView
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
            DataTable datatable = new DataTable();
            adapter.Fill(datatable);
            BudgetGridView.DataSource = datatable;
            BudgetGridView.DataBind();
            con.Close();

        }
    }
}