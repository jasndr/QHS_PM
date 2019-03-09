using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Admin
{
    /// <summary>
    /// @File: GrantForm.aspx.cs
    /// @Author: Yang Rui
    /// @Summary: Grant Form to keep internal tracking information for grants.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018AUG06 - Jason Delos Reyes  -  Added documentation for easier readibility and maintainability.
    ///  2019MAR08 - Jason Delos Reyes  -  Added ability to open grant form by referring to ID
    ///                                    referral in URL.
    /// </summary>
    public partial class GrantForm : System.Web.UI.Page
    {
        GrantManager mgr = GrantManager.Mgr;

        /// <summary>
        /// Binds the page in preparation for view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //int projectId = 0;
            //Int32.TryParse(Request.QueryString["ProjectId"], out projectId);

            string grantIdText = Request.QueryString["Id"];

            if (!Page.IsPostBack)
            {
                BindControl();

                int grantId = 0;
                Int32.TryParse(grantIdText, out grantId);

                if (grantId > 0)
                    OpenGrant(grantId, true);

                //if (projectId > 0)
                //{
                //    //LoadEditScript(true);
                //    btnAdd_Click(null, null);
                //    ddlProject.SelectedValue = projectId.ToString();                    
                //}
            }

        }

        /// <summary>
        /// Binds the dropdown options on page load.
        /// </summary>
        private void BindControl()
        {
            if (!Creator.Equals(string.Empty))
            {
                IDictionary<string, IDictionary<int, string>> controlSource = mgr.GetControlSource();

                PageUtility.BindDropDownList(ddlInvestor, controlSource["ddlInvestor"], String.Empty);

                PageUtility.BindDropDownList(ddlGrantStatus, controlSource["ddlGrantStatus"], String.Empty);
                PageUtility.BindDropDownList(ddlFundStatus, controlSource["ddlFundStatus"], String.Empty);
                PageUtility.BindDropDownList(ddlFundStatus1, controlSource["ddlFundStatus"], "--- Select ---");

                PageUtility.BindDropDownList(ddlProject, controlSource["ddlProject"], string.Empty);

                PageUtility.BindDropDownList(ddlBiostat, controlSource["ddlBiostat"], "--- Select ---");

                PageUtility.BindDropDownList(ddlInternal, controlSource["ddlInternal"], "--- Select ---");

                //GridViewBioStat.DataSource = controlSource["ddlBiostat"];
                //GridViewBioStat.DataBind();

                //GridViewGrant.DataSource = controlSource["GridViewGrant"];
                //GridViewGrant.DataBind();
                BindGridViewBiostat(-1);
            }
        }

        /// <summary>
        /// Gets data form database upon clicking the "Submit" button to 
        /// extract a data table of grants, with the appropriate
        /// parameters specified.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumbit_Click(object sender, EventArgs e)
        {
            BindGridViewGrantAll();
        }

        /// <summary>
        /// Obtains data table information from the database.
        /// </summary>
        private void BindGridViewGrantAll()
        {
            //int piId = -1, biostatId = -1;
            //int fundStatusId = -1, exinternal = -1;

            //Int32.TryParse(ddlInvestor.SelectedValue, out piId);
            //Int32.TryParse(ddlBiostat.SelectedValue, out biostatId);
            //Int32.TryParse(ddlFundStatus1.SelectedValue, out fundStatusId);
            //Int32.TryParse(ddlInternal.SelectedValue, out exinternal);

            //DataTable grantTable = mgr.GetGrantAll(piId, biostatId, fundStatusId, exinternal);

            GridViewGrant.DataSource = GetGrantTable();
            GridViewGrant.DataBind();
        }

        /// <summary>
        /// Obtains grant table based on what has been specified from the
        /// dropdown selections.
        /// </summary>
        /// <returns></returns>
        private DataTable GetGrantTable()
        {
            int piId = -1, biostatId = -1;
            int fundStatusId = -1, exinternal = -1;

            Int32.TryParse(ddlInvestor.SelectedValue, out piId);
            Int32.TryParse(ddlBiostat.SelectedValue, out biostatId);
            Int32.TryParse(ddlFundStatus1.SelectedValue, out fundStatusId);
            Int32.TryParse(ddlInternal.SelectedValue, out exinternal);

            return mgr.GetGrantAll(piId, biostatId, fundStatusId, exinternal);
        }

        /// <summary>
        /// If the id number has been clicked on the data table of grants presented,
        /// the program loads the grant form associated with the grant ID and 
        /// the corresponding row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewGrant_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("editRecord"))
            {
                int grantId = -1;
                int.TryParse(e.CommandArgument as string, out grantId);

                //if (grantId > 0)
                //{
                //    LoadEditGrant(grantId);
                //    //LoadEditScript(true);
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //       "ModalScript", PageUtility.LoadEditScript(true), false);
                //}
                OpenGrant(grantId, false);
            }
        }

        /// <summary>
        /// Loads Grant form with originally/previously entered 
        /// grant information.
        /// </summary>
        /// <param name="grantId">Referred grant</param>
        private void LoadEditGrant(int grantId)
        {
            Grant grant = mgr.GetGrantById(grantId);

            if (grant != null)
            {
                //MyGrant = grant;
                BindEditModal(grant);
            }
        }

        /// <summary>
        /// Populates Grant form from information obtained from
        /// grant instance.
        /// </summary>
        /// <param name="grant">Referred grant.</param>
        private void BindEditModal(Grant grant)
        {
            // Sets grant form with grant information provided.
            SetGrant(grant);

            // Sets Principal Investigator text box for grant form.
            SetGrantPI(grant.GrantPIs);

            // Populates Biostatistician section of grant form.
            BindGridViewBiostat(grant.GrantBiostats);
        }

        /// <summary>
        /// Given the list of QHS faculty, populates the "Biostatistician" form
        /// on the botton, with their associated percentage, fee, how many 
        /// years of grant, and note/commentary.
        /// </summary>
        /// <param name="grantBiostat"></param>
        private void BindGridViewBiostat(ICollection<GrantBiostat> grantBiostat)
        {
            DataTable dt = CreateBiostatTable(grantBiostat, true);
            GridViewBiostat.DataSource = dt;
            GridViewBiostat.DataBind();
        }

        /// <summary>
        /// Deletes Biostatistician row on Grant Form if trash icon/delete button selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewBiostat_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridViewBiostat.Rows[e.RowIndex];
            Label lblId = row.FindControl("lblId") as Label;

            if (lblId != null)
            {
                int grantBiostatId = 0;
                Int32.TryParse(lblId.Text, out grantBiostatId);

                if (grantBiostatId > 0)
                {
                    mgr.DeleteGrantBiostatById(grantBiostatId);
                }
            }

            BindGridViewBiostat(e.RowIndex);
        }

        /// <summary>
        /// Adds row for "Biostatistician" row to add person for the current grant form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddBiostat_Click(object sender, EventArgs e)
        {
            BindGridViewBiostat(-1);
        }

        /// <summary>
        /// Deletes "Biostatistician" (QHS Faculty/Staff) section row based on row selected to delete.
        /// Actually deletes all rows and regenerates entire "Biostatistician" section besides the 
        /// index that was specifed to delete.
        /// </summary>
        /// <param name="deleteRowIndex">Row to delete.</param>
        private void BindGridViewBiostat(int deleteRowIndex)
        {
            DataTable dt = CreateBiostatTable(null, false);
            DataRow dr;

            foreach (GridViewRow gv in GridViewBiostat.Rows)
            {
                if (gv.RowIndex != deleteRowIndex)
                {
                    dr = dt.NewRow();

                    Label rowId = gv.FindControl("lblId") as Label;
                    DropDownList ddl = gv.FindControl("ddlBiostats") as DropDownList;
                    TextBox txtPct = gv.FindControl("TextBoxPct") as TextBox;
                    TextBox txtFee = gv.FindControl("TextBoxFee") as TextBox;
                    TextBox txtNote = gv.FindControl("TextBoxNote") as TextBox;
                    TextBox txtYear = gv.FindControl("TextBoxYear") as TextBox;

                    dr[0] = rowId.Text;
                    dr[1] = ddl.SelectedValue;
                    dr[2] = txtPct.Text;
                    dr[3] = txtFee.Text;
                    dr[4] = txtYear.Text;
                    dr[5] = txtNote.Text;

                    dt.Rows.Add(dr);
                }
            }

            if (dt.Rows.Count == 0 || deleteRowIndex < 0)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            GridViewBiostat.DataSource = dt;
            GridViewBiostat.DataBind();
        }

        /// <summary>
        /// Creates "Biostatistician" (QHS Faculty/Staff) table with the given GrantBiostat (Biostatiscians assoicated with
        /// the instance of the grant) information and creates the grid with pre-filled information from the database.
        /// </summary>
        /// <param name="grantBiostat">Biostatisticians (QHS faculty/staff members) associated with the instance of the grant.</param>
        /// <param name="hasRow">Indicates whether or not to add an empty row to the "Biostatistician" table if no QHS/Faculty
        ///                      have already beeen associated with the referred grant.</param>
        /// <returns></returns>
        private DataTable CreateBiostatTable(ICollection<GrantBiostat> grantBiostat, bool hasRow)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("BiostatId");
            dt.Columns.Add("Pct");
            dt.Columns.Add("Fee");
            dt.Columns.Add("Year");
            dt.Columns.Add("Note");

            if (grantBiostat != null && grantBiostat.Count > 0)
            {
                foreach (var biostat in grantBiostat)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = biostat.Id;
                    dr[1] = biostat.BiostatId;
                    dr[2] = biostat.Pct;
                    dr[3] = biostat.Fee;
                    dr[4] = biostat.Year;
                    dr[5] = biostat.Note;

                    dt.Rows.Add(dr);
                }
            }
            else if (hasRow)
            {
                dt.Rows.Add();
            }

            return dt;
        }

        /// <summary>
        /// Prepopulates data table for "Biostatistician" section of the grant form;
        /// notably the Biostat (QHS Faculty/Staff) dropdown to select a member for grant information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewBiostat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = e.Row.FindControl("ddlBiostats") as DropDownList;

                string biostatId = (e.Row.FindControl("lblBiostatId") as Label).Text;

                if (ddl != null)
                {
                    //ddl.Items.AddRange(ddlBiostat.Items.OfType<ListItem>().ToArray());
                    foreach (ListItem li in ddlBiostat.Items)
                    {
                        ListItem newItem = new ListItem();
                        newItem.Value = li.Value;
                        newItem.Text = li.Text;
                        newItem.Selected = false;

                        if (li.Value == biostatId)
                        {
                            newItem.Selected = true;
                        }

                        ddl.Items.Add(newItem);
                    }

                    //ddl.Items.FindByValue(biostatId).Selected = true;                

                    //ddl.SelectedValue = biostatId;
                    //ddl.Items.Remove(biostatId);

                    //    //if (Convert.ToInt16(biostatId) > 9)
                    //    //    e.Row.Cells[3].BackColor = System.Drawing.Color.Green;
                    //    //else
                    //    //    e.Row.Cells[3].BackColor = System.Drawing.Color.Red;
                }

            }
        }

        /// <summary>
        /// Opens the pop-up page of the grant form.
        /// </summary>
        /// <param name="grantId">Grant ID specified in URL string.</param>
        /// <param name="fromPageLoad">Boolean value to indicate if function called from page load.</param>
        protected void OpenGrant(int grantId, bool fromPageLoad)
        {

            if (grantId > 0)
            {
                BindGridViewGrantAll();

                LoadEditGrant(grantId);

                if (fromPageLoad == true)
                {
                    //StringBuilder sb = new StringBuilder();
                    //sb.Append(@"<script type='text/javascript'>");
                    //sb.Append("$(function() { ");
                    //sb.Append("$('#editModal').modal('show');");
                    //sb.Append(" });");
                    //sb.Append(@"</script>");

                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                    //                "ModalScript", sb.ToString(), false);

                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup()", true);

                }
                else
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                    //   "ModalScript", PageUtility.LoadEditScript(true), false);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#editModal').modal('show');");
                    sb.Append(@"</script>");

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                    "ModalScript", sb.ToString(), false);
                }

            }
        }
        

        /// <summary>
        /// Creates a new form for a new Grant entry form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearEditForm();

            //LoadEditScript(true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(true), false);
        }
        /// <summary>
        /// Saves current grant entry form.
        /// - If the grant was previously saved >> save the current grant into the referred grant instance form the grant ID given.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int grantId = 0;
            Int32.TryParse(lblGrantId.Text, out grantId);

            //Grant grant = MyGrant as Grant;
            Grant newGrant = GetGrant(grantId);

            grantId = mgr.SaveGrant(grantId, newGrant);

            if (grantId > 0)
            {
                mgr.SaveGrantPI(grantId, TextBoxPI.Text.Split(';'), Creator, DateTime.Now);

                List<GrantBiostat> lstBiostat = GetGrantBiostat(grantId);
                mgr.SaveGrantBiostat(grantId, lstBiostat);
            }

            //LoadEditScript(false);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(false), false);

            //if (Request.QueryString.Count > 0)
            //{
            //    //Request.QueryString.Remove("ProjectId");
            //    //Response.Redirect("GrantForm");
            //    string newUrl = Request.Url.AbsolutePath;
            //    Response.Write(newUrl);
            //}

            BindGridViewGrantAll();
        }

        /// <summary>
        /// Obtains list of Biostatisticians (QHS faculty/staff) corresponding to the grant based on the
        /// grant ID specified.
        /// </summary>
        /// <param name="grantId">Referred Grant ID.</param>
        /// <returns>List of Biostatisticians (QHS faculty/staff) corresponding to grant.</returns>
        private List<GrantBiostat> GetGrantBiostat(int grantId)
        {
            List<GrantBiostat> lstBiostat = new List<GrantBiostat>();

            foreach (GridViewRow row in GridViewBiostat.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddl = row.FindControl("ddlBiostats") as DropDownList;
                    Label lblId = row.FindControl("lblId") as Label;
                    TextBox txtPct = row.FindControl("TextBoxPct") as TextBox;
                    TextBox txtFee = row.FindControl("TextBoxFee") as TextBox;
                    TextBox txtNote = row.FindControl("TextBoxNote") as TextBox;
                    TextBox txtYear = row.FindControl("TextBoxYear") as TextBox;

                    int biostatId = 0;
                    int output = 0;
                    decimal outPct = 0.0m;

                    if (ddl != null && txtPct != null && txtFee != null && txtNote != null)
                    {
                        Int32.TryParse(ddl.SelectedValue, out biostatId);

                        if (biostatId != 0)
                        {
                            GrantBiostat biostat = new GrantBiostat()
                            {
                                Id = int.TryParse(lblId.Text, out output) ? output : -1,
                                GrantId = grantId,
                                BiostatId = biostatId,
                                Pct = decimal.TryParse(txtPct.Text, out outPct) ? outPct : default(decimal?),
                                Fee = int.TryParse(txtFee.Text, out output) ? output : default(int?),
                                Year = txtYear.Text,
                                Note = txtNote.Text,
                                Creator = Creator,
                                CreateDate = DateTime.Now
                            };
                            lstBiostat.Add(biostat);
                        }
                    }
                }
            }

            return lstBiostat;
        }

        //private void AddGrantBiostat(Grant grant)
        //{
        //    foreach(GridViewRow row in GridViewBiostat.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            DropDownList ddl = row.FindControl("ddlBiostats") as DropDownList;
        //            TextBox txtPct = row.FindControl("TextBoxPct") as TextBox;
        //            TextBox txtFee = row.FindControl("TextBoxFee") as TextBox;
        //            TextBox txtNote = row.FindControl("TextBoxNote") as TextBox;
        //            TextBox txtYear = row.FindControl("TextBoxYear") as TextBox;

        //            int biostatId = 0;
        //            int output = 0;

        //            if (ddl != null && txtPct != null && txtFee != null && txtNote != null)
        //            {
        //                Int32.TryParse(ddl.SelectedValue, out biostatId);

        //                if (biostatId > 0)
        //                {
        //                    GrantBiostat biostat = new GrantBiostat()
        //                    {
        //                        GrantId = grant.Id,
        //                        BiostatId = biostatId,
        //                        Pct = int.TryParse(txtPct.Text, out output) ? output : default(int?),
        //                        Fee = int.TryParse(txtFee.Text, out output) ? output : default(int?),
        //                        Year = int.TryParse(txtYear.Text, out output) ? output : default(int?),
        //                        Note = txtNote.Text,
        //                        Creator = Creator,
        //                        CreateDate = DateTime.Now
        //                    };
        //                    grant.GrantBiostats.Add(biostat);
        //                }
        //            }
        //        }
        //    }
        //}


        /// <summary>
        /// Fundamentally clears the current grant web form to create a fresh new
        /// grant form for entry.
        /// </summary>
        private void ClearEditForm()
        {
            Grant grant = new Grant();
            grant.Id = -1;
            grant.ProjectId = -1;

            SetGrant(grant);

            SetGrantPI(new List<GrantPI>());

            BindGridViewBiostat(new List<GrantBiostat>());
        }

        /// <summary>
        /// Obtains user information and records this information as the user who created
        /// the grant record.
        /// </summary>
        public string Creator
        {
            get
            {
                return Page.User.Identity.Name;
            }
        }

        /// <summary>
        /// Obtains current grant information of the current instance of a grant.
        /// </summary>
        public object MyGrant
        {
            get
            {
                return Session["Grant"];
            }
            set
            {
                Session["Grant"] = value;
            }
        }

        /// <summary>
        /// Given a grant ID, creates an instance of a Grant from the values 
        /// entered through the grant form and returns this instance.
        /// </summary>
        /// <param name="grantId">Referred Grant ID.</param>
        /// <returns>Instance of Grant obtained from grant form entered on web interface.</returns>
        private Grant GetGrant(int grantId)
        {
            DateTime dt;
            int amt;
            int ddlValue;
            Grant grant = new Grant()
            {
                Id = grantId,
                ProjectId = Int32.TryParse(ddlProject.SelectedValue, out ddlValue) ? ddlValue : -1,
                Collab = TextBoxCollab.Text,
                HealthInit = TextBoxHealthInit.Text,
                NativeHawaiian = chkNativeHawaiian.Checked,
                GrantTitle = TextBoxGrantTitle.Text,
                GrantType = TextBoxGrantType.Text,
                PgmAnn = TextBoxPgmAnn.Text,
                FundAgcy = TextBoxFundAgcy.Text,
                StartDate = DateTime.TryParse(TextBoxStartDate.Text, out dt) ? dt : (DateTime?)null,
                EndDate = DateTime.TryParse(TextBoxEndDate.Text, out dt) ? dt : (DateTime?)null,
                SubDate = DateTime.TryParse(TextBoxSubDate.Text, out dt) ? dt : (DateTime?)null,
                SubDeadline = DateTime.TryParse(TextBoxSubDeadline.Text, out dt) ? dt : (DateTime?)null,
                InitDate = DateTime.TryParse(TextBoxInitDate.Text, out dt) ? dt : (DateTime?)null,
                FundDate = DateTime.TryParse(TextBoxFundDate.Text, out dt) ? dt : (DateTime?)null,
                FirstYrAmt = int.TryParse(TextBoxFirstYrAmt.Text, System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out amt) ? amt : (int?)null,
                TotalAmt = int.TryParse(TextBoxTotalAmt.Text, System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out amt) ? amt : (int?)null,
                TotalCost = int.TryParse(TextBoxTotalCost.Text, System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out amt) ? amt : (int?)null,
                MyGrantNum = int.TryParse(TextBoxMyGrantNum.Text, out amt) ? amt : (int?)null,
                Fundnum = TextBoxFundNum.Text,
                GrantStatus = ddlGrantStatus.SelectedItem.Text,
                FundStatus = ddlFundStatus.SelectedItem.Text,
                IsInternal = chkInternal.Checked,
                Followup = TextBoxFollowup.Text,
                Comment = TextBoxComment.Text
            };

            return grant;
        }

        /// <summary>
        /// Sets the Grant Form with the instance of the grant provided (and presumably obtained from the database.)
        /// </summary>
        /// <param name="grant">Referred instance of a grant.</param>
        private void SetGrant(Grant grant)
        {
            if (grant != null)
            {
                lblGrantId.Text = grant.Id > 0 ? grant.Id.ToString() : string.Empty;
                ddlProject.SelectedValue = grant.ProjectId.ToString();

                TextBoxCollab.Text = grant.Collab;
                TextBoxHealthInit.Text = grant.HealthInit;
                chkNativeHawaiian.Checked = grant.NativeHawaiian;
                TextBoxGrantTitle.Text = grant.GrantTitle;
                TextBoxGrantType.Text = grant.GrantType;
                TextBoxPgmAnn.Text = grant.PgmAnn;
                TextBoxFundAgcy.Text = grant.FundAgcy;
                TextBoxStartDate.Text = grant.StartDate != null ? Convert.ToDateTime(grant.StartDate).ToShortDateString() : "";
                TextBoxEndDate.Text = grant.EndDate != null ? Convert.ToDateTime(grant.EndDate).ToShortDateString() : "";
                TextBoxFirstYrAmt.Text = string.Format("{0:n0}", grant.FirstYrAmt);
                TextBoxTotalAmt.Text = string.Format("{0:n0}", grant.TotalAmt);
                TextBoxTotalCost.Text = string.Format("{0:n0}", grant.TotalCost);

                TextBoxMyGrantNum.Text = grant.MyGrantNum.ToString();
                TextBoxFundNum.Text = grant.Fundnum;

                ddlGrantStatus.ClearSelection();
                if (grant.GrantStatus != null)
                {
                    var grantStatus = ddlGrantStatus.Items.FindByText(grant.GrantStatus);
                    if (grantStatus != null)
                    {
                        grantStatus.Selected = true;
                    }
                }

                ddlFundStatus.ClearSelection();
                if (grant.FundStatus != null)
                {
                    var fundStatus = ddlFundStatus.Items.FindByText(grant.FundStatus);
                    if (fundStatus != null)
                    {
                        fundStatus.Selected = true;
                    }
                }

                TextBoxSubDate.Text = grant.SubDate != null ? Convert.ToDateTime(grant.SubDate).ToShortDateString() : "";
                TextBoxSubDeadline.Text = grant.SubDeadline != null ? Convert.ToDateTime(grant.SubDeadline).ToShortDateString() : "";
                TextBoxInitDate.Text = grant.InitDate != null ? Convert.ToDateTime(grant.InitDate).ToShortDateString() : "";
                TextBoxFundDate.Text = grant.FundDate != null ? Convert.ToDateTime(grant.FundDate).ToShortDateString() : "";

                TextBoxFollowup.Text = grant.Followup;
                TextBoxComment.Text = grant.Comment;

                chkInternal.Checked = grant.IsInternal;

            }
        }

        /// <summary>
        /// Sets the grant project's PI based on the list given.
        /// Most likely obtained from list of PI's corresponding
        /// to the project that the grant is affiliated with.
        /// </summary>
        /// <param name="piList">The given list of investigators.</param>
        private void SetGrantPI(ICollection<GrantPI> piList)
        {
            bool disabled = false;
            StringBuilder sb = new StringBuilder();
            foreach (var pi in piList)
            {
                if (pi.Invest != null)
                {
                    sb.Append(pi.Invest.FirstName);
                    sb.Append(" ");
                    sb.Append(pi.Invest.LastName);
                    sb.Append("; ");

                    if (pi.Id == 0)
                    {
                        disabled = true;
                    }
                }
            }
            TextBoxPI.Text = sb.ToString();

            TextBoxPI.ReadOnly = disabled;
        }

        /// <summary>
        /// Creates an excel file of the list of grants (based on the available, optional parameters of Principal Investigator,
        /// QHS Faculty/Staff, Fund Status, and/or Internal/External grants) that have been displayed on the Tracking > Grant main page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = GetGrantTable();

            FileExport fileExport = new FileExport(this.Response);
            fileExport.ExcelExport(dt, "Grant.xlsx");
        }

    }
}