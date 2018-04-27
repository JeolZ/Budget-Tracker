﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

//Gives access to the static "Roles" class 
using System.Web.Security;

namespace Budget_Tracker.Member
{
    public partial class Budget : System.Web.UI.Page
    {
        // todo: potentially needless to put it there
        private DataTable datatable;

        // Pseudo of the user you want to see the budget of
        string pseudo;


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

            // Gets the default connection string/path to our database from the web.config file
            string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Creates a connection to our database
            SqlConnection con = new SqlConnection(dbstring);

            // query
            string sqlStr = "SELECT * FROM ChangeView WHERE (Pseudo = @username)";

            // Open the database connection
            con.Open();

            // Create an executable SQL command containing our SQL statement and the database connection
            SqlCommand sqlCmd = new SqlCommand(sqlStr, con);

            // Fill in the parameters in our prepared SQL statement
            sqlCmd.Parameters.AddWithValue("@username", pseudo);

            // Adapt the data from the SQL query in order to populate the GridView
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
            datatable = new DataTable();
            adapter.Fill(datatable);

            // Add a new column to the DataTable to add the buttons to allow the user to modify a change
            datatable.Columns.Add(new DataColumn("Modify", typeof(string)));

            BudgetGridView.DataSource = datatable;
            BudgetGridView.DataBind();
            con.Close();
        }

        protected void BudgetGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            // Hide columns from the GridView (because we need to data but we don't want to show it)
            e.Row.Cells[0].Visible = false;
        }

        protected void BudgetGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button link = new Button();
                link.Text = "link!";
                Button link2 = new Button();
                link2.Text = "link2!";
                e.Row.Cells[9].Controls.Add(link);
                e.Row.Cells[9].Controls.Add(link2);
            }
        }
    }
}