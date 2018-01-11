﻿using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace ProjectManagement.Admin
{
    public partial class ClientAgreementForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();
            }           
        }
                

        private void BindControl()
        {
            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                dropDownSource = db.CollabCtr
                                .OrderBy(c => c.Id)
                                .Where(c => c.Id > 0)
                                .Select(x => new { x.Id, FullName = (x.NameAbbrv + " | " + x.Name) })
                                .ToDictionary(c => c.Id, c => c.FullName);

                PageUtility.BindDropDownList(ddlCollab, dropDownSource, string.Empty);

                dropDownSource = db.Project2
                                .OrderByDescending(d => d.Id)
                                .Select(x => new { x.Id, FullName = (x.Id + " " + x.Title).Substring(0, 118) })
                                .ToDictionary(c => c.Id, c => c.FullName);
                
            }

            PageUtility.BindDropDownList(ddlProject, dropDownSource, string.Empty);

            //phase
            dropDownSource = new Dictionary<int, string>();
            for (int p = 1; p < 10; p++)
            {
                string s = string.Format("Phase-{0}", p);
                dropDownSource.Add(p, s);
            }
            PageUtility.BindDropDownList(ddlProjectPhaseHdn, dropDownSource, "");
            PageUtility.BindDropDownList(ddlProjectPhase, dropDownSource, "");

            BindRptClientAgmt();
        }

        protected void ddlCollab_Changed(Object sender, EventArgs e)
        {
            BindRptClientAgmt();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = 0;
            string ccAbbrv = txtCCAbbrv.Value,
                   agmtId = txtAgmtId.Value,
                   strId = txtId.Value;

            //char[] delimiter = { '-', '_' };

            //if (agmtId.Split(delimiter).Length > 1)
            //{
            //    Int32.TryParse(agmtId.Split(delimiter)[1], out id);
            //}

            Int32.TryParse(strId, out id);
            
            string strFilename = "";
            byte[] myData = new byte[0];
            // Check to see if file was uploaded
            if (fileUpload.PostedFile != null)
            {
                // Get a reference to PostedFile object
                HttpPostedFile myFile = fileUpload.PostedFile;

                // Get size of uploaded file
                int nFileLen = myFile.ContentLength;

                // make sure the size of the file is > 0
                if (nFileLen > 0)
                {
                    // Allocate a buffer for reading of the file
                    myData = new byte[nFileLen];

                    // Read uploaded file from the Stream
                    myFile.InputStream.Read(myData, 0, nFileLen);

                    // Create a name for the file to store
                    strFilename = System.IO.Path.GetFileName(myFile.FileName);

                    // Write data into a file
                    //WriteToFile(Server.MapPath(strFilename), ref myData);

                    //// Store it in database
                    //int nFileID = WriteToDB(strFilename, myFile.ContentType, ref myData);

                }
                

            }

            ClientAgmt agmt = GetClientAgmt(id, strFilename, myData);

            //save to database
            SaveClientAgmt(agmt);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(false), false);
            
            BindRptClientAgmt();
        }

        private void BindRptClientAgmt()
        {
            var dt = CreateAgreementDataTable(true);

            rptClientAgmt.DataSource = dt;
            rptClientAgmt.DataBind();
        }

        private DataTable CreateAgreementDataTable(bool isFromDb)
        {
            int collabId = 0;
            Int32.TryParse(ddlCollab.SelectedValue, out collabId);

            DataTable dt = new DataTable("tblClientAgmt");

            if (isFromDb)
            {
                dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            }
            dt.Columns.Add("AgmtId", System.Type.GetType("System.String"));
            dt.Columns.Add("PIName", System.Type.GetType("System.String"));
            dt.Columns.Add("ProjectId", System.Type.GetType("System.Int32"));
            dt.Columns.Add("ProjectPhase", System.Type.GetType("System.String"));
            dt.Columns.Add("ProjectTitle", System.Type.GetType("System.String"));
            dt.Columns.Add("ClientRefNum", System.Type.GetType("System.String"));
            dt.Columns.Add("ApprovedPhdRate", System.Type.GetType("System.String"));
            dt.Columns.Add("ApprovedPhdHr", System.Type.GetType("System.String"));
            dt.Columns.Add("InvoicedPhdHr", System.Type.GetType("System.String"));
            dt.Columns.Add("RemainingPhdHr", System.Type.GetType("System.String"));
            dt.Columns.Add("ApprovedMsRate", System.Type.GetType("System.String"));
            dt.Columns.Add("ApprovedMsHr", System.Type.GetType("System.String"));
            dt.Columns.Add("InvoicedMsHr", System.Type.GetType("System.String"));
            dt.Columns.Add("RemainingMsHr", System.Type.GetType("System.String"));

            dt.Columns.Add("RequestDate", System.Type.GetType("System.String"));
            dt.Columns.Add("ApprovalDate", System.Type.GetType("System.String"));
            dt.Columns.Add("ClientApprovalDate", System.Type.GetType("System.String"));
            dt.Columns.Add("CompletionDate", System.Type.GetType("System.String"));
            //dt.Columns.Add("CollabCtr", System.Type.GetType("System.String"));
            //dt.Columns.Add("EndDate", System.Type.GetType("System.String"));
            //dt.Columns.Add("BillingSchedule", System.Type.GetType("System.String"));
          
            dt.Columns.Add("Status", System.Type.GetType("System.String"));

            if (isFromDb)
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    ////var query = db.ClientAgmt
                    ////            .Join(db.CollabCtr, a => a.CollabCtrId, c => c.Id, (a, c) => new {a, c.NameAbbrv})
                    ////            .Join(db.Projects, ca => ca.a.ProjectId, p => p.Id,
                    ////                (ca, p) => new {ca, p.Title}
                    ////            );

                    //var qInvoice = from i in db.InvoiceItem2 
                    //               //join b in db.BioStats on i.BiostatId equals b.Id
                    //               select new { i.ClientAgmtId, i.StaffType, i.InvoiceHr} into s
                    //               group s by new { s.ClientAgmtId, s.StaffType } into g
                    //               select new
                    //               {
                    //                   ClientAgmtId = g.Key.ClientAgmtId,
                    //                   PhdInvoiceHr = g.Where(t => t.StaffType == "phd").Sum(t => t.InvoiceHr),
                    //                   MsInvoiceHr = g.Where(t => t.StaffType != "phd").Sum(t => t.InvoiceHr)
                    //               };

                    //var query = from a in db.ClientAgmt
                    //            join i in qInvoice on a.Id equals i.ClientAgmtId
                    //            join p in db.Project2 on a.ProjectId equals p.Id
                    //            select new { a.Id, a.AgmtId, a.ProjectId, a.ProjectPhase, p.Title, a.ApprovedPhdHr, a.ApprovedMsHr, 
                    //                        PhdInvoiceHr = (i.PhdInvoiceHr == null ? 0 : i.PhdInvoiceHr), 
                    //                        MsInvoiceHr = (i.MsInvoiceHr == null ? 0 : i.MsInvoiceHr),
                    //                        a.AgmtStatus, a.CollabCtrId };                            

                    var query = db.vwAgreement.Where(a => a.CollabCtrId > 0);

                    //foreach (var q in query.OrderBy(a => a.ca.a.CollabCtr.NameAbbrv).ThenByDescending(a => a.ca.a.Id).ToList())
                    if (collabId > 0)
                    {
                        query = query.Where(c => c.CollabCtrId == collabId);
                    }

                    foreach (var q in query.Distinct().ToList())
                    {
                        DataRow dr = dt.NewRow();

                        dr["Id"] = q.Id;
                        dr["AgmtId"] = q.AgmtId;
                        dr["PIName"] = q.PIName;
                        dr["ProjectId"] = q.ProjectId;
                        dr["ProjectPhase"] = q.ProjectPhase;
                        dr["ProjectTitle"] = q.Title;
                        dr["ClientRefNum"] = q.ClientRefNum;
                        dr["ApprovedPhdRate"] = q.ApprovedPhdRate;
                        dr["ApprovedPhdHr"] = q.ApprovedPhdHr;
                        dr["InvoicedPhdHr"] = q.PhdInvoiceHr;
                        dr["RemainingPhdHr"] = q.PhdRemaining;
                        dr["ApprovedMsRate"] = q.ApprovedMsRate;
                        dr["ApprovedMsHr"] = q.ApprovedMsHr;
                        dr["InvoicedMsHr"] = q.MsInvoiceHr;
                        dr["RemainingMsHr"] = q.MsRemaining;
                        dr["RequestDate"] = q.RequestDate;
                        dr["ApprovalDate"] = q.ApprovalDate;
                        dr["ClientApprovalDate"] = q.ClientApprovalDate;
                        dr["CompletionDate"] = q.CompletionDate;
                        dr["Status"] = q.AgmtStatus;

                        dt.Rows.Add(dr);
                    }
                }
            }
            else
            {
                foreach (RepeaterItem item in rptClientAgmt.Items)
                {
                    DataRow dr = dt.NewRow();

                    //dr["Id"] = ((Label)item.FindControl("lblId")).Text;
                    dr["AgmtId"] = ((Label)item.FindControl("lblAgmtId")).Text;
                    dr["PIName"] = ((Label)item.FindControl("lblPIName")).Text;
                    dr["ProjectId"] = ((Label)item.FindControl("lblProjectId")).Text;
                    dr["ProjectPhase"] = ((Label)item.FindControl("lblProjectPhase")).Text;
                    dr["ProjectTitle"] = ((Label)item.FindControl("lblProjectTitle")).Text;
                    dr["ClientRefNum"] = ((Label)item.FindControl("lblClientRefNum")).Text;
                    dr["ApprovedPhdRate"] = ((Label)item.FindControl("lblApprovedPhdRate")).Text;
                    dr["ApprovedPhdHr"] = ((Label)item.FindControl("lblApprovedPhdHr")).Text;
                    dr["InvoicedPhdHr"] = ((Label)item.FindControl("lblInvoicedPhdHr")).Text;
                    dr["RemainingPhdHr"] = ((Label)item.FindControl("lblRemainingPhdHr")).Text;
                    dr["ApprovedMsRate"] = ((Label)item.FindControl("lblApprovedMsRate")).Text;
                    dr["ApprovedMsHr"] = ((Label)item.FindControl("lblApprovedMsHr")).Text;
                    dr["InvoicedMsHr"] = ((Label)item.FindControl("lblInvoicedMsHr")).Text;
                    dr["RemainingMsHr"] = ((Label)item.FindControl("lblRemainingMsHr")).Text;
                    dr["RequestDate"] = ((Label)item.FindControl("lblRequestDate")).Text;
                    dr["ApprovalDate"] = ((Label)item.FindControl("lblApprovalDate")).Text;
                    dr["ClientApprovalDate"] = ((Label)item.FindControl("lblClientApprovalDate")).Text;
                    dr["CompletionDate"] = ((Label)item.FindControl("lblCompletionDate")).Text;
                    dr["Status"] = ((Label)item.FindControl("lblStatus")).Text;

                    dt.Rows.Add(dr);
                }

                if (hdnSortOrder.Text.Contains(","))
                {
                    var strSort = hdnSortOrder.Text.Split(',');
                    var sortColumn = (AgreementExportEnum) Int32.Parse(strSort[0]);
                    var sortOrder = strSort[1].ToUpper();

                    DataView dvData = new DataView(dt);
                    dvData.Sort = sortColumn + " " + sortOrder;

                    dt = dvData.ToTable();
                }
            }

            return dt;
        }

        private int SaveClientAgmt(ClientAgmt ca)
        {
            int caid = -1;

            try
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    if (ca.Id > 0) //update
                    {
                        var prevClientAgmt = db.ClientAgmt.FirstOrDefault(c => c.Id == ca.Id);

                        if (prevClientAgmt != null)
                        {
                            ca.CollabCtrId = prevClientAgmt.CollabCtrId;

                            if (prevClientAgmt.FileName != null && prevClientAgmt.FileUpload != null)
                            {
                                if (ca.FileUpload.Count<byte>() == 0 && prevClientAgmt.FileUpload.Count<byte>() > 0)
                                {
                                    ca.FileName = prevClientAgmt.FileName;
                                    ca.FileUpload = prevClientAgmt.FileUpload;
                                }
                            }

                            db.Entry(prevClientAgmt).CurrentValues.SetValues(ca);
                        }
                    }
                    else //new
                    {
                        db.ClientAgmt.Add(ca);
                    }

                    db.SaveChanges();

                    caid = ca.Id;
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

            return caid;
        }

        private ClientAgmt GetClientAgmt(int id, string strFilename, byte[] myData)
        {
            int intOutput;
            DateTime dt1, dt2, dt3, dt4;
            Decimal dc = 0.0m;

            ClientAgmt ca = new ClientAgmt()
            {
                Id = id,
                CollabCtrId = Int32.TryParse(Request.QueryString["Id"], out intOutput) ? intOutput : -1,
                Project2Id = Int32.TryParse(ddlProject.SelectedValue, out intOutput) ? intOutput : -1,
                ProjectPhase = ddlProjectPhase.SelectedItem.Text,
                ClientRefNum = txtClientRefNum.Value,
                RequestDate = DateTime.TryParse(txtRequestDate.Text, out dt1) ? dt1 : (DateTime?)null,
                ApprovedPhdHr = Decimal.TryParse(txtPhdHour.Value, out dc) ? dc : (Decimal?)null,
                ApprovedPhdRate = Decimal.TryParse(txtPhdRate.Value, out dc) ? dc : (Decimal?)null,
                ApprovedMsHr = Decimal.TryParse(txtMsHour.Value, out dc) ? dc : (Decimal?)null,
                ApprovedMsRate = Decimal.TryParse(txtMsRate.Value, out dc) ? dc : (Decimal?)null,
                ApprovalDate = DateTime.TryParse(txtBqhsApprovalDate.Text, out dt2) ? dt2 : DateTime.Now,
                ClientApprovalDate = DateTime.TryParse(txtClientApprovalDate.Text, out dt3) ? dt3 : (DateTime?)null,
                CompletionDate = DateTime.TryParse(txtCompletionDate.Text, out dt4) ? dt4 : (DateTime?)null,
                FileName = strFilename,
                FileUpload = myData,
                Comments = txtComments.Value,
                Creator = Page.User.Identity.Name,
                CreateDate = DateTime.Now,
                AgmtId = txtAgmtId.Value,
                AgmtStatus = ddlStatus.Value
            };
            
            return ca;
        }

        protected void rptClientAgmt_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {
            if (((Button)e.CommandSource).Text.Equals("Edit"))
            {
                int id = 0;
                int.TryParse(((Button)e.CommandSource).CommandArgument, out id);

                //txtAgmtId.Value = ((Button)e.CommandSource).CommandArgument;

                if (id > 0)
                {
                    ClientAgmt ca = GetClientAgmtById(id);

                    if (ca != null)
                    {
                        SetClientAgmt(ca);

                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                        //           "ModalScript", PageUtility.LoadEditScript(true), false);
                        StringBuilder sb = new StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#editModal').modal('show');");
                        sb.Append("$('#MainContent_fileUpload').fileinput('enable');");
                        //sb.Append("var projectId = $('#MainContent_ddlProject').val(); ");
                        //sb.Append("bindPI(projectId);");
                        sb.Append("calcTotal();");
                        sb.Append(@"</script>");

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                   "ModalScript", sb.ToString(), false);
                    }
                }
            }
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            //string filePath = (sender as LinkButton).CommandArgument;
            //Response.ContentType = ContentType;
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            //Response.WriteFile(filePath);
            //Response.End();

            int id = 0;
            string ccAbbrv = txtCCAbbrv.Value,
                   agmtId = txtAgmtId.Value,
                   strId = txtId.Value;

            //char[] delimiter = { '-', '_' };
            //string txtId = agmtId.Split(delimiter)[1];

            Int32.TryParse(strId, out id); 
     
            if (id > 0)
            {
                DataTable fileTable = new DataTable();
                fileTable.Columns.Add("FileName", System.Type.GetType("System.String"));
                fileTable.Columns.Add("FileUpload", typeof(byte[]));

                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    var query = db.ClientAgmt
                                .Where(c => c.Id == id)
                                .Select(x => new {x.FileName, x.FileUpload});

                    if (query != null)
                    {
                       foreach(var item in query.ToList())
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


        private void SetClientAgmt(ClientAgmt ca)
        {
            txtId.Value = ca.Id.ToString();
            txtCCAbbrv.Value = ca.CollabCtr.NameAbbrv;
            txtAgmtId.Value = ca.AgmtId;    //ca.Id > 0 ? txtCCAbbrv.Value + '-' + ca.Id.ToString() : "";
            ddlProject.SelectedValue = ca.Project2Id.ToString();
            txtPI.Value = ca.Project2.Invests.FirstName + ' ' + ca.Project2.Invests.LastName;
            txtProjectType.Value = Enum.GetName(typeof(ProjectType), ca.Project2.ProjectType);

            int phaseId = 0;
            string[] phase = ca.ProjectPhase.Split('-');
            if (phase.Length > 1)
            {
                Int32.TryParse(phase[1], out phaseId);
            }

            ddlProjectPhase.SelectedValue = phaseId > 0 ? phaseId.ToString() : "";

            txtClientRefNum.Value = ca.ClientRefNum;
            txtRequestDate.Text = ca.RequestDate != null ? Convert.ToDateTime(ca.RequestDate).ToShortDateString() : "";
            
            txtPhdRate.Value = ca.ApprovedPhdRate != null ? ca.ApprovedPhdRate.ToString() : "";
            txtPhdHour.Value = ca.ApprovedPhdHr != null ? ca.ApprovedPhdHr.ToString() : "";           
            txtMsRate.Value = ca.ApprovedMsRate != null ? ca.ApprovedMsRate.ToString() : "";
            txtMsHour.Value = ca.ApprovedMsHr != null ? ca.ApprovedMsHr.ToString() : "";
            txtBqhsApprovalDate.Text = ca.ApprovalDate != null ? Convert.ToDateTime(ca.ApprovalDate).ToShortDateString() : "";
            txtClientApprovalDate.Text = ca.ClientApprovalDate != null ? Convert.ToDateTime(ca.ClientApprovalDate).ToShortDateString() : "";
            txtCompletionDate.Text = ca.CompletionDate != null ? Convert.ToDateTime(ca.CompletionDate).ToShortDateString() : "";
            ddlStatus.Value = ca.AgmtStatus != null ? ca.AgmtStatus : "";

            //fileUpload
            lnkFile.Text = ca.FileName;
            
            txtComments.Value = ca.Comments;
        }

        private ClientAgmt GetClientAgmtById(int id)
        {
            ClientAgmt ca = null;

            using (var db = new ProjectTrackerContainer())
            {
                ca = db.ClientAgmt
                    .Include(a => a.Project2.Invests)
                    .Include(a => a.CollabCtr)
                    .FirstOrDefault(a => a.Id == id);

                //ca.CollabCtr = ca.CollabCtr;
                //ca.Project2.Invests = ca.Project2.Invests;
                //ca.ProjectPhase = ca.ProjectPhase;
                //ca.AgmtStatus = ca.AgmtStatus;

                //if (ca.Project2Id > 0)
                //{
                //    var query = db.ProjectPhase.Where(p => p.ProjectId == ca.Project2Id).ToList();

                //    //ListItem removeItem=ddlProjectPhase.Items.FindByValue("1");
                //    //ddlProjectPhase.Items.Remove(removeItem);

                //    //foreach (var p in query)
                //    //{
                //    //    foreach (ListItem li in ddlProjectPhaseHdn.Items)
                //    //    {
                //    //        ListItem newItem = new ListItem();
                //    //        newItem.Value = li.Value;
                //    //        newItem.Text = li.Text;
                //    //        newItem.Selected = false;

                //    //        if (li.Text == p.Name)
                //    //        {
                //    //            newItem.Selected = true;
                //    //        }

                //    //        ddlProjectPhase.Items.Add(newItem);
                //    //    }
                //    //}

                //    //ddlProjectPhase.Items.Insert(0, new ListItem(string.Empty, String.Empty));

                //    foreach (ListItem li in ddlProjectPhase.Items)
                //    {
                //        //if (query.Contains(li.Text))
                //        //remove item if not in query
                //    }
                //}
            }

            return ca;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearEditForm();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(true), false);
        }

        private void ClearEditForm()
        {
            ddlProjectPhase.Items.Clear();
            ddlStatus.Items.Clear();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static bool IsAgmtIdValid(string agmtId, string id)
        {
            bool isValid = false;

            using (var db = new ProjectTrackerContainer())
            {
                var agmt = db.ClientAgmt.FirstOrDefault(a => a.AgmtId == agmtId);

                if (agmt == null || agmt.Id.ToString() == id)
                {
                    isValid = true;
                }

            }

            return isValid;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = CreateAgreementDataTable(false);

            int collabId = 0;
            Int32.TryParse(ddlCollab.SelectedValue, out collabId);

            string strCollab = collabId > 0 ? (ddlCollab.SelectedItem.Text.Split('|')[0]).Trim() : "all";

            string fileValue = String.Format("Agreement-{0}.xlsx", strCollab);

            FileExport fileExport = new FileExport(this.Response);
            fileExport.ExcelExport(dt, fileValue);
        }

    }
}