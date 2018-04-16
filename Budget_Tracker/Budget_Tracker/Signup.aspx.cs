using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Gives access to the static "Roles" class 
using System.Web.Security;

namespace Budget_Tracker
{
    public partial class Signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CreateUserWizard1_OnCreatedUser(object sender, EventArgs e)
        {
            // Set the role of the created user to the selected role in the DropDownList
            Roles.AddUserToRole(CreateUserWizard1.UserName, "Member");

            /* If newly created users should always belong to the same role (e.g. on a site where 
             * users can register themselves, they would probably all belong to "userRole"), we don't
             * need a DropDownList, and the role could simply be hardcoded like this:
             * Roles.AddUserToRole(CreateUserWizard1.UserName, "userRole");
             */

        }
    }
}