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
                       "ModalScript", PageUtility.LoadEditScript(false), false);

            BindRptCC();
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
                var query = db.CollabCtr;

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

                    collabCtrProjSxn.Visible = true;

                    var dropDownSource = new Dictionary<int, string>();

                    /// Populates dropdown of projects associated with the collaboration center.
                    using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                    {
                       
                        dropDownSource = db.Project2
                                        .Join(db.ClientAgmt, p => p.Id, ca => ca.Project2Id, (p, ca) => new { p, ca })
                                        .Where(j => j.ca.CollabCtrId == id)
                                        .OrderByDescending(y => y.p.Id)
                                        .Select(x => new { x.p.Id, FullName = (x.p.Id + " " + x.p.Title).Substring(0, 150) })
                                        .Distinct()
                                        .ToDictionary(d => d.Id, d => d.FullName);


                        PageUtility.BindDropDownList(ddlCollabCtrProjects, dropDownSource, "-- List of Projects for Collaborative Center --");
                    }


                    CollabCtr cc = GetCollabCtrById(id);

                    if (cc != null)
                    {
                        SetCollabCtr(cc);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                   "ModalScript", PageUtility.LoadEditScript(true), false);
                    }
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
    }
}