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
            string sqlStr = "SELECT * FROM ChangeView WHERE (Pseudo = @username) ORDER BY Date DESC";

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
            datatable.Columns.Add(new DataColumn("Edit", typeof(string)));

            BudgetGridView.DataSource = datatable;
            BudgetGridView.DataBind();
            con.Close();

            AssignClassesToRow();

            ChangeHeaderText();
        }

        protected void BudgetGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Hide columns from the GridView (because we need the data but we don't want to show it)
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[8].Visible = false;

            // Populate the new column with buttons to manipulate the rows
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Create new buttons, one to delete the row, the other one to edit it
                Button deleteBTN = new Button();
                Button editBTN = new Button();
                deleteBTN.Text = "Delete";
                editBTN.Text = "Edit";
                deleteBTN.Click += (sender1, EventArgs) => { DeleteChange(sender1, EventArgs, e.Row.Cells[0].Text); };
                editBTN.Click += (sender1, EventArgs) => { RedirectsToEdit(sender1, EventArgs, e.Row.Cells[0].Text); };

                // Add them at the last columns
                e.Row.Cells[e.Row.Controls.Count - 1].Controls.Add(deleteBTN);
                e.Row.Cells[e.Row.Controls.Count - 1].Controls.Add(editBTN);
            }
        }

        protected void DeleteChange(object sender, EventArgs e, string id)
        {
            // Gets the default connection string/path to our database from the web.config file
            string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Creates a connection to our database
            SqlConnection con = new SqlConnection(dbstring);

            // query
            string sqlStr = "DELETE FROM CHANGE WHERE ChangeId = @id";

            // Open the database connection
            con.Open();

            // Create an executable SQL command containing our SQL statement and the database connection
            SqlCommand sqlCmd = new SqlCommand(sqlStr, con);

            // Fill in the parameters in our prepared SQL statement
            sqlCmd.Parameters.AddWithValue("@id", id);

            // Execute SQL Command
            sqlCmd.ExecuteNonQuery();

            con.Close();

            // Refresh the page
            Response.Redirect(Request.RawUrl);
        }

        protected void RedirectsToEdit(object sender, EventArgs e, string id)
        {
            // Redirect to the page to edit the change with the right id
            Response.Redirect("AddChange.aspx?id=" + id);
        }

        protected void AssignClassesToRow()
        {
            // for each rows in the GridView
            foreach (GridViewRow r in BudgetGridView.Rows)
            {
                // compare the 2nd cell to each row (which corresponds to the amount)
                if (Double.Parse(r.Cells[1].Text) < 0)
                {
                    r.CssClass = "ExpenditureRow";
                }
                else
                {
                    r.CssClass = "IncomeRow";
                }
            }
            if (BudgetGridView.HeaderRow != null)
            {
                BudgetGridView.HeaderRow.CssClass = "HeaderRowGridView";
            }
        }

        protected void ChangeHeaderText()
        {
            if (Request.QueryString["pseudo"] != null)
            {
                BudgetHeaderText.InnerText = Request.QueryString["pseudo"] + "'s budget!";
            }
        }

        protected void StatsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("BudgetStats.aspx?pseudo=" + pseudo);
        }
    }
}