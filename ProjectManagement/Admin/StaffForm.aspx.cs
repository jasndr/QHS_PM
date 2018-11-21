using System;
using System.Collections.Generic;
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
                /// Populates "Study Area" checkbox grid.
                var qUserRights = context.AspNetRoles.OrderBy(x => ((x.Name == "Super") ? 1 :
                                                                 ((x.Name == "Admin") ? 2 :
                                                                 ((x.Name == "Biostat") ? 3 : 4)))).ToList();


                Repeater rptUserRights = (Repeater)e.Row.FindControl("rptUserRights");
                if (e.Row.RowType == DataControlRowType.DataRow && rptUserRights != null)
                {
                    rptUserRights.DataSource = qUserRights;
                    rptUserRights.DataBind();

                    Label lblUserName = (Label)e.Row.FindControl("lblLogonId");

                    // Check if userName exists in biostats.
                    var listBiostats = context.BioStats.Select(x => x.LogonId).ToList();
                    bool isUserBiostat = (listBiostats.Where(x=>x.Contains(lblUserName.Text)).FirstOrDefault() != null) ? true : false;
                    List<IEnumerable<string>> qRightsOfCurrUser = null;
                    string userName = lblUserName.Text;

                    if (isUserBiostat == true) 
                    {
                        // Obtains list of user roles for row QHS faculty/staff.
                        qRightsOfCurrUser = context.AspNetUsers
                                                            .Where(x => x.UserName == userName)
                                                            .Select(y => y.AspNetRoles.Select(z => z.Name)).ToList();
                    }
                    

                    //Make checkbox disabled in view; make appropriate user rights checked.
                    foreach (RepeaterItem item in rptUserRights.Items)
                    {
                        CheckBox checkbox = item.FindControl("chkId") as CheckBox;
                        if (checkbox != null)
                        {
                            // Disables checkbox in intial page load.
                            checkbox.Enabled = false;

                            // Checks checkbox if the current checkbox exist in rights of current user.
                            var currRights = checkbox.Text;

                            // if checkbox.text is one of the options in [qRightsOfCurrentuser]
                            
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
                lblMsg.Text = "Saved successfully.";
            }
        }
    }
}