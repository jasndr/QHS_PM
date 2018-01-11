using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using ProjectManagement.Models;

namespace ProjectManagement.Account
{
    public partial class Register : Page
    {
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
                if (roleMgr.RoleExists("Guest"))
                {
                    if (!userMgr.IsInRole(userMgr.FindByEmail(Email.Text).Id, "Guest"))
                    {
                        IdUserResult = userMgr.AddToRole(userMgr.FindByEmail(Email.Text).Id, "Guest");
                    }
                }

                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                signinManager.PasswordSignIn(UserName.Text, Password.Text, isPersistent: true, shouldLockout: false);

                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else
            {
                ErrorMessage.Text = IdUserResult.Errors.FirstOrDefault();
            }
        }
    }
}