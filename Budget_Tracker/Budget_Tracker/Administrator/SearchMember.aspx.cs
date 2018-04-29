using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Security;

namespace Budget_Tracker.Administrator
{
    public partial class SearchMember : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Gets the default connection string/path to our database from the web.config file
            string dbstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Creates a connection to our database
            SqlConnection con = new SqlConnection(dbstring);

            // query
            string sqlStr = "SELECT UserName as 'Member', RoleName as 'Role' FROM aspnet_usersinroles uir, aspnet_roles r, aspnet_users u WHERE uir.userid = u.userid and uir.roleid = r.roleid";

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

            // Add a new column to the DataTable to add the buttons to allow the administrator to modify a user
            datatable.Columns.Add(new DataColumn("Edit", typeof(string)));

            MemberGridView.DataSource = datatable;
            MemberGridView.DataBind();
            con.Close();


        }

        protected void MemberGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Populate the new column with buttons to manipulate the rows
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Create new buttons, to delete the user, to see his budget and to upgrade him to administrator
                Button deleteBTN = new Button();
                Button upgradeBTN = new Button();
                Button budgetBTN = new Button();
                deleteBTN.Text = "Delete";
                upgradeBTN.Text = "Upgrade";
                budgetBTN.Text = "Budget";
                deleteBTN.Click += (sender1, EventArgs) => { deleteUser(sender1, EventArgs, e.Row.Cells[0].Text); };
                upgradeBTN.Click += (sender1, EventArgs) => { upgradeUser(sender1, EventArgs, e.Row.Cells[0].Text); };
                budgetBTN.Click += (sender1, EventArgs) => { budgetUser(sender1, EventArgs, e.Row.Cells[0].Text); };

                // Add them at the last columns
                e.Row.Cells[e.Row.Controls.Count - 1].Controls.Add(deleteBTN);
                e.Row.Cells[e.Row.Controls.Count - 1].Controls.Add(upgradeBTN);
                e.Row.Cells[e.Row.Controls.Count - 1].Controls.Add(budgetBTN);
            }
        }

        protected void deleteUser(object sender, EventArgs e, string username)
        {
            // Delete a user
            Membership.DeleteUser(username);
            // Refresh the page
            Response.Redirect(Request.RawUrl);
        }

        protected void upgradeUser(object sender, EventArgs e, string username)
        {
            // Remove user from Member
            Roles.RemoveUserFromRole(username, "Member");
            // Add user to Administrator
            Roles.AddUserToRole(username, "Administrator");
            // Refresh the page
            Response.Redirect(Request.RawUrl);
        }

        protected void budgetUser(object sender, EventArgs e, string username)
        {
            // Redirects to the user's budget page
            Response.Redirect("../Member/Budget.aspx?pseudo=" + username);
        }
    }
}