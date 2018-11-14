using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using ProjectManagement.Models;

namespace ProjectManagement.Account
{
    /// <summary>
    /// @File: Register.aspx.cs
    /// @FrontEnd: Register.aspx
    /// @Author: Yang Rui
    /// @Summary: Registration page of Project Tracking System.
    /// 
    ///           Page that allows users to create their own accounts on the Project Tracking System.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018NOV07 - Jason Delos Reyes  -  Added comments/documentation for easier legibility and
    ///                                    easier data structure view and management.
    ///  2018NOV08 - Jason Delos Reyes  -  Added requirement of confirming one's email before creating
    ///                                    and account for added security measures.
    ///  2018NOV13 - Jason Delos Reyes  -  Made registration form hidden after submission to print "read email"
    ///                                    message to prompt user to check his/her email to confirm.
    /// </summary>
    public partial class Register : Page
    {
        /// <summary>
        /// Creates new user and assigns user to role 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            //var manager = new UserManager();
            //var user = new ApplicationUser() { UserName = UserName.Text, Email = UserName.Text };
            //IdentityResult result = manager.Create(user, Password.Text);
            //if (result.Succeeded)
            //{
            //    IdentityHelper.SignIn(manager, user, isPersistent: false);
            //    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            //}
            //else 
            //{
            //    ErrorMessage.Text = result.Errors.FirstOrDefault();
            //}

            Models.ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdUserResult;

            // Create a RoleStore object by using the ApplicationDbContext object. 
            // The RoleStore is only allowed to contain IdentityRole objects.
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects.
            // When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object. 
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser
            {
                UserName = UserName.Text,
                Email = Email.Text
            };
            IdUserResult = userMgr.Create(appUser, Password.Text);

            //assign new user to default biostat role
            if (IdUserResult.Succeeded)
            {

                //Add email confirmation

                var provider = new DpapiDataProtectionProvider("ProjectManagement");
                userMgr.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                    provider.Create("EmailConfirmation"));


                string code = userMgr.GenerateEmailConfirmationToken(appUser.Id);
                string callBackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, appUser.Id, Request);

                string sendTo = Email.Text;
                string subject = "QHS Project Tracking System - Confirm your account";
                string body = "Please confirm your account by clicking <a href=\"" + callBackUrl + "\">here</a>";

                IdentityMessage im = new IdentityMessage()
                {
                    Subject = subject,
                    Destination = sendTo,
                    Body = body,
                };
                EmailService emailService = new EmailService();
                emailService.Send(im);

                //Add user as guest
                if (roleMgr.RoleExists("Guest"))
                {
                    if (!userMgr.IsInRole(userMgr.FindByEmail(Email.Text).Id, "Guest"))
                    {
                        IdUserResult = userMgr.AddToRole(userMgr.FindByEmail(Email.Text).Id, "Guest");
                    }
                }

                //Website should automatically log user in if email is already confirmed,
                //otherwise, they should click the confirmation link in their email.
                if (appUser.EmailConfirmed)
                {
                    var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                    signinManager.PasswordSignIn(UserName.Text, Password.Text, isPersistent: true, shouldLockout: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else
                {
                    ErrorMessage.Text = "An email has been sent to your account.  Please view the email and confirm your" +
                                         " account to complete the registration process.";
                    registerForm.Visible = false;
                }


               
            }
            else
            {
                ErrorMessage.Text = IdUserResult.Errors.FirstOrDefault();
            }
        }
        
        


    }
}