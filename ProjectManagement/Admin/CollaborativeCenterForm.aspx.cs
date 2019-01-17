using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Admin
{
    /// <summary>
    /// @File: CollaborativeCenterForm.aspx.cs
    /// @FrontEnd: CollaborativeCenterForm.aspx
    /// @Author: Yang Rui
    /// @Summary: Collaborative Center Form of the Admin section of the Project Tracking System.
    /// 
    ///           Creates and maintains a record of collaboration centers that QHS have worked with
    ///           in faculty and staff projects.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018JUL23 - Jason Delos Reyes  -  Added dropdown of projects affiliated with collaborative centers so it would be
    ///                                    easier for the admin team to systematically look for projects without having
    ///                                    to go through multiple pages in the system. Also added documentation for easier
    ///                                    readibility and clarity of the system.
    ///  2018JUL27 - Jason Delos Reyes  -  Added dropdown list of projects associated with the collaboration center so it
    ///                                    would be easier for the admin team to quickly glace through the list of projects
    ///                                    connected to that specific collaboration center.
    ///  2018AUG02 - Jason Delos Reyes  -  Added functionality so that the page will automatically go to the Project page being
    ///                                    referred to when selecting the specific project from the recently-added projects
    ///                                    dropdown page affiliated with the selected Collaborative Center.
    ///  2018AUG16 - Jason Delos Reyes  -  Formulated the "list of projects associated with collaborative center" dropdown to
    ///                                    temporarily force the School of Nursing & Dental Hygiene to generate its own 
    ///                                    associated list of students until a more universal solution can be implemented.
    ///  2018SEP13 - Jason Delos Reyes  -  Added a "list of client agreements associated with collaborative center" dropdown
    ///                                    but did not yet add "go to page" functionality for client agreements.
    ///  2018SEP18 - Jason Delos Reyes  -  Added "go to page" functionality for client agreement dropdown. Also added drop down 
    ///                                    for Collaborative Centers on the front page with the "go to page" functionality also
    ///                                    included, despite the fact that users can already easily click the "edit" button
    ///                                    in the grid of listed Collaborative Centers.
    ///  2018SEP21 - Jason Delos Reyes  -  Configured the "update" button so that when the user clicks on it, the system 
    ///                                    updates the information in the database but doesn't close the window.
    ///  2019JAN11 - Jason Delos Reyes  -  Added functionality to partition Collaborative Centers between all, active, 
    ///                                    and inactive collaborative centers.
    ///  2019JAN14 - Jason Delos Reyes  -  Fixed errors that prevented page fron switching between all, active, and inactive
    ///                                    collaborative centers.
    ///                                 -  Fixed the issue that would open an empty form when switching between all, active, and 
    ///                                    inactive collaborative centers using the radio button options.
    ///  2019JAN15 - Jason Delos Reyes  -  Replaced ordinary dropdown to a searchable one for on the main
    ///                                    page with the list of collaborative centers.
    /// </summary>
    public partial class CollaborativeCenterForm : System.Web.UI.Page
    {
       
        /// <summary>
        /// Prepares collaboration center page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindRptCC();
            }
        }

        /// <summary>
        /// Customized "Close" function to hide the projects list dropdown
        /// when modal window view has been closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            collabCtrProjSxn.Visible = false;
            collabCtrClientAgmtSxn.Visible = false;
        }

        /// <summary>
        /// Saves the current collaboration center form entry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ccId = 0;
            Int32.TryParse(lblCCId.Text, out ccId);

            CollabCtr cc = GetCollabCtr(ccId);

            if (SaveCC(cc) < 0)
            {
                //error handling
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript("update"), false);

            BindRptCC(ccId);
        }

        /// <summary>
        /// Saves current Collaboration Center form into the database by either updating the 
        /// exisiting entry with the same name, or adding a new entry into the database.
        /// </summary>
        /// <param name="cc">Collaboration Center form newly entered on the web form.</param>
        /// <returns>Referred collaboration center id (it will be a new ID if newly entered into database).</returns>
        private int SaveCC(CollabCtr cc)
        {
            int ccid = -1;

            try
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    if (cc.Id > 0) //update
                    {
                        var prevCC = db.CollabCtr.FirstOrDefault(c => c.Id == cc.Id);

                        if (prevCC != null)
                        {
                            db.Entry(prevCC).CurrentValues.SetValues(cc);
                        }
                    }
                    else //new
                    {
                        db.CollabCtr.Add(cc);
                    }

                    db.SaveChanges();
                    ccid = cc.Id;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //
            }

            return ccid;
        }

        /// <summary>
        /// Obtains all fields necessary for the Collaboration Report page.
        /// </summary>
        private void BindRptCC()
        {

            DataTable ccTable = GetCollabCtrAll();

            rptCC.DataSource = ccTable;
            rptCC.DataBind();

            LoadCCForm(-1);

        }

        /// <summary>
        /// Refreshes and loads page based on current collab center report.
        /// </summary>
        /// <param name="id">Referred collab center id.</param>
        private void BindRptCC(int collabCtrId)
        {

            DataTable ccTable = GetCollabCtrAll();

            rptCC.DataSource = ccTable;
            rptCC.DataBind();

            LoadCCForm(collabCtrId);

        }

        /// <summary>
        /// Obtains all Collaboration Centers from the database.
        /// </summary>
        /// <returns>Table of all collaboration centers that have previously been entered.</returns>
        private DataTable GetCollabCtrAll()
        {
            DataTable dt = new DataTable("collabCtrTable");

            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            dt.Columns.Add("NameAbbrv", System.Type.GetType("System.String"));
            dt.Columns.Add("StartDate", System.Type.GetType("System.String"));
            dt.Columns.Add("EndDate", System.Type.GetType("System.String"));
            dt.Columns.Add("BillingSchedule", System.Type.GetType("System.String"));
            dt.Columns.Add("NextInvoiceDate", System.Type.GetType("System.String"));

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var query = db.CollabCtr.Where(x=>x.Id > 0);

                //if (collabCenterType.SelectedValue == "active")
                //{
                //    // Shows active Collaborative Centers (with expiration date OR)
                //    query = db.CollabCtr.Where(x => x.EndDate != null);
                //}

                switch (collabCenterType.SelectedValue)
                {
                    case "active": // Shows active Collaborative Centers (with no end date OR end date > today)
                        query = query.Where(x => x.EndDate == null || (x.EndDate != null && x.EndDate >= DateTime.Today));
                        break;
                    case "inactive":
                        query = query.Where(x=>x.EndDate != null && x.EndDate < DateTime.Today);
                        break;
                    default:
                        break;
                }

                foreach (var cc in query.Where(t => t.Id  > 0).OrderBy(c => c.Id).ToList())
                {
                    DataRow dr = dt.NewRow();

                    dr[0] = cc.Id;
                    dr[1] = cc.Name;
                    dr[2] = cc.NameAbbrv;
                    dr[3] = Convert.ToDateTime(cc.StartDate).ToShortDateString();
                    dr[4] = cc.EndDate != null ? Convert.ToDateTime(cc.EndDate).ToShortDateString() : "";
                    dr[5] = cc.BillingSchedule;
                    dr[6] = cc.NextInvoiceDate != null ? Convert.ToDateTime(cc.NextInvoiceDate).ToShortDateString() : "";

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }        

        /// <summary>
        /// Opens the specific Collaboration Center form based on the "Edit" button row specified in the table of
        /// Collaboration Centers listed from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptCC_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {
            if (((Button)e.CommandSource).Text.Equals("Edit"))
            {
                
                lblCCId.Text = ((Button)e.CommandSource).CommandArgument;

                int id = 0;
                int.TryParse(lblCCId.Text, out id);

                if (id > 0)
                {
                    LoadCCForm(id);
                }
            }
        }

        /// <summary>
        /// Loads Collaborative Center form given id.
        /// </summary>
        /// <param name="id">Referred collab center id.</param>
        private void LoadCCForm(int id)
        {
            collabCtrProjSxn.Visible = true;
            collabCtrClientAgmtSxn.Visible = true;

            var dropDownSource = new Dictionary<int, string>();
            var dropDownSourcewString = new Dictionary<string, string>();

            /// Populates dropdowns of projects and client agreements associated with the collaboration center.
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                /// List of Collab Centers
                dropDownSource = db.CollabCtr
                                   .OrderBy(c => c.NameAbbrv)
                                   .Where(c => c.Id > 0)
                                   .Select(x => new { x.Id, FullName = (x.NameAbbrv + " | " + x.Name) })
                                   .ToDictionary(c => c.Id, c => c.FullName);
                PageUtility.BindDropDownList(ddlCollab, dropDownSource, string.Empty);

                /// Pulls list of projects associated with collaborative center OR
                /// if collaborative center is SONDH, pulls the projects with 
                /// "MOU" listed as the grant payment.
                dropDownSource = db.Project2
                                      .GroupJoin(
                                         db.JabsomAffils,
                                         p => p.GrantDepartmentFundingType,
                                         ja => ja.Id,
                                         (p, ja) => new { p, ja })
                                      .Where(y => y.p.ClientAgmt.FirstOrDefault().CollabCtrId == id
                                             || (id == 116
                                                  && y.ja.FirstOrDefault().Name == "School of Nursing & Dental Hygiene")
                                                  && (y.p.IsMOU == 1 || y.p.GrantOther.Contains("MOU") || (y.p.AknOther.Contains("MOU"))))
                                      .OrderByDescending(y => y.p.Id)
                                      .Select(x => new { x.p.Id, FullName = (x.p.Id + " " + x.p.Title).Substring(0, 150) })
                                      .Distinct()
                                      .ToDictionary(d => d.Id, d => d.FullName);


                PageUtility.BindDropDownList(ddlCollabCtrProjects, dropDownSource, "-- List of Projects for Collaborative Center --");

                /// Pulls list of Client Agreements associated with collaborative center.
                dropDownSourcewString = db.ClientAgmt
                                     .Join(db.CollabCtr, ca => ca.CollabCtrId, cct => cct.Id, (ca, cct) => new { ca, cct })
                                     .Where(z => z.ca.CollabCtrId == id)
                                     .OrderByDescending(y => y.ca.Id)
                                     .Select(x => new { /*x.ca.Id*/x.ca.AgmtId, FullName = (x.ca.Id + " - " + x.ca.AgmtId + " - " + x.ca.ProjectPhase + " - Project ID " + x.ca.Project2Id).Substring(0, 150) })
                                     .Distinct()
                                     .ToDictionary(d => d.AgmtId, d => d.FullName);

                PageUtility.BindDropDownList(ddlCollabCtrClientAgmts, dropDownSourcewString, "-- List of Client Agreements for Collaborative Center --");
                
            }


            CollabCtr cc = GetCollabCtrById(id);

            if (cc != null)
            {
                SetCollabCtr(cc);

                if (cc.Id > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                               "ModalScript", PageUtility.LoadEditScript(true), false);
                }
            }
        }

        /// <summary>
        /// Using provided collaboration center ID, obtains the collaboration center entry from the form.
        /// </summary>
        /// <param name="ccId">Referred collaboration center ID.</param>
        /// <returns>New/updated collaboration center form, to be saved on the database end.</returns>
        private CollabCtr GetCollabCtr(int ccId)
        {
            DateTime dt;
            CollabCtr cc = new CollabCtr()
            {
                Id = ccId,
                Name = txtCCName.Value,
                NameAbbrv = txtCCAbbrv.Value,
                StartDate = DateTime.TryParse(txtStartDate.Text, out dt) ? dt : DateTime.Now,
                EndDate = DateTime.TryParse(txtEndDate.Text, out dt) ? dt : (DateTime?)null,
                NextInvoiceDate = DateTime.TryParse(txtNextInvoiceDate.Text, out dt) ? dt : (DateTime?)null,
                Contact = txtName.Value,
                Phone = txtPhone.Value,
                Email = txtEmail.Value,
                FiscalContact = txtFiscalName.Value,
                FiscalPhone = txtFiscalPhone.Value,
                FiscalEmail = txtFiscalEmail.Value,
                FiscalMail1 = txtFiscalMail1.Value,
                FiscalMail2 = txtFiscalMail2.Value,
                City = txtFiscalCity.Value,
                State = ddlFiscalState.Value,
                ZipCode = txtFiscalZip.Value,
                BillingSchedule = ddlBillingSchedule.Value,
                BillOther = ddlBillingSchedule.Value == "Other" ? txtBillOther.Text : "",
                Memo = txtMemo.Value,
                Creator = Page.User.Identity.Name,
                CreateDate = DateTime.Now
            };

            return cc;
        }

        /// <summary>
        /// Prints collaboration center from the provided collaboration center to the web form for review.
        /// </summary>
        /// <param name="cc">Referred collaboration center instance.</param>
        private void SetCollabCtr(CollabCtr cc)
        {
            lblCCId.Text = cc.Id > 0 ? cc.Id.ToString() : "";
            txtCCName.Value = cc.Name;
            txtCCAbbrv.Value = cc.NameAbbrv;
            txtStartDate.Text = cc.StartDate != default(DateTime) ? Convert.ToDateTime(cc.StartDate).ToShortDateString() : "";
            txtEndDate.Text = cc.EndDate != null ? Convert.ToDateTime(cc.EndDate).ToShortDateString() : "";
            txtNextInvoiceDate.Text = cc.NextInvoiceDate != null ? Convert.ToDateTime(cc.NextInvoiceDate).ToShortDateString() : "";
            txtName.Value = cc.Contact;
            txtPhone.Value = cc.Phone;
            txtEmail.Value = cc.Email;
            txtFiscalName.Value = cc.FiscalContact;
            txtFiscalPhone.Value = cc.FiscalPhone;
            txtFiscalEmail.Value = cc.FiscalEmail;
            txtFiscalMail1.Value = cc.FiscalMail1;
            txtFiscalMail2.Value = cc.FiscalMail2;
            txtFiscalCity.Value = cc.City;
            ddlFiscalState.Value = cc.State != null ? cc.State : "";
            txtFiscalZip.Value = cc.ZipCode;
            ddlBillingSchedule.Value =  cc.BillingSchedule != null ? cc.BillingSchedule : "";
            txtBillOther.Text = cc.BillOther;
            txtMemo.Value = cc.Memo;
        }

        /// <summary>
        /// Finds collaboration center using the ID and returns that collaboration center instance.
        /// </summary>
        /// <param name="id">Referred collaboration center ID.</param>
        /// <returns>Collaboration Center record from the obtained collaboration center ID.</returns>
        private CollabCtr GetCollabCtrById(int id)
        {
            CollabCtr ctr = null;

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                ctr = db.CollabCtr.FirstOrDefault(t => t.Id == id);
            }

            return ctr;
        }

        /// <summary>
        /// Prepares a new form for a new Collaboration Center entry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearEditForm();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(true), false);
        }

        /// <summary>
        /// Clears web form and prepares form for a new Collaobration Center Form entry.
        /// </summary>
        private void ClearEditForm()
        {
            CollabCtr newCtr = new CollabCtr();
            newCtr.StartDate = default(DateTime);

            SetCollabCtr(newCtr);
        }

        /// <summary>
        /// Add an agreement that will tie to the current collaboration center.
        /// Clicking this button will automatically go to a *new* Client Agreement Form entry page
        /// with the pertaining collaborative center information already pre-filled and necessary ID # auto-incremented.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddAgreement_Click(object sender, EventArgs e)
        {
            int ccId = 0;
            Int32.TryParse(lblCCId.Text, out ccId);

            if (ccId > 0 && txtCCAbbrv.Value != null)
            {
                Response.Redirect(String.Format("ClientAgreementForm?Id={0}&Name={1}",ccId, txtCCAbbrv.Value));
            }
        }

        /// <summary>
        /// Add new invoice entry that will tie to the current collaboration center.
        /// Clicking this button will automatically go to a *new* invoice form with the 
        /// pertinent collaborative center information already pre-filled and necessary ID # auto-incremented.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddInvoice_Click(object sender, EventArgs e)
        {
            int ccId = 0;
            Int32.TryParse(lblCCId.Text, out ccId);

            if (ccId > 0 && txtCCAbbrv.Value != null)
            {
                Response.Redirect(String.Format("InvoiceForm?CCId={0}&CCName={1}",ccId, txtCCAbbrv.Value));
            }
        }

        /// <summary>
        /// Updates list of collaborative centers between
        ///     - currently active (no end date noted or have not approched end date),
        ///     - currently inactive (end date surpassed),
        ///     - or all.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void collabCenterType_Changed(Object sender, EventArgs e)
        {
            BindRptCC();
        }

        /// <summary>
        /// On the main page with the list of Collaborative Centers, if the dropdown is changed,
        /// the program will redirect to the one selected in the dropdown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCollab_Changed(Object sender, EventArgs e)
        {

            int ccId = 0;
            ccId = Int32.TryParse(ddlCollab.SelectedValue, out ccId) ? ccId : -1;

            if (ccId > 0)
            {
                // Open Collab Center form with information loaded from extracted Collab Ctr Id.
                //CollabCtr cc = GetCollabCtrById(ccId);

                //if (cc != null)
                //{
                //    SetCollabCtr(cc);

                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //               "ModalScript", PageUtility.LoadEditScript(true), false);
                //}
                LoadCCForm(ccId);
            }


        }

        /// <summary>
        /// When "Project" dropdown is changed, the system will automatically redirect to the project form of the project
        /// being referred to.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCollabCtrProjects_Changed(Object sender, EventArgs e)
        {

            int projectId = 0;
            projectId = Int32.TryParse(ddlCollabCtrProjects.SelectedValue, out projectId) ? projectId: -1;

            if (projectId > 0)
            {
                Response.Redirect(String.Format("~/ProjectForm2?Id={0}", projectId));
            }


        }

        /// <summary>
        /// When "Client Agreements" dropdown is changed, the system will automatically redirect to the client agreement form
        /// being referred to.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCollabCtrClientAgmts_Changed(Object sender, EventArgs e)
        {

            //int clientAgmtId = 0;
            //clientAgmtId = Int32.TryParse(ddlCollabCtrClientAgmts.SelectedValue, out clientAgmtId) ? clientAgmtId : -1;

            //if (clientAgmtId > 0)
            //{
            string clientAgmtId = ddlCollabCtrClientAgmts.SelectedValue;
            Response.Redirect(String.Format("~/Admin/ClientAgreementForm?ClientAgmtId={0}", clientAgmtId));
            //}


        }

    }
}