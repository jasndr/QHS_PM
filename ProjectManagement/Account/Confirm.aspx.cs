using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Account
{
    /// <summary>
    /// @File: Confirm.aspx.cs
    /// @FrontEnd: Confirm.aspx
    /// @Author: Jason Delos Reyes
    /// @Summary: Email confirmation page of Project Tracking System.
    /// 
    ///           This form appears after the user receives the "email confirmation" email to verify that they 
    ///           have access to the account they specify.  Users will not be able to log in unless they have
    ///           click this link, which will automatically confirm their email and redirect them to log in
    ///           with confirmed credentials.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018NOV13 - Jason Delos Reyes  -  Added comments/documentation for easier legibility and
    ///                                    easier data structure view and management.
    ///                                 -  Added function that confirms the user's email so they
    ///                                    would be able to log in.  Users are unable to log in
    ///                                    without first confirming their email for their 
    ///                                    account integrity.
    /// </summary>
    public partial class Confirm : Page
    {
        /// <summary>
        /// Systematically confirms the user's email if the url link sent to the user's email
        /// has been clicked.  Only the referred user has access to this email link, which will
        /// automatically confirm the email upon clicking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Obtain code and user Id for email confirmation
            string code = IdentityHelper.GetCodeFromRequest(Request);
            string userId = IdentityHelper.GetUserIdFromRequest(Request);

            ApplicationUser User = new ApplicationUser();

            var manager = new UserManager();
            User = manager.FindById(userId);
            var provider = new DpapiDataProtectionProvider("ProjectManagement");

            manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                provider.Create("EmailConfirmation"));

            // Confirms email based on received code and user id.
            IdentityResult validToken = manager.ConfirmEmail(User.Id, code);

        }
    }
}