using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Budget_Tracker.Administrator
{
    public partial class AddItems : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Add a default value to the Drop Down List
            ListItem defaultValue = new ListItem("Select an item", String.Empty);
            defaultValue.Attributes.Add("disabled", "disabled");
            PossibleItems.Items.Insert(0, defaultValue);
            PossibleItems.Items[0].Selected = true;

            // Create ListItems corresponding to the possible items we can add things to (ex: Currency, Purpose)
            ListItem currency = new ListItem("Currency", "Currency");
            ListItem purpose = new ListItem("Purpose", "Purpose");
            ListItem paymentMethod = new ListItem("Payment Method", "Payment Method");
            // Add them to the Drop Down List
            PossibleItems.Items.Add(currency);
            PossibleItems.Items.Add(purpose);
            PossibleItems.Items.Add(paymentMethod);
        }

        protected void PossibleItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            PossibleItems.Items[0].Selected = false;
            Label1.Text = "1";
        }
    }
}