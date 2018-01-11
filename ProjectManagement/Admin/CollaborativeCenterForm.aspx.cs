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
    public partial class CollaborativeCenterForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindRptCC();
            }
        }        

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

        private void BindRptCC()
        {
            DataTable ccTable = GetCollabCtrAll();

            rptCC.DataSource = ccTable;
            rptCC.DataBind();
        }

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

        protected void rptCC_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {
            if (((Button)e.CommandSource).Text.Equals("Edit"))
            {
                lblCCId.Text = ((Button)e.CommandSource).CommandArgument;

                int id = 0;
                int.TryParse(lblCCId.Text, out id);

                if (id > 0)
                {                   
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

        private CollabCtr GetCollabCtrById(int id)
        {
            CollabCtr ctr = null;

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                ctr = db.CollabCtr.FirstOrDefault(t => t.Id == id);
            }

            return ctr;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearEditForm();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(true), false);
        }

        private void ClearEditForm()
        {
            CollabCtr newCtr = new CollabCtr();
            newCtr.StartDate = default(DateTime);

            SetCollabCtr(newCtr);
        }

        protected void btnAddAgreement_Click(object sender, EventArgs e)
        {
            int ccId = 0;
            Int32.TryParse(lblCCId.Text, out ccId);

            if (ccId > 0 && txtCCAbbrv.Value != null)
            {
                Response.Redirect(String.Format("ClientAgreementForm?Id={0}&Name={1}",ccId, txtCCAbbrv.Value));
            }
        }

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