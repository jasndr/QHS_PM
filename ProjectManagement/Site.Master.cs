using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement
{
    /// <summary>
    /// @File: Site.Master.cs
    /// @Author: Yang Rui
    /// @Summary: Links that can be viewed on all pages.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018MAY14 - Jason Delos Reyes  -  Added Ola Hawaiʻi Summary Report tab under "Data Presentations". Also added
    ///                                    comments/documentation to improve legibility on data structure view and
    ///                                    management.
    ///  2018MAY21 - Jason Delos Reyes  -  Addded PI/Project Review List under "Forms" that is only accessible to "Admin"
    ///                                    accounts for easier tracking purposes (instead of just relying on emails to 
    ///                                    see a list of unreviewed PI and Project entries.).
    ///  2018JUL20 - Jason Delos Reyes  -  Made "Data Presentation" > "RMATRIX Summary Report" and "Ola Hawaii Summary Report" 
    ///                                    hidden as to consolidate to a unified summary report form for both grants, that can
    ///                                    easily be expanded to more grants.
    ///  2019APR26 - Jason Delos Reyes  -  Replace sole "RMATRIX Monthly Report" link to a more inclusive "RMATRIX / Ola HAWAII Monthly Report"
    ///                                    tab to allow pulling Ola HAWAII reports for now, and possibly expand to other grants in the future.
    /// </summary>
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        /// <summary>
        /// Loads page using Anti-XSRF token.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        /// <summary>
        /// On page load, hides links based on whether or not users are logged into the system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("Biostat"))
            {
                //ClientLink.Visible = true;
                PILink.Visible = true;
                //BiostatLink.Visible = true;
                ProjectLink.Visible = true;
                //ProjectListLink.Visible = true;
                //PaperLink.Visible = true;
                //AdminLink.Visible = true;
                TimeEntryLink.Visible = true;
                
                ReportLink1.Visible = true;
                //ReportLink2.Visible = true;
                //ReportLink3.Visible = true;
                ReportLink4.Visible = true;
                //ReportLink5.Visible = true;
                ReportLink6.Visible = true;
                ReportLink7.Visible = true;

                if (HttpContext.Current.User.IsInRole("Admin"))
                {
                    ProjectList.Visible = true;
                 //   ImportRequestLink.Visible = true;
                }

            }

            //if (HttpContext.Current.User.IsInRole("Super"))
            //{
            //    SuperLink.Visible = true;
            //}
        }

        /// <summary>
        /// Logs users out of system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }
    }

}