using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Guest
{
    public partial class ConfirmRequest : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            }


            if (HasPreviousPage())
            {
                //TextBox textbox2 = (TextBox)PreviousPage.FindControl("TextBox1");

                TextBox firstName = (TextBox)PreviousPage.FindControl("txtFirstName");
                lblFirstName.Text = firstName.Text;

                TextBox lastName = (TextBox)PreviousPage.FindControl("txtLastName");
                lblLastName.Text = lastName.Text;


            }

        }

        /// <summary>
        /// Returns to previous page if there is a previous page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {

            bool hasPreviousPage = HasPreviousPage();

            if (hasPreviousPage.Equals(true))
            {
                object refUrl = ViewState["RefUrl"];
                if (refUrl != null)
                {
                    Response.Redirect((string)refUrl);
                }
            } 

        }

        /// <summary>
        /// Returns a boolean value if there is a previous page.
        /// </summary>
        /// <returns>True if there is a previous page, false if there isn't.</returns>
        private bool HasPreviousPage()
        {
            if (Page.PreviousPage != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}