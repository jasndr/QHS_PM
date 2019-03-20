using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Admin
{
    /// <summary>
    /// @File: Invoice.aspx.cs
    /// @Front End : Invoice.aspx
    /// @Author: Yang Rui
    /// @Summary: Form to keep track of invoice information that has been sent to clients for payments.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///                                 
    ///  2018SEP11 - Jason Delos Reyes  -  Added documentation for easier readibility and maintainability.
    ///                                 -  Edited bindInvoiceId() to add a leading "0" before writing the name
    ///                                    of the new agreement Id to conform to new naming convention.
    ///                                 -  Added documentation to the Javascript code on the front-end
    ///                                    code for code readability for some of the functions that play
    ///                                    a role in display features of the page (e.g., auto-creation of
    ///                                    agreement number for new Client Agreement forms).
    ///  2018SEP21 - Jason Delos Reyes  -  Configured excel file to be able to add a header file with
    ///                                    the invoice id and dates of invoice, which has also been added
    ///                                    to the file name for reference.
    ///  2018OCT03 - Jason Delos Reyes  -  Configured the invoice form so that the window does not 
    ///                                    close after the user clicks the "update" button by writing
    ///                                    an "modal open" script after postback.
    ///  2018DEC12 - Jason Delos Reyes  -  Added "Excess Consultation Hours" for overage.  However, the feature
    ///                                    is still not being sorted correctly.
    ///  2018DEC13 - Jason Delos Reyes  -  Edited "Excess Consultation Hours" (via means of P_INVOICE_HOURS2 stored procedure)
    ///                                    to prevent duplicating hours with multiple entries.  However, still need to 
    ///                                    account for multiple users AND removing of 0 billable hours.
    ///  2018DEC14 - Jason Delos Reyes  -  Added accounting for multiple users and provided removing 0 billable option
    ///                                    upon the creation of a new invoice (since creating an invoice makes 
    ///                                    form semi-permanent).
    ///  2019JAN17 - Jason Delos Reyes  -  Added radio button partioning between active and inactive collaborative centers as well as
    ///                                    making the collaborative center dropdown a searchable dropdown to be able to quickly 
    ///                                    narrow down to the specific center.
    ///  2019MAR12 - Jason Delos Reyes  -  Made changes to the "Update" button so that the page would reload another 
    ///                                    instance of the form instead of simply saving the form.  This change
    ///                                    would help in cases when the form regenerates a new ID upon saving a new
    ///                                    Invoice form.
    /// </summary>
    public partial class InvoiceForm : System.Web.UI.Page
    {
        /// <summary>
        /// Loads page with information from database according to information available.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();

                int id = 0;
                string invoiceId = Request.QueryString["InvoiceId"];
                if (invoiceId != null)
                {

                    Invoice1 invoice = GetInvoiceByInvoiceId(invoiceId);

                        if (invoice != null)
                            id = invoice.Id;
                }

                if (id > 0)
                    OpenInvoice(id, true);

            }
            //else
            //{
            //    //Reopens the modal after page postback.
            //    ClientScript.RegisterStartupScript(GetType(), "ModalScript", "$('#editModal').modal('show');", true);
            //}
        }        

        //protected void btnAddInvoiceItem_Click(object sender, EventArgs e)
        //{
        //    BindgvInvoiceItem(-1);
        //}

        //protected void gvInvoiceItem_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DropDownList ddl = e.Row.FindControl("ddlBiostat") as DropDownList;

        //        string id = (e.Row.FindControl("lblBiostatId") as Label).Text;

        //        if (ddl != null)
        //        {
        //            //ddl.Items.AddRange(ddlBiostat.Items.OfType<ListItem>().ToArray());
        //            foreach (ListItem li in ddlBiostatHdn.Items)
        //            {
        //                ListItem newItem = new ListItem();
        //                newItem.Value = li.Value;
        //                newItem.Text = li.Text;
        //                newItem.Selected = false;

        //                if (li.Value == id)
        //                {
        //                    newItem.Selected = true;
        //                }

        //                ddl.Items.Add(newItem);
        //            }

        //        }

        //        ddl = e.Row.FindControl("ddlAgreement") as DropDownList;

        //        id = (e.Row.FindControl("lblAgreementId") as Label).Text;

        //        if (ddl != null)
        //        {
        //            //ddl.Items.AddRange(ddlBiostat.Items.OfType<ListItem>().ToArray());
        //            foreach (ListItem li in ddlAgreementHdn.Items)
        //            {
        //                ListItem newItem = new ListItem();
        //                newItem.Value = li.Value;
        //                newItem.Text = li.Text;
        //                newItem.Selected = false;

        //                if (li.Value == id)
        //                {
        //                    newItem.Selected = true;
        //                }

        //                ddl.Items.Add(newItem);
        //            }
        //        }

        //    }
        //}

        /// <summary>
        /// Updates the invoice entry when "update" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = 0;
            string ccAbbrv = txtCCAbbrv.Value,
                   ccId = txtCCId.Value,
                   invoiceId = txtInvoiceId.Value,
                   strId = txtId.Value;

            //char[] delimiter = { '-', '_' };

            //if (invoiceId.Split(delimiter).Length > 1)
            //{
            //    Int32.TryParse(invoiceId.Split(delimiter)[1], out id);
            //}
            Int32.TryParse(strId, out id);

            string fileName = "";
            byte[] fileData = new byte[0];
            // Check to see if file was uploaded
            if (fileUpload.PostedFile != null)
            {
                // Get a reference to PostedFile object
                HttpPostedFile uploadFile = fileUpload.PostedFile;

                // Get size of uploaded file
                int nFileLen = uploadFile.ContentLength;

                // make sure the size of the file is > 0
                if (nFileLen > 0)
                {
                    // Allocate a buffer for reading of the file
                    fileData = new byte[nFileLen];

                    // Read uploaded file from the Stream
                    uploadFile.InputStream.Read(fileData, 0, nFileLen);

                    // Create a name for the file to store
                    fileName = System.IO.Path.GetFileName(uploadFile.FileName);

                    // Write data into a file
                    //WriteToFile(Server.MapPath(strFilename), ref myData);

                    //// Store it in database
                    //int nFileID = WriteToDB(strFilename, myFile.ContentType, ref myData);

                }

            }

            Invoice1 invoice = GetInvoice(id, fileName, fileData);

            int savedInvoiceId = SaveInvoice(invoice);
            if (savedInvoiceId > 0)
            {
                SaveInvoiceItem(invoice);
            }

            string appPath = String.Empty;
            appPath = HttpContext.Current.Request.ApplicationPath.Length > 1 ? HttpContext.Current.Request.ApplicationPath
                                                                                +"/Admin/"
                                                                             : String.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "successMsg",
                "alert('Record Saved Successfully'); window.location='" + appPath
                                                                       + "InvoiceForm?InvoiceId="
                                                                       + invoiceId + "';", true);

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
            //           "ModalScript", PageUtility.LoadEditScript("update"/*false*/), false);

            //BindRptInvoice(savedInvoiceId);

            //foreach (GridViewRow row in gvInvoiceItem.Rows)
            //{
            //    if (row.RowType == DataControlRowType.DataRow)
            //    {
            //        TextBox txtAgreementDate = row.FindControl("txtAgreementDate") as TextBox;
            //    }
            //}
        }

        /// <summary>
        /// Saves the INVOICE ITEM into the database with values from the web form and associates
        /// it with the current invoice instance.
        /// </summary>
        /// <param name="invoice">Instance of an invoice.</param>
        private void SaveInvoiceItem(Invoice1 invoice)
        {
            //List<InvoiceItem> lstInvoiceItem = GetInvoiceItem(invoiceId);

            //if (lstInvoiceItem.Count > 0)
            //{
            //    using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            //    {
            //        var invoiceItems = db.InvoiceItem.Where(b => b.InvoiceId == invoiceId).ToList();

            //        foreach (var invoiceItem in lstInvoiceItem)
            //        {
            //            var itemInDB = db.InvoiceItem.FirstOrDefault(i => i.Id == invoiceItem.Id);

            //            if (itemInDB != null)
            //            {
            //                db.Entry(itemInDB).CurrentValues.SetValues(invoiceItem);
            //            }
            //            else
            //            {
            //                db.InvoiceItem.Add(invoiceItem);
            //            }
            //        }

            //        foreach (var item in invoiceItems)
            //        {
            //            if (lstInvoiceItem.FindIndex(i => i.Id == item.Id) < 0)
            //            {
            //                db.InvoiceItem.Remove(item);
            //            }
            //        }

            //        db.SaveChanges();
            //    }
            //}
            List<InvoiceItem2> lstInvoiceItem = GetInvoiceItem2(invoice);

            if (lstInvoiceItem.Count > 0)
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    var invoiceItems = db.InvoiceItem2.Where(b => b.Invoice1.Id == invoice.Id).ToList();
                    foreach (var invoiceItem in lstInvoiceItem)
                    {
                        var itemInDB = db.InvoiceItem2.FirstOrDefault(i => i.Id == invoiceItem.Id);

                        if (itemInDB != null)
                        {
                            db.Entry(itemInDB).CurrentValues.SetValues(invoiceItem);
                        }
                        else
                        {
                            db.InvoiceItem2.Add(invoiceItem);
                        }
                    }

                    foreach (var item in invoiceItems)
                    {
                        if (lstInvoiceItem.FindIndex(i => i.Id == item.Id) < 0)
                        {
                            //db.InvoiceItem2.Remove(item);
                            item.IsDeleted = true;
                        }
                    }

                    db.SaveChanges();
                }
            }

        }

        /// <summary>
        /// Given the invoice, generates a list of projects associated with the invoice period
        /// and prints out the estimated and invoice hours for that project and phase instance.
        /// </summary>
        /// <param name="invoice">Instance of the invoice.</param>
        /// <returns>List of invoice items for the invoice report.</returns>
        private List<InvoiceItem2> GetInvoiceItem2(Invoice1 invoice)
        {
            var lstInvoiceItem = new List<InvoiceItem2>();

            foreach (RepeaterItem item in rptNewInvoice.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var lblId = item.FindControl("lblId") as Label;
                    var agmt = item.FindControl("lblAgreement") as Label;
                    var staffType = item.FindControl("lblType") as Label;
                    var tobeBilled = ((System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtToBeBilled"));

                    int id = 0;
                    Int32.TryParse(lblId.Text, out id);
                    decimal invoiceHr = 0.0m;
                    Decimal.TryParse(tobeBilled.Value, out invoiceHr);

                    using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                    {
                        var agreement = db.ClientAgmt.FirstOrDefault(a => a.AgmtId == agmt.Text);

                        if (agreement != null)
                        {
                            InvoiceItem2 i = new InvoiceItem2()
                            {
                                Id = id,
                                Invoice1Id = invoice.Id,
                                ClientAgmtId = agreement.Id,
                                StaffType = staffType.Text,
                                InvoiceHr = invoiceHr,
                                Creator = Page.User.Identity.Name,
                                CreationDate = DateTime.Now,
                                IsDeleted = false
                            };

                            lstInvoiceItem.Add(i);
                        }                        
                    }
                }
            }

            return lstInvoiceItem;
        }

        //private List<InvoiceItem> GetInvoiceItem(int invoiceId)
        //{
        //    List<InvoiceItem> lstInvoiceItem = new List<InvoiceItem>();

        //    foreach (GridViewRow row in gvInvoiceItem.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            Label lblId = row.FindControl("lblId") as Label;
        //            DropDownList ddlAgreement = row.FindControl("ddlAgreement") as DropDownList;
        //            DropDownList ddlBiostat = row.FindControl("ddlBiostat") as DropDownList;
        //            TextBox txtDesc = row.FindControl("txtDesc") as TextBox;
        //            TextBox txtInvoiceRate = row.FindControl("txtRate") as TextBox;
        //            TextBox txtInvoiceHr = row.FindControl("txtCompletedHours") as TextBox;

        //            int agreementId = -1, biostatId = -1;
        //            int output = 0;
        //            decimal dOutput = 0.0m;

        //            if (ddlAgreement != null && ddlBiostat != null && txtDesc != null && txtInvoiceRate != null && txtInvoiceHr != null)
        //            {
        //                Int32.TryParse(ddlAgreement.SelectedValue, out agreementId);
        //                Int32.TryParse(ddlBiostat.SelectedValue, out biostatId);

        //                if (agreementId > 0)
        //                {
        //                    InvoiceItem invoiceItem = new InvoiceItem()
        //                    {
        //                        Id = int.TryParse(lblId.Text, out output) ? output : -1,
        //                        InvoiceId = invoiceId,
        //                        ClientAgmtId = agreementId,                                
        //                        BiostatId = biostatId > 0 ? biostatId : -1,
        //                        Desc = txtDesc.Text,
        //                        InvoiceRate = decimal.TryParse(txtInvoiceRate.Text, out dOutput) ? dOutput : 0.0m,
        //                        InvoiceHr = decimal.TryParse(txtInvoiceHr.Text, out dOutput) ? dOutput : 0.0m,
        //                        Creator = Page.User.Identity.Name,
        //                        CreateDate = DateTime.Now
        //                    };

        //                    lstInvoiceItem.Add(invoiceItem);
        //                }
        //            }
        //        }
        //    }

        //    return lstInvoiceItem;
        //}

        /// <summary>
        /// Saves the INVOICE entry based on the Invoice Form web fields
        /// into the database.  The current values are overridden with the
        /// fields typed from the webform (in the given invoice instance).
        /// Otherwise, if there is no previous invoice associated with this
        /// form, a new invoice entry is created in the database.
        /// </summary>
        /// <param name="invoice">Instance of invoice.</param>
        /// <returns>Invoice ID.</returns>
        private int SaveInvoice(Invoice1 invoice)
        {
            int invoiceId = -1;

            try
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    if (invoice.Id > 0) //update
                    {
                        var prevInvoice = db.Invoice1Set.FirstOrDefault(c => c.Id == invoice.Id);

                        if (prevInvoice != null)
                        {
                            invoice.CollabCtrId = prevInvoice.CollabCtrId;

                            if (prevInvoice.FileName != null && prevInvoice.FileUpload != null)
                            {
                                if (invoice.FileUpload.Count<byte>() == 0 && prevInvoice.FileUpload.Count<byte>() > 0)
                                {
                                    invoice.FileName = prevInvoice.FileName;
                                    invoice.FileUpload = prevInvoice.FileUpload;
                                }
                            }

                            db.Entry(prevInvoice).CurrentValues.SetValues(invoice);
                        }
                    }
                    else //new
                    {
                        db.Invoice1Set.Add(invoice);
                    }

                    db.SaveChanges();

                    invoiceId = invoice.Id;
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

            return invoiceId;
        }

        /// <summary>
        /// Creates an intance of an invoice with the given
        /// values from the web form.
        /// </summary>
        /// <param name="id">Invoice ID.</param>
        /// <param name="fileName">Name of uploaded file.</param>
        /// <param name="fileData">Uploaded file information stored in byte[] format.</param>
        /// <returns>Instance of the invoice.</returns>
        private Invoice1 GetInvoice(int id, string fileName, byte[] fileData)
        {
            int ccid = 0;
            DateTime dt;
            Decimal dc = 0.0m;

            Invoice1 invoice = new Invoice1()
            {
                Id = id,
                CollabCtrId = Int32.TryParse(txtCCId.Value, out ccid) ? ccid : -1,
                InvoiceDate = DateTime.TryParse(txtInvoiceDate.Text, out dt) ? dt : DateTime.Now,
                StartDate = DateTime.TryParse(txtStartDate.Text, out dt) ? dt : DateTime.Now,
                EndDate = DateTime.TryParse(txtEndDate.Text, out dt) ? dt : DateTime.Now,
                //SubTotal = Decimal.TryParse(txtSubTotal.Value, out dc) ? dc : (Decimal?)null,
                //Discount = Decimal.TryParse(txtDiscount.Value, out dc) ? dc : (Decimal?)null,
                PaymentRcvdDate = DateTime.TryParse(txtRcvDate.Text, out dt) ? dt : (DateTime?)null,
                PaymentRcvdAmount = Decimal.TryParse(txtPaymentRcvd.Value, out dc) ? dc : (Decimal?)null,
                FileName = fileName,
                FileUpload = fileData,
                Comments = txtComments.Value,
                Creator = Page.User.Identity.Name,
                CreateDate = DateTime.Now,
                InvoiceId = txtInvoiceId.Value
            };       

            return invoice;
        }

        //protected void gvInvoiceItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    GridViewRow row = gvInvoiceItem.Rows[e.RowIndex];
            
        //    //BindgvInvoiceItem(e.RowIndex);
        //}

        /// <summary>
        /// Prepares invoice report if user selects a specific invoice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptInvoice_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {
            if (((Button)e.CommandSource).Text.Equals("Edit"))
            {
                chkRemoveZeros.Visible = false; // Hide "remove zeros" as it doesn't work for saved invoices.

                int id = 0;
                int.TryParse(((Button)e.CommandSource).CommandArgument, out id);

                OpenInvoice(id, false);
                
            }
        }

        /// <summary>
        /// Obtains instance of invoice and prepares invoice form.
        /// </summary>
        /// <param name="id">Given invoice id.</param>
        /// <param name="fromPageLoad">Boolean value to distingusih whether or not the function was
        ///                            called from page load as it needs to call a client script
        ///                            instead of being able to parse javascript from codebehind.</param>
        private void OpenInvoice(int id, bool fromPageLoad)
        {
            if (id > 0)
            {
                Invoice1 invoice = GetInvoiceById(id);

                if (invoice != null)
                {
                    SetInvoice(invoice);

                    if (fromPageLoad == true)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup()", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                   "ModalScript", PageUtility.LoadEditScript(true), false);
                    }
                }
            }
        }

        /// <summary>
        /// Prepares invoice form based on the information available form
        /// the database.
        /// </summary>
        /// <param name="invoice">Instance of the invoice.</param>
        private void SetInvoice(Invoice1 invoice)
        {
            txtId.Value = invoice.Id.ToString();
            txtCCAbbrv.Value = invoice.CollabCtr.NameAbbrv;
            txtCCId.Value = invoice.CollabCtrId.ToString();
            txtInvoiceDate.Text = invoice.InvoiceDate != null ? Convert.ToDateTime(invoice.InvoiceDate).ToShortDateString() : "";
            txtInvoiceId.Value = invoice.InvoiceId;     //invoice.Id > 0 ? txtCCAbbrv.Value + '-' + invoice.Id.ToString() : "";
            txtStartDate.Text = invoice.StartDate != null ? Convert.ToDateTime(invoice.StartDate).ToShortDateString() : "";
            txtEndDate.Text = invoice.EndDate != null ? Convert.ToDateTime(invoice.EndDate).ToShortDateString() : "";            

            //txtSubTotal.Value = invoice.SubTotal != null ? invoice.SubTotal.ToString() : "";
            //txtDiscount.Value = invoice.Discount != null ? invoice.Discount.ToString() : "";

            //txtGrandTotal.Value = invoice.SubTotal != null && invoice.Discount != null ? (invoice.SubTotal - invoice.Discount).ToString() : "";
            txtRcvDate.Text = invoice.PaymentRcvdDate != null ? Convert.ToDateTime(invoice.PaymentRcvdDate).ToShortDateString() : "";
            txtPaymentRcvd.Value = invoice.PaymentRcvdAmount != null ? invoice.PaymentRcvdAmount.ToString() : "";
            txtComments.Value = invoice.Comments != null ? invoice.Comments : "";

            lnkFile.Text = invoice.FileName;

            //BindgvInvoiceItem(invoice.InvoiceItem);
            BindRptNewInvoice(invoice.Id);
        }        

        //private void BindgvInvoiceItem(ICollection<InvoiceItem> items)
        //{          
        //    DataTable dt = CreateInvoiceItemTable(items, true);
        //    gvInvoiceItem.DataSource = dt;
        //    gvInvoiceItem.DataBind();
        //}

        /// <summary>
        /// Returns the instance of the invoice from the database 
        /// based on the invoice Id specified.
        /// </summary>
        /// <param name="id">Invoice Id specified.</param>
        /// <returns>Instance of invoice from database.</returns>
        private Invoice1 GetInvoiceById(int id)
        {
            Invoice1 invoice1 = null;
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                invoice1 = db.Invoice1Set.FirstOrDefault(i => i.Id == id);
                invoice1.CollabCtr.NameAbbrv = invoice1.CollabCtr.NameAbbrv;
                invoice1.InvoiceItem2 = invoice1.InvoiceItem2;
                foreach (InvoiceItem2 i in invoice1.InvoiceItem2)
                {
                    i.ClientAgmt = i.ClientAgmt;
                }

                //ddlAgreementHdn.Items.Clear();
                //IDictionary<int, string> dropDownSource = new Dictionary<int, string>();
                //dropDownSource = db.ClientAgmt
                //                .Where(a => a.CollabCtr.Id == invoice1.CollabCtrId)
                //                .ToDictionary(c => c.Id, c => c.AgmtId);

                //PageUtility.BindDropDownList(ddlAgreementHdn, dropDownSource, "--- Select ---");

            }

            return invoice1;
        } 

        /// <summary>
        /// Pulls instance of Invoice from database based on given
        /// Invoice ID.
        /// </summary>
        /// <param name="invoiceId">Given invoice ID. (E.g., I-OBGYN-02).</param>
        /// <returns>Instance of Invoice.</returns>
        private Invoice1 GetInvoiceByInvoiceId(string invoiceId)
        {
            Invoice1 invoice1 = null;
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                invoice1 = db.Invoice1Set.FirstOrDefault(i => i.InvoiceId == invoiceId);
                invoice1.CollabCtr.NameAbbrv = invoice1.CollabCtr.NameAbbrv;
                invoice1.InvoiceItem2 = invoice1.InvoiceItem2;
                foreach (InvoiceItem2 i in invoice1.InvoiceItem2)
                {
                    i.ClientAgmt = i.ClientAgmt;
                }
            }
            return invoice1;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //ClearEditForm();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(true), false);
        }
        
        /// <summary>
        /// Loads page based on information available (Ex: If there is a collab center id available,
        /// the collabortive center is stored in the front end web page).  Also loads the list
        /// of collaborative centers in the main list page alphabetically by collab center abbreviation.
        /// </summary>
        private void BindControl()
        {
            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();
            int ccid = 0;
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                //dropDownSource = db.BioStats
                //                //.Where(b => b.EndDate > DateTime.Now)
                //                .OrderByDescending(b => b.Id)
                //                .ToDictionary(c => c.Id, c => c.Name);

                //PageUtility.BindDropDownList(ddlBiostatHdn, dropDownSource, "- Select -");

                Int32.TryParse(Request.QueryString["CCId"], out ccid);

                txtCCId.Value = ccid.ToString();

                //dropDownSource = db.ClientAgmt
                //                .Where(a => a.CollabCtr.Id == ccid)
                //                .ToDictionary(c => c.Id, c => c.CollabCtr.NameAbbrv + "-" + c.Id);

                //PageUtility.BindDropDownList(ddlAgreementHdn, dropDownSource, "--- Select ---");

                dropDownSource = db.CollabCtr
                                .OrderBy(c => c.NameAbbrv)
                                .Where(c => c.Id > 0)
                                .Select(x => new { x.Id, FullName = (x.NameAbbrv + " | " + x.Name) })
                                .ToDictionary(c => c.Id, c => c.FullName);

                PageUtility.BindDropDownList(ddlCollab, dropDownSource, string.Empty);
            }

            //BindgvInvoiceItem(-1);

            BindRptInvoice();

            //if (ccid > 0)
            //    BindRptNewInvoice(ccid);
        }

        /// <summary>
        /// Fetches a new invoice report with list of projects as well as estimated hours based 
        /// on the start and end dates specified.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFetchInvoice_Click(object sender, EventArgs e)
        {
            //var ccAbbrv = txtCCAbbrv.Value;
            int ccId = 0;
            Int32.TryParse(txtCCId.Value, out ccId);
            DateTime startDate, endDate;

            int invoiceId = 0;
            Int32.TryParse(txtId.Value, out invoiceId);

            //if (invoiceId > 0)
            //    BindRptNewInvoice(invoiceId);
            //else 
                if (DateTime.TryParse(txtStartDate.Text, out startDate) && DateTime.TryParse(txtEndDate.Text, out endDate))
                    BindRptNewInvoice(ccId, invoiceId, startDate, endDate);
                      
        }

        /// <summary>
        /// Produces new invoice report with just the invoice id specifed.  
        /// </summary>
        /// <param name="invoiceId">Invoice Id specified.</param>
        private void BindRptNewInvoice(int invoiceId)
        {
            if (invoiceId > 0)
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    var invoice = db.Invoice1Set.FirstOrDefault(n => n.Id == invoiceId);

                    if (invoice != null)
                    {
                        //var query = db.P_INVOICE_HOURS(invoice.CollabCtrId, invoice.Id, invoice.StartDate, invoice.EndDate, false);
                        //var query = db.P_INVOICE_HOURS2(invoice.CollabCtrId, invoice.Id, invoice.StartDate, invoice.EndDate, false);

                        bool removeZeros = chkRemoveZeros.Checked ? true : false;
                        var query = db.P_INVOICE_HOURS2a(invoice.CollabCtrId, invoice.Id, invoice.StartDate, invoice.EndDate, false, removeZeros);

                        db.SaveChanges();

                        rptNewInvoice.DataSource = query;
                        rptNewInvoice.DataBind();
                    }
                }
            }
        }

        /// <summary>
        /// Produces a list of projects and estimated and invoice hours for the time period specified
        /// to be displayed onto a web-based grid form.
        /// 
        /// NOTE: You are only permitted to press this once.  If you have made a mistake (e.g., entered
        /// the wrong time period), please exit out of the page and re-open the invoice form again.
        /// *Do not* save the form without being certain that you are content with this invoice form.
        /// You will need to create another invoice if you have previously saved an invoice form with
        /// a mistake.
        /// </summary>
        /// <param name="ccId">Referred/current collaborative center.</param>
        /// <param name="invoiceId">Invoice ID of current form.</param>
        /// <param name="startdate">Invoice Start Date specified.</param>
        /// <param name="enddate">Invoice End Date specified.</param>
        private void BindRptNewInvoice(int ccId, int invoiceId, DateTime startdate, DateTime enddate)
        {
            if (ccId > 0)
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    //var query = db.P_INVOICE_HOURS2(ccId, invoiceId, startdate, enddate, true);
                    bool removeZeros = chkRemoveZeros.Checked ? true : false;
                    var query = db.P_INVOICE_HOURS2a(ccId, invoiceId, startdate, enddate, true, removeZeros);
                    db.SaveChanges();

                    rptNewInvoice.DataSource = query;
                    rptNewInvoice.DataBind();
                }
            }
        }

        protected void rptNewInvoice_ItemCreated(object source, RepeaterItemEventArgs e)
        {
            ScriptManager scriptMan = ScriptManager.GetCurrent(this);
            LinkButton btn = e.Item.FindControl("lnkDelete") as LinkButton;
            if (btn != null)
            {
                //btn.Click += lnkDelete_Click;
                scriptMan.RegisterAsyncPostBackControl(btn);
            }

        }

        //private void lnkDelete_Click(object sender, EventArgs e)
        //{
            
        //}

        /// <summary>
        /// Provides an option to delete an invoice instance if payment hasn't been scheduled yet.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptNewInvoice_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {               

                int id = 0;
                Int32.TryParse(e.CommandArgument.ToString(), out id);

                int index = e.Item.ItemIndex;

                BindRptNewInvoiceByRow(index);              
            }
        }

        /// <summary>
        /// Rebinds invoice list by excluding the invoice specified,
        /// giving the illusion of "deleting" an invoice.
        /// </summary>
        /// <param name="index">Index of invoice entry to be deleted.</param>
        private void BindRptNewInvoiceByRow(int index)
        {
            DataTable dt = CreateInvoiceTable(index, false);
            
            rptNewInvoice.DataSource = dt;
            rptNewInvoice.DataBind();
        }        

        /// <summary>
        /// Creates the Excel sheet of the invoice form based on the report 
        /// that has been created in the excel sheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = CreateInvoiceTable(-1, true);

            string fileValue = String.Format("Invoice-{0}.xlsx", txtInvoiceId.Value);
            
            string fileName = String.Format("Invoice-{0}-From-{1}-To-{2}.xlsx", txtInvoiceId.Value, txtStartDate.Text, txtEndDate.Text);
            string titleName = String.Format("Invoice {0} from {1} to {2}", txtInvoiceId.Value, txtStartDate.Text, txtEndDate.Text);

            FileExport fileExport = new FileExport(this.Response);
            //fileExport.ExcelExport(dt, fileValue);
            fileExport.ExcelExport2(dt, fileName, titleName);
        }

        /// <summary>
        /// Binds invoice report for download and/or deletion, depending whether or not
        /// index is specified.
        /// </summary>
        /// <param name="index">Index of selected invoice if specified to delete.</param>
        /// <param name="fileExport">Boolean value to determine whether or not
        ///                          the data table being requested will be exported
        ///                          as an excel file.</param>
        /// <returns>Data Tabke of invoice report.</returns>
        private DataTable CreateInvoiceTable(int index, bool fileExport)
        {
            DataTable dt = new DataTable("tblRptInvoice");

            decimal phdTotal = 0.0m,
                    msTotal = 0.0m,
                    subTotal = 0.0m,
                    discount = 0.0m/*,
                    grandTotal = 0.0m*/;

            if (!fileExport)
            {
                dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            }
            dt.Columns.Add("ProjectId", System.Type.GetType("System.Int32"));
            dt.Columns.Add("PI", System.Type.GetType("System.String"));
            dt.Columns.Add("Title", System.Type.GetType("System.String"));
            dt.Columns.Add("Agreement", System.Type.GetType("System.String"));
            dt.Columns.Add("Type", System.Type.GetType("System.String"));
            dt.Columns.Add("Approved", System.Type.GetType("System.String"));
            dt.Columns.Add("Invoiced", System.Type.GetType("System.String"));
            dt.Columns.Add("Remain", System.Type.GetType("System.String"));
            dt.Columns.Add("TimeTracking", System.Type.GetType("System.String"));
            dt.Columns.Add("ToBeBilled", System.Type.GetType("System.String"));
            dt.Columns.Add("Rate", System.Type.GetType("System.String"));
            dt.Columns.Add("SubTotal", System.Type.GetType("System.String"));
            dt.Columns.Add("Discount", System.Type.GetType("System.String"));
            dt.Columns.Add("RemainAfter", System.Type.GetType("System.String"));

            foreach (RepeaterItem item in rptNewInvoice.Items)
            {
                if (item.ItemIndex != index)
                {
                    DataRow dr = dt.NewRow();

                    int id = 0
                        ,projectid = 0;
                    Int32.TryParse(((Label)item.FindControl("lblId")).Text, out id);
                    Int32.TryParse(((Label)item.FindControl("lblProjectId")).Text, out projectid);

                    if (!fileExport)
                    {
                        dr["Id"] = id;
                    }
                    dr["ProjectId"] = projectid;
                    dr["PI"] = ((Label)item.FindControl("lblPI")).Text;
                    dr["Title"] = ((Label)item.FindControl("lblProjectTitle")).Text;
                    dr["Agreement"] = ((Label)item.FindControl("lblAgreement")).Text;
                    dr["Type"] = ((Label)item.FindControl("lblType")).Text;
                    dr["Approved"] = ((Label)item.FindControl("lblApprovedHr")).Text;
                    dr["Invoiced"] = ((Label)item.FindControl("lblInvoicedHr")).Text;
                    dr["Remain"] = ((Label)item.FindControl("lblRemainingHr")).Text;
                    dr["TimeTracking"] = ((Label)item.FindControl("lblTimeTracking")).Text;
                    dr["ToBeBilled"] = ((System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtToBeBilled")).Value;
                    dr["Rate"] = ((TextBox)item.FindControl("txtRate")).Text;
                    //dr["SubTotal"] = ((System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtSubTotal2")).Value;
                    dr["Discount"] = ((System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("lblDiscountHr")).Value;
                    dr["RemainAfter"] = ((System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("lblRemainAfterBill")).Value;

                    dt.Rows.Add(dr);

                    decimal invoiced = 0.0m,
                            rate = 0.0m,
                            timeTracking = 0.0m;

                    decimal.TryParse(dr["ToBeBilled"].ToString(), out invoiced);
                    decimal.TryParse(dr["Rate"].ToString(), out rate);
                    decimal.TryParse(dr["TimeTracking"].ToString(), out timeTracking);

                    if (dr["Type"].ToString() == "phd")
                    {
                        phdTotal += timeTracking;    //invoiced;

                    }
                    else
                        msTotal += timeTracking;    //invoiced;

                    dr["SubTotal"] = String.Format("{0:C}", invoiced * rate);
                    dr["Discount"] = timeTracking > invoiced ? (timeTracking - invoiced).ToString() : "0.0";
                    subTotal += invoiced * rate;
                    discount += Convert.ToDecimal(dr["Discount"].ToString()) * rate;
                }
            }

            // add footer
            if (fileExport)
            {
                DataRow drEmpty = dt.NewRow();
                dt.Rows.Add(drEmpty);

                DataRow dr1 = dt.NewRow();

                dr1[5] = "Phd-Total";
                dr1[6] = phdTotal.ToString();
                dr1[10] = "Sub-Total";
                dr1[11] = String.Format("{0:C}", subTotal + discount);

                dt.Rows.Add(dr1);

                DataRow dr2 = dt.NewRow();

                dr2[5] = "MS-Total";
                dr2[6] = msTotal.ToString();
                dr2[10] = "Discount";
                dr2[11] = String.Format("{0:C}", discount);

                dt.Rows.Add(dr2);

                DataRow dr3 = dt.NewRow();
                                
                dr3[10] = "Grand-Total";
                dr3[11] = String.Format("{0:C}", subTotal);

                dt.Rows.Add(dr3);

            }

            return dt;
        }

        /// <summary>
        /// Partitions between active, inactive, or all collaborative centers for display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void collabCenterType_Changed(Object sender, EventArgs e)
        {
            BindRptInvoice();
        }

        /// <summary>
        /// Changes the list of invoices to the one restricted to the collaborative center
        /// specified in the drop down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCollab_Changed(Object sender, EventArgs e)
        {
            BindRptInvoice();
        }


        /// <summary>
        /// Binds the list of invoices based on the collaborative center selected.
        /// </summary>
        private void BindRptInvoice()
        {
            int collabId = 0;
            Int32.TryParse(ddlCollab.SelectedValue, out collabId);

            DataTable dt = new DataTable("tblInvoice");

            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));            
            dt.Columns.Add("StartDate", System.Type.GetType("System.String"));
            dt.Columns.Add("EndDate", System.Type.GetType("System.String"));
            dt.Columns.Add("Subtotal", System.Type.GetType("System.String"));
            dt.Columns.Add("Discount", System.Type.GetType("System.String"));
            dt.Columns.Add("Payment", System.Type.GetType("System.String"));
            dt.Columns.Add("RcvdDate", System.Type.GetType("System.String"));
            dt.Columns.Add("Creator", System.Type.GetType("System.String"));
            dt.Columns.Add("InvoiceId", System.Type.GetType("System.String"));
            dt.Columns.Add("CollabCtr", System.Type.GetType("System.String"));
            dt.Columns.Add("InvoiceDate", System.Type.GetType("System.String"));

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var query = db.Invoice1Set
                            .Join(db.CollabCtr, i => i.CollabCtrId, c => c.Id,
                                (i, c) => new { i, c.NameAbbrv, i.CollabCtrId }
                            );

                if (collabId > 0)
                {
                    query = query.Where(c => c.CollabCtrId == collabId);
                }

                switch (collabCenterType.SelectedValue)
                {
                    case "active":
                        //query = query.Where(x => x.i.EndDate == null || (x.i.EndDate != null && x.i.EndDate >= DateTime.Today));
                        query = query.Where(y => y.i.CollabCtr.EndDate == null || (y.i.CollabCtr.EndDate != null && y.i.CollabCtr.EndDate >= DateTime.Today));
                        break;
                    case "inactive":
                        //query = query.Where(x => x.i.EndDate != null && x.i.EndDate < DateTime.Today);
                        query = query.Where(y => y.i.CollabCtr.EndDate != null && y.i.CollabCtr.EndDate < DateTime.Today);
                        break;
                    default:
                        break;
                }

                foreach (var ic in query.OrderBy(c => c.i.InvoiceId).ToList())
                {
                    DataRow dr = dt.NewRow();

                    dr[0] = ic.i.Id;                    
                    dr[1] = Convert.ToDateTime(ic.i.StartDate).ToShortDateString();
                    dr[2] = Convert.ToDateTime(ic.i.EndDate).ToShortDateString();
                    dr[3] = ic.i.SubTotal != null ? ic.i.SubTotal.ToString() : "";
                    dr[4] = ic.i.Discount != null ? ic.i.Discount.ToString() : "";
                    dr[5] = ic.i.PaymentRcvdAmount != null ? ic.i.PaymentRcvdAmount.ToString() : "";
                    dr[6] = ic.i.PaymentRcvdDate != null ? Convert.ToDateTime(ic.i.PaymentRcvdDate).ToShortDateString() : "";
                    dr[7] = ic.i.Creator;
                    dr[8] = ic.i.InvoiceId;
                    dr[9] = ic.NameAbbrv;
                    dr[10] = ic.i.InvoiceDate != null ? Convert.ToDateTime(ic.i.InvoiceDate).ToShortDateString() : "";

                    dt.Rows.Add(dr);
                }
            }

            rptInvoice.DataSource = dt;
            rptInvoice.DataBind();
            

        }

        /// <summary>
        /// Given the invoice Id, binds invoice instance
        /// into the given web form.
        /// </summary>
        /// <param name="id"></param>
        private void BindRptInvoice(int id)
        {
           // int invoiceId = id;
            //Int32.TryParse(txtId.Value, out invoiceId);
            if (id > 0)
            {
                //string updatedQueryString = "?InvoiceId=" + invoiceId;

                Invoice1 invoice = GetInvoiceById(id);

                if (invoice != null)
                {
                    SetInvoice(invoice);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                               "ModalScript", PageUtility.LoadEditScript("update"), false);
                }



            }
        }

        /// <summary>
        /// (Currently not being used.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAgreement_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            int agmtId = 0;
            Int32.TryParse(ddl.SelectedValue, out agmtId);

            if (agmtId > 0)
            {
                GridViewRow row = (GridViewRow)ddl.Parent.Parent;
                //int idx = row.RowIndex;

                if (row != null)
                {
                    TextBox txtAgreementDate = row.FindControl("txtAgreementDate") as TextBox;
                    TextBox txtProjectId = row.FindControl("txtProjectId") as TextBox;
                    //TextBox txtRate = row.FindControl("txtRate") as TextBox;
                    //TextBox txtCompletedHours = row.FindControl("txtCompletedHours") as TextBox;
                    //TextBox txtTotalCost = row.FindControl("txtTotalCost") as TextBox;

                    using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                    {
                        var agmt = db.ClientAgmt.FirstOrDefault(t => t.Id == agmtId);

                        if (agmt != null)
                        {
                            txtAgreementDate.Text = agmt.ApprovalDate != null ? Convert.ToDateTime(agmt.ApprovalDate).ToShortDateString() : "";
                            txtProjectId.Text = agmt.Project2Id.ToString();
                            //txtRate.Text = agmt.ApprovedPhdRate != null ? agmt.ApprovedPhdRate.ToString() : "";
                            //txtCompletedHours.Text = agmt.ApprovedPhdHr != null ? agmt.ApprovedPhdHr.ToString() : "";

                            if (agmt.Project2Id > 0)
                            {
                                DropDownList ddlBiostat = row.FindControl("ddlBiostat") as DropDownList;

                                if (ddlBiostat != null)
                                {
                                    var dropDownSource = new Dictionary<int, string>();
                                    ddlBiostat.ClearSelection();

                                    dropDownSource = db.BioStats
                                                    .Join (db.ProjectBioStats, b => b.Id, p => p.BioStats_Id, (b, p) => new { b.Id, b.Name, p.EndDate, p.Projects_Id})
                                                    .Where(b => b.EndDate > DateTime.Now && b.Projects_Id == agmt.Project2Id)
                                                    .ToDictionary(c => c.Id, c => c.Name);
                                    PageUtility.BindDropDownList(ddlBiostat, dropDownSource, "- Select -");
                                }
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// (Currently Not Being Used.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBiostat_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            int biostatId = 0;
            Int32.TryParse(ddl.SelectedValue, out biostatId);

            if (biostatId > 0)
            {
                GridViewRow row = (GridViewRow)ddl.Parent.Parent;

                if (row != null)
                {
                    DropDownList ddlAgreement = row.FindControl("ddlAgreement") as DropDownList;

                    int agmtId = 0;
                    Int32.TryParse(ddlAgreement.SelectedValue, out agmtId);

                    DateTime dtStart, dtEnd = Convert.ToDateTime("2099-01-01");
                    bool validateDate = DateTime.TryParse(txtStartDate.Text, out dtStart) && DateTime.TryParse(txtEndDate.Text, out dtEnd);

                    if (agmtId > 0 && validateDate)
                    {
                        TextBox txtRate = row.FindControl("txtRate") as TextBox;
                        TextBox txtCompletedHours = row.FindControl("txtCompletedHours") as TextBox;

                        using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                        {
                            var agmt = db.ClientAgmt.FirstOrDefault(t => t.Id == agmtId);
                            var biostat = db.BioStats.FirstOrDefault(b => b.Id == biostatId);

                            if (agmt != null)
                            {
                                txtRate.Text = biostat.Type == "phd" ? agmt.ApprovedPhdRate.ToString() : agmt.ApprovedMsRate.ToString();

                                string query = string.Format(@"SELECT SUM(t.Duration) 
                                                            FROM TimeEntries t
                                                            INNER JOIN Date d
	                                                            ON d.DateKey = t.DateId
                                                            INNER JOIN ProjectPhase p
	                                                            ON t.ProjectId = p.ProjectId
		                                                            AND t.PhaseId = p.Id
                                                            WHERE t.ProjectId = {0}
	                                                            and t.BioStatId = {1}
	                                                            and p.Name = '{2}'
	                                                            and d.[Date] BETWEEN '{3}' AND '{4}'"
                                                            ,agmt.Project2Id 
                                                            ,biostatId
                                                            ,agmt.ProjectPhase
                                                            ,dtStart.ToShortDateString()
                                                            ,dtEnd.ToShortDateString()
                                                            );

                                var totalHours = db.Database.SqlQuery<decimal?>(query);

                                if (totalHours.FirstOrDefault() != null)
                                {
                                    txtCompletedHours.Text = totalHours.FirstOrDefault().ToString();
                                }

                            }
                        }
                    }

                }

            }
        }


        //private void BindgvInvoiceItem(int rowIndex)
        //{
        //    DataTable dt = CreateInvoiceItemTable(null, false);
        //    DataRow dr;

        //    foreach (GridViewRow row in gvInvoiceItem.Rows)
        //    {
        //        if (row.RowIndex != rowIndex)
        //        {
        //            dr = dt.NewRow();

        //            Label rowId = row.FindControl("lblId") as Label;
        //            DropDownList ddlAgmt = row.FindControl("ddlAgreement") as DropDownList;
        //            TextBox txtAgreementDate = row.FindControl("txtAgreementDate") as TextBox;
        //            DropDownList ddlBiostat = row.FindControl("ddlBiostat") as DropDownList;
        //            TextBox txtProjectId = row.FindControl("txtProjectId") as TextBox;
        //            TextBox txtDesc = row.FindControl("txtDesc") as TextBox;
        //            TextBox txtRate = row.FindControl("txtRate") as TextBox;
        //            TextBox txtCompletedHours = row.FindControl("txtCompletedHours") as TextBox;
        //            TextBox txtTotalCost = row.FindControl("txtTotalCost") as TextBox;

        //            dr[0] = rowId.Text;
        //            dr[1] = ddlAgmt.SelectedValue;
        //            dr[2] = ddlBiostat.SelectedValue;
        //            dr[3] = txtRate.Text;
        //            dr[4] = txtCompletedHours.Text;
        //            dr[5] = txtDesc.Text;
        //            dr[6] = txtAgreementDate.Text;
        //            dr[7] = txtProjectId.Text;
        //            dr[8] = txtTotalCost.Text;

        //            dt.Rows.Add(dr);
        //        }
        //    }

        //    if (dt.Rows.Count == 0 || rowIndex < 0)
        //    {
        //        dr = dt.NewRow();
        //        dt.Rows.Add(dr);
        //    }

        //    gvInvoiceItem.DataSource = dt;
        //    gvInvoiceItem.DataBind();
        //}

        //private DataTable CreateInvoiceItemTable(ICollection<InvoiceItem> invoiceItems, bool hasRow)
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Id");
        //    dt.Columns.Add("AgreementId");
        //    dt.Columns.Add("BiostatId");
        //    dt.Columns.Add("Rate");
        //    dt.Columns.Add("CompletedHours");
        //    dt.Columns.Add("Desc");
        //    dt.Columns.Add("AgreementDate");
        //    dt.Columns.Add("ProjectId");
        //    dt.Columns.Add("TotalCost");

        //    if (invoiceItems != null && invoiceItems.Count > 0)
        //    {
        //        foreach (var invoiceItem in invoiceItems)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = invoiceItem.Id;
        //            dr[1] = invoiceItem.ClientAgmtId;
        //            dr[2] = invoiceItem.BiostatId;
        //            dr[3] = invoiceItem.InvoiceRate;
        //            dr[4] = invoiceItem.InvoiceHr;
        //            dr[5] = invoiceItem.Desc;
        //            dr[6] = ""; //invoiceItem.ClientAgmt.ClientApprovalDate != null ? Convert.ToDateTime(invoiceItem.ClientAgmt.ClientApprovalDate).ToShortDateString() : "";
        //            dr[7] = ""; //invoiceItem.ClientAgmt.ProjectId;
        //            dr[8] = invoiceItem.InvoiceRate * invoiceItem.InvoiceHr;

        //            dt.Rows.Add(dr);
        //        }
        //    }
        //    else if (hasRow)
        //    {
        //        dt.Rows.Add();
        //    }

        //    return dt;
        //}

        protected void DownloadFile(object sender, EventArgs e)
        {
            int id = 0;
            string ccAbbrv = txtCCAbbrv.Value,
                   invoiceId = txtInvoiceId.Value,
                   strId = txtId.Value;

            //char[] delimiter = { '-', '_' };
            //string txtId = invoiceId.Split(delimiter)[1];

            Int32.TryParse(strId, out id);

            if (id > 0)
            {
                DataTable fileTable = new DataTable();
                fileTable.Columns.Add("FileName", System.Type.GetType("System.String"));
                fileTable.Columns.Add("FileUpload", typeof(byte[]));

                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    var query = db.Invoice1Set
                                .Where(c => c.Id == id)
                                .Select(x => new { x.FileName, x.FileUpload });

                    if (query != null)
                    {
                        foreach (var item in query.ToList())
                        {
                            DataRow dr = fileTable.NewRow();

                            dr[0] = item.FileName;
                            dr[1] = item.FileUpload;

                            fileTable.Rows.Add(dr);
                        }
                    }
                }

                if (fileTable.Rows.Count > 0)
                {
                    DataRow row = fileTable.Rows[0];

                    string name = (string)row["FileName"];
                    //string contentType = (string)row["ContentType"];
                    Byte[] data = (Byte[])row["FileUpload"];

                    // Send the file to the browser
                    //Response.AddHeader("Content-type", contentType);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + name);
                    Response.BinaryWrite(data);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static bool IsInvoiceIdValid(string invoiceId, string id)
        {
            bool isValid = false;

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var invoice = db.Invoice1Set.FirstOrDefault(a => a.InvoiceId == invoiceId);

                if (invoice == null || invoice.Id.ToString() == id)
                {
                    isValid = true;
                }

            }

            return isValid;
        }


    }
}