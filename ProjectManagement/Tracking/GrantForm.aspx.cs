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
    public partial class GrantForm : System.Web.UI.Page
    {
        GrantManager mgr = GrantManager.Mgr;
        protected void Page_Load(object sender, EventArgs e)
        {
            //int projectId = 0;
            //Int32.TryParse(Request.QueryString["ProjectId"], out projectId);

            if (!Page.IsPostBack)
            {
                BindControl();

                //if (projectId > 0)
                //{
                //    //LoadEditScript(true);
                //    btnAdd_Click(null, null);
                //    ddlProject.SelectedValue = projectId.ToString();                    
                //}
            }          

        }

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

        protected void btnSumbit_Click(object sender, EventArgs e)
        {
            BindGridViewGrantAll();
        }

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

        protected void GridViewGrant_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("editRecord"))
            {               
                int grantId = -1;
                int.TryParse(e.CommandArgument as string, out grantId);

                if (grantId > 0)
                {
                    LoadEditGrant(grantId);
                    //LoadEditScript(true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(true), false);
                }
            }
        }

        private void LoadEditGrant(int grantId)
        {
            Grant grant = mgr.GetGrantById(grantId);            

            if (grant != null)
            {
                //MyGrant = grant;
                BindEditModal(grant);
            }
        }

        private void BindEditModal(Grant grant)
        {
            SetGrant(grant);

            SetGrantPI(grant.GrantPIs);

            BindGridViewBiostat(grant.GrantBiostats);
        }        

        private void BindGridViewBiostat(ICollection<GrantBiostat> grantBiostat)
        {
            DataTable dt = CreateBiostatTable(grantBiostat, true);
            GridViewBiostat.DataSource = dt;
            GridViewBiostat.DataBind();
        }

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

        protected void btnAddBiostat_Click(object sender, EventArgs e)
        {
            BindGridViewBiostat(-1);
        }

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
                foreach(var biostat in grantBiostat)
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
               

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearEditForm();

            //LoadEditScript(true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(true), false);
        }

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

        private void ClearEditForm()
        {
            Grant grant = new Grant();
            grant.Id = -1;
            grant.ProjectId = -1;

            SetGrant(grant);

            SetGrantPI(new List<GrantPI>());

            BindGridViewBiostat(new List<GrantBiostat>());
        }       
        
        public string Creator
        {
            get
            {
                return Page.User.Identity.Name;
            }
        }

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

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = GetGrantTable();

            FileExport fileExport = new FileExport(this.Response);
            fileExport.ExcelExport(dt, "Grant.xlsx");
        }

    }
}