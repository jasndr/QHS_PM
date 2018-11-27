using System;
using System.Collections.Generic;
using System.Drawing;//
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Admin
{
    /// <summary>
    /// @File: StaffForm.aspx.cs
    /// @FrontEnd: StaffForm.aspx
    /// @Author: Yang Rui
    /// @Summary: List of QHS staff and account privileges of Project Tracking System.
    /// 
    ///           Lists the staff, their degree type for billing (phd vs. ms), email address, logon user name,
    ///           and date when their account expires. The admin team has the ability to view and 
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018NOV16 - Jason Delos Reyes  -  Added comments/documentation for easier legibility and
    ///                                    easier data structure view and management.
    ///  2018NOV20 - Jason Delos Reyes  -  Added "User Rights" view to see which user rights
    ///                                    (Super/Admin/Biostat/Guest) to the corresponds to each QHS faculty/staff.
    ///  2018NOV26 - Jason Delos Reyes  -  Fixed "edit" view issue where user rights were unchecked upon 
    ///                                    editing the current user.
    ///                                 -  Changed "update success message" into Javascript pop-up.
    ///                                 -  Removed "user rights" checkboxes for faculty/staff without tracking system accounts.
    ///  2018NOV27 - Jason Delos Reyes  -  Enabled "user rights" feature to add/remove user rights privileges on Admin/Staff page.
    ///  </summary>
    public partial class StaffForm : System.Web.UI.Page
    {
        /// <summary>
        /// Prepares the page upon load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }

        /// <summary>
        /// Loads the list of QHS faculty/staff on the table grid.
        /// </summary>
        private void BindGrid()
        {
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                /// Loads list of QHS faculty/staff there is at least 1 PI in the system.
                /// Otherwise, a blank table is provided.
                if (context.Invests.Count() > 0)
                {
                    var query = context.BioStats
                           .OrderBy(d => d.Id);

                    GridView1.DataSource = query.ToList();

                    /// Populates "User Rights" checkbox grid in Admin/StaffForm page.
                    var qUserRights = context.AspNetRoles.OrderBy(x => ((x.Name == "Super") ? 1 :
                                                                     ((x.Name == "Admin") ? 2 :
                                                                     ((x.Name == "Biostat") ? 3 : 4)))).ToList();

                    GridView1.DataBind();



                }
                else
                {
                    var obj = new List<Invest>();
                    obj.Add(new Invest());
                    // Bind the DataTable which contain a blank row to the GridView
                    GridView1.DataSource = obj;
                    GridView1.DataBind();
                    int columnsCount = GridView1.Columns.Count;
                    GridView1.Rows[0].Cells.Clear();// clear all the cells in the row
                    GridView1.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
                    GridView1.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

                    //You can set the styles here
                    GridView1.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    GridView1.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
                    GridView1.Rows[0].Cells[0].Font.Bold = true;
                    //set No Results found to the new added cell
                    GridView1.Rows[0].Cells[0].Text = "NO RESULT FOUND!";
                }
            }
        }

        /// <summary>
        /// For adding new entries, if the current user is a super user (that has access to 
        /// "Admin" tab on tracking system), a new QHS faculty/staff member is added 
        /// to the "Biostat" table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Page.User.IsInRole("Super"))
            {
                if (e.CommandName == "InsertNew")
                {
                    GridViewRow row = GridView1.FooterRow;

                    TextBox txtName = row.FindControl("txtNameNew") as TextBox;
                    TextBox txtType = row.FindControl("txtTypeNew") as TextBox;
                    TextBox txtEmail = row.FindControl("txtEmailNew") as TextBox;
                    TextBox txtLogonId = row.FindControl("txtLogonIdNew") as TextBox;
                    TextBox txtEndDate = row.FindControl("txtEndDateNew") as TextBox;

                    if (txtName != null && txtEmail != null && txtLogonId != null)
                    {
                        using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                        {
                            int largestId = context.BioStats.OrderByDescending(b => b.Id).FirstOrDefault(g => g.Id != 99).Id + 1;

                            BioStat biostat = new BioStat()
                            {
                                //Id = largestId, // <- Ignored by the system!!!
                                Name = txtName.Text,
                                Type = txtType.Text,
                                Email = txtEmail.Text,
                                LogonId = txtLogonId.Text,
                                EndDate = DateTime.Parse(txtEndDate.Text),
                                //BitValue = (long)Math.Pow(2, largestId) // <- To be added once Id can be inserted via Identity Insert

                            };

                            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT BioStats ON");

                            context.BioStats.Add(biostat);

                            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT BioStats OFF");

                            WriteInvest(context, biostat);

                            BindGrid();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Allows row to be edited upon clicking edit button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;


            //------------------------------------

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                /// Populates "User Rights" checkbox grid in Admin/StaffForm page.
                var qUserRights = context.AspNetRoles.OrderBy(x => ((x.Name == "Super") ? 1 :
                                                                 ((x.Name == "Admin") ? 2 :
                                                                 ((x.Name == "Biostat") ? 3 : 4)))).ToList();



                Repeater rptUserRights = (Repeater)GridView1.Rows[e.NewEditIndex].FindControl("rptUserRights");
                if (rptUserRights != null)
                {
                    rptUserRights.DataSource = qUserRights;
                    rptUserRights.DataBind();

                    Label lblUserName = (Label)GridView1.Rows[e.NewEditIndex].FindControl("lblLogonId");

                    
                    string userName = lblUserName.Text;

                    // Check if Biostat is registered user.
                    var listRgstdUsers = context.AspNetUsers.Select(x => x.UserName).ToList();
                    bool isBiostatRegistered = false;

                    isBiostatRegistered = (listRgstdUsers.Where(x => x.Contains(userName)).FirstOrDefault() != null) ? true : false;

                    List<IEnumerable<string>> qRightsOfCurrUser = null;

                    if (isBiostatRegistered == true)
                    {
                        // Obtains list of user roles for row QHS faculty/staff.
                        qRightsOfCurrUser = context.AspNetUsers
                                                            .Where(x => x.UserName == userName)
                                                            .Select(y => y.AspNetRoles.Select(z => z.Name)).ToList();

                        //Make appropriate user rights checked.
                        foreach (RepeaterItem item in rptUserRights.Items)
                        {
                            CheckBox checkbox = item.FindControl("chkId") as CheckBox;
                            if (checkbox != null)
                            {

                                // Checks checkbox if the current checkbox exist in rights of current user.
                                var currRights = checkbox.Text;

                                bool doesBiostathaveRights = (currRights != null && qRightsOfCurrUser.Where(x => x.Contains(currRights)).FirstOrDefault() != null)
                                                                ? true : false;

                                if (qRightsOfCurrUser.Count > 0 && doesBiostathaveRights == true)
                                {
                                    checkbox.Checked = true;
                                }

                            }


                        }
                    }

                }

            }

            //------------------------------------

            BindGrid();

        }

        /// <summary>
        /// Prevents updating the table grid row if items are canceled and refreshes the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        /// <summary>
        /// Upon updates the Biostat table in the database based on the
        /// values used in the tracking system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Page.User.IsInRole("Super"))
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];

                Label lblId = row.FindControl("lblId") as Label;
                TextBox txtName = row.FindControl("txtName") as TextBox;
                TextBox txtType = row.FindControl("txtType") as TextBox;
                TextBox txtEmail = row.FindControl("txtEmail") as TextBox;
                TextBox txtLogonId = row.FindControl("txtLogonId") as TextBox;
                TextBox txtEndDate = row.FindControl("txtEndDate") as TextBox;

                Repeater rptUserRights = row.FindControl("rptUserRights") as Repeater;


                if (txtName != null && txtLogonId != null && txtEmail != null)
                {
                    using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                    {
                        int biostatId = Convert.ToInt32(lblId.Text);
                        BioStat biostat = context.BioStats.First(x => x.Id == biostatId);
                        biostat.Name = txtName.Text;
                        biostat.Type = txtType.Text;
                        biostat.Email = txtEmail.Text;
                        biostat.LogonId = txtLogonId.Text;
                        biostat.EndDate = DateTime.Parse(txtEndDate.Text);



                        // Update the user privileges
                        // (Replace or Add/Delete as necessary

                        //var selectedRights = rptUserRights.check

                        var qRightsOfCurrUser = context.AspNetUsers
                                                                .Where(x => x.UserName == txtLogonId.Text)
                                                                .Select(y => y.AspNetRoles.Select(z => z.Name)).ToList();

                        // For each of the user rights (Super, Admin, Biostat, ...),
                        // If there is a match in qRightsOfCurrentUser
                        // If there is not already in database
                        foreach (RepeaterItem item in rptUserRights.Items)
                        {
                            CheckBox checkbox = item.FindControl("chkId") as CheckBox;
                            if (checkbox != null)
                            {

                                var currRights = checkbox.Text;
                                bool markedRights = checkbox.Checked == true ? true : false;

                                AspNetUser biostatUserAcct = context.AspNetUsers.First(x => x.UserName == biostat.LogonId);
                                AspNetRole currAcctRole = context.AspNetRoles.First(x => x.Name == currRights);

                                if (markedRights)
                                {
                                    //Add Role if doesn't exist.
                                    if (biostatUserAcct.AspNetRoles.FirstOrDefault(x => x.Name == currRights) == null)
                                    {
                                        biostatUserAcct.AspNetRoles.Add(currAcctRole);
                                    }
                                    
                                } else
                                {
                                    // Delete Role if currently exists.
                                    if (biostatUserAcct.AspNetRoles.FirstOrDefault(x => x.Name == currRights) != null)
                                    {
                                        biostatUserAcct.AspNetRoles.Remove(currAcctRole);
                                    }
                                }

                            }
                        }
                        // Make appropriate user rights checked.
                        foreach (RepeaterItem item in rptUserRights.Items)
                        {
                            CheckBox checkbox = item.FindControl("chkId") as CheckBox;
                            if (checkbox != null)
                            {
                               
                                // Checks checkbox if the current checkbox exist in rights of current user.
                                var currRights = checkbox.Text;

                                bool doesBiostathaveRights = (currRights != null && qRightsOfCurrUser.Where(x => x.Contains(currRights)).FirstOrDefault() != null)
                                                                ? true : false;

                                if (qRightsOfCurrUser.Count > 0 && doesBiostathaveRights == true)
                                {
                                    checkbox.Checked = true;
                                }

                            }

                        }
                        /****/


                        WriteInvest(context, biostat);

                        GridView1.EditIndex = -1;
                        BindGrid();

                    }
                }
            }
        }

        /// <summary>
        /// Prepares row data (only need user rights checkboxes for now).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                /// Populates "User Rights" checkbox grid in Admin/StaffForm page.
                var qUserRights = context.AspNetRoles.OrderBy(x => ((x.Name == "Super") ? 1 :
                                                                 ((x.Name == "Admin") ? 2 :
                                                                 ((x.Name == "Biostat") ? 3 : 4)))).ToList();


                Repeater rptUserRights = (Repeater)e.Row.FindControl("rptUserRights");
                if (e.Row.RowType == DataControlRowType.DataRow && rptUserRights != null)
                {
                    rptUserRights.DataSource = qUserRights;
                    rptUserRights.DataBind();

                    Label lblUserName = (Label)e.Row.FindControl("lblLogonId");
                    TextBox txtUserName = (TextBox)e.Row.FindControl("txtLogonId");
                    string userName = lblUserName != null ? lblUserName.Text : txtUserName != null ? txtUserName.Text : "";
                    

                    // Check if Biostat is registered user.
                    var listRgstdUsers = context.AspNetUsers.Select(x => x.UserName).ToList();
                    bool isBiostatRegistered = false;

                    if (userName != "")
                    {
                        isBiostatRegistered = (listRgstdUsers.Where(x => x.Contains(userName)).FirstOrDefault() != null) ? true : false;

                        List<IEnumerable<string>> qRightsOfCurrUser = null;

                        if (isBiostatRegistered == true)
                        {
                            // Obtains list of user roles for row QHS faculty/staff.
                            qRightsOfCurrUser = context.AspNetUsers
                                                                .Where(x => x.UserName == userName)
                                                                .Select(y => y.AspNetRoles.Select(z => z.Name)).ToList();

                            // Make appropriate user rights checked.
                            foreach (RepeaterItem item in rptUserRights.Items)
                            {
                                CheckBox checkbox = item.FindControl("chkId") as CheckBox;
                                if (checkbox != null)
                                {
                                    // Disables checkbox in intial page load.
                                    // checkbox.Enabled = false; (now handled on front end)

                                    // Checks checkbox if the current checkbox exist in rights of current user.
                                    var currRights = checkbox.Text;

                                    bool doesBiostathaveRights = (currRights != null && qRightsOfCurrUser.Where(x => x.Contains(currRights)).FirstOrDefault() != null)
                                                                    ? true : false;

                                    if (qRightsOfCurrUser.Count > 0 && doesBiostathaveRights == true)
                                    {
                                        checkbox.Checked = true;
                                    }

                                }

                            }

                        }
                        else
                        {
                            // Hides checkboxes if there is no registered account for the faculty/staff member.
                            rptUserRights.Visible = false;
                        }

                        

                    }
                    else
                    {
                        e.Row.Cells[7].BackColor = Color.IndianRed;
                    }
                    
                    
                }

            }
        }

        /// <summary>
        /// NOTE: Currently NOT being used. Populates repeater of checkboxes in "User Rights" section.
        /// </summary>
        /// <param name="enable">Boolean value whether or not the checkboxes should be enabled or disabled for use.</param>
        //private void EnableUserRights(bool enable)
        //{

        //}

        /// <summary>
        /// Checks to see if a valid Biostat entry has been entered into the tracking
        /// system based on the entry saved on the Staff form.
        /// </summary>
        /// <param name="context">Database being referred to.</param>
        /// <param name="biostat">Instance of the BioStat table in the tracking system.</param>
        private void WriteInvest(ProjectTrackerContainer context, BioStat biostat)
        {
            var result = context.Entry(biostat).GetValidationResult();
            if (!result.IsValid)
            {
                lblMsg.Text = result.ValidationErrors.First().ErrorMessage;
            }
            else
            {
                context.SaveChanges();
                //lblMsg.Text = "Saved successfully.";

                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Faculty/staff saved successfully.');", true);

            }
        }
    }
}