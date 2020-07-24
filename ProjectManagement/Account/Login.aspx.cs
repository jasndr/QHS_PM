using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Owin;
using ProjectManagement.Models;
using System.Text;

namespace ProjectManagement.Account
{
    /// <summary>
    /// @File: Login.aspx.cs
    /// @FrontEnd: Login.aspx
    /// @Author: Yang Rui
    /// @Summary: Login page of Project Tracking System.
    /// 
    ///           Login page controls that serves as a locking mechanism for the to provide authorize
    ///           and appropriate access to individuals through individual accounts in the tracking system.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018NOV08 - Jason Delos Reyes  -  Added comments/documentation for easier legibility and
    ///                                    easier data structure view and management.
    ///                                 -  Added requirement of a "confirmed" email account before
    ///                                    accessing your account.
    ///  2020JUL23 - Jason Delos Reyes  -  Created warning message for those attempting to log into the
    ///                                    existing tracking system in preparation for the database split.
    ///  </summary>
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                //// Validate the user password
                //var manager = new ApplicationUserManager();
                //ApplicationUser user = manager.Find(UserName.Text, Password.Text);
                //if (user != null)
                //{
                //    IdentityHelper.SignIn(manager, user, RememberMe.Checked);
                //    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                //}
                //else
                //{
                //    FailureText.Text = "Invalid username or password.";
                //    ErrorMessage.Visible = true;
                //}                

                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                // Require the user to have a confirmed email before they can log on.
                var user = manager.FindByName(UserName.Text);
                if (user != null)
                {
                    if (!user.EmailConfirmed)
                    {
                        FailureText.Text = "Invalid login attempt.  You must have a confirmed email account.";
                        ErrorMessage.Visible = true;
                    }
                    else
                    {

                        // This doen't count login failures towards account lockout
                        // To enable password failures to trigger lockout, change to shouldLockout: true
                        var result = signinManager.PasswordSignIn(UserName.Text, Password.Text, RememberMe.Checked, shouldLockout: true);

                        ////todo create a user log file saved to \logs
                        //StringBuilder logBuilder = new StringBuilder();
                        //logBuilder.Append(UserName.Text);
                        //logBuilder.AppendLine();
                        //logBuilder.AppendLine(Context.GetOwinContext().Response.ToString());

                        switch (result)
                        {
                            case SignInStatus.Success:
                                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                                break;
                            case SignInStatus.LockedOut:
                                Response.Redirect("/Account/Lockout");
                                break;
                            case SignInStatus.RequiresVerification:
                                Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                                Request.QueryString["ReturnUrl"],
                                                                RememberMe.Checked),
                                                  true);
                                break;
                            case SignInStatus.Failure:
                            default:
                                FailureText.Text = "Invalid login attempt";
                                ErrorMessage.Visible = true;
                                break;
                        }
                    }
                }
            }
        }
    }
}