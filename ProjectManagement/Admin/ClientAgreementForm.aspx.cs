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
    /// <summary>
    /// @File: ClientAgreementForm.aspx.cs
    /// @Front End : ClientAgreementForm.aspx
    /// @Author: Yang Rui
    /// @Summary: Form to keep track of information in the signed client agreement form. Also links a project and a phase
    ///           to the corresponding Collaboration Center to keep track of approved PhD and MS hours for a project.
    ///           The agreement is also uploaded for records into the database.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018AUG08 - Jason Delos Reyes  -  Added documentation for easier readibility and maintainability.
    ///                                 -  Reordered the dropdown of Collaboration Centers from in order by
    ///                                    Collaboration Center ID number to the alphabetical order of the
    ///                                    Collaboration Center name abbreviation.
    ///  2018AUG09 - Jason Delos Reyes  -  Made Projects dropdown typeable and searchable to make it easier
    ///                                    to look for projects with new client agreements.  Also able to
    ///                                    make custom size for dropdown through overriding its CSS code.
    ///  2018SEP11 - Jason Delos Reyes  -  Edited bindAgmtId() to add a leading "0" before writing the name
    ///                                    of the new agreement Id to conform to new naming convention.
    ///                                 -  Added documentation to the Javascript code on the front-end
    ///                                    code for code readability for some of the functions that play
    ///                                    a role in display features of the page (e.g., auto-creation of
    ///                                    agreement number for new Client Agreement forms).
    ///  2018SEP13 - Jason Delos Reyes  -  Created functionality for dropdown of client agreement form on
    ///                                    Collaborative Center page.  URL now allows "ClientAgmtId" value,
    ///                                    which should open edit modal with corresponding client agmt
    ///                                    information, to be linked from dropdown in Collab Ctr form.
    ///  2018SEP24 - Jason Delos Reyes  -  Fixed "update button" functionality so that the program would save
    ///                                    the newly-entered information upon clicking "update", display a
    ///                                    success message, and reopen the same page.
    ///  2018OCT03 - Jason Delos Reyes  -  Updated the "update" button so that the internal client agreement id 
    ///                                    number is not exposed, while keeping the same page open.  Will still
    ///                                    need to address for dropdown link to client agreement form in 
    ///                                    collaborative center form.
    ///  2018OCT04 - Jason Delos Reyes  -  The Collab Center dropdown - to - Client Agreement form feature no
    ///                                    longer reveals the internal Client Agreement ID number.  It now
    ///                                    just uses abbreviation agreement ID number, which is already
    ///                                    known by the user and can not be altered (unless to enter an external
    ///                                    pseudo-client record, which is highly unlikely since the page can
    ///                                    only be used by an admin account).
    ///  2019JAN16 - Jason Delos Reyes  -  Added feature to be able to partition between client agreemnts
    ///                                    with currently active and/or inactive collaborative centers.
    ///  2019JAN17 - Jason Delos Reyes  -  Added "searchable dropdown" so users are able to search the 
    ///                                    dropdown a specific collaborative center in mind.
    ///  2019MAR11 - Jason Delos Reyes  -  Adjusted "Update" functionality so that the page is redirected to the
    ///                                    same page by the URL string instead of simply stopping the modal from closing.
    ///  2019MAY06 - Jason Delos Reyes  -  Increased the potential number of phases to 20 to increase the number of phases
    ///                                    that could be displayed on the Client Agreement Form.
    /// </summary>
    public partial class ClientAgreementForm : System.Web.UI.Page
    {

        /// <summary>
        /// Loads page with fields with the correct information
        /// in preparation for data entry and/or interaction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();

                int id = 0;
                string clientAgmtId = Request.QueryString["ClientAgmtId"];
                if (clientAgmtId != null)
                {
                    ClientAgmt ca = GetClientAgmtByAgmtAbbrv(clientAgmtId);
                    if (ca != null)
                        id = ca.Id;
                }
                //Int32.TryParse(ca.Id, out id);
                if (id > 0)
                {
                    OpenClientAgmt(id);
                }

            }
            //else
            //{
                
            //    //Reopens modal after page postback.
            //    ClientScript.RegisterStartupScript(GetType(), "ModalScript", "$('#editModal').modal('show');", true);
                 
            //}
           
        }
                
        /// <summary>
        /// Binds the necessary drop down sources from the database and
        /// binds the proper client agreement form if necessary.
        /// </summary>
        private void BindControl()
        {
            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();
            

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                dropDownSource = db.CollabCtr
                                .OrderBy(c=>c.NameAbbrv)//.OrderBy(c => c.Id)
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
            for (int p = 1; p < 20; p++)
            {
                string s = string.Format("Phase-{0}", p);
                dropDownSource.Add(p, s);
            }
            PageUtility.BindDropDownList(ddlProjectPhaseHdn, dropDownSource, "");
            PageUtility.BindDropDownList(ddlProjectPhase, dropDownSource, "");

            BindRptClientAgmt();
            

        }

        /// <summary>
        /// Updates list of client agreements with collaborative centers between
        ///     - currently active (no end date specified or not approached end date specified)
        ///     - currently inactive (end date specified and passed)
        ///     - or all.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void collabCenterType_Changed(Object sender, EventArgs e)
        {
            BindRptClientAgmt();
        }

        /// <summary>
        /// Display a list of Client Agreements on the data table if the collaboration center
        /// choice has been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCollab_Changed(Object sender, EventArgs e)
        {
            BindRptClientAgmt();
        }

        /// <summary>
        /// Saves the current project form into the database when the "Save" button has been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            string appPath = String.Empty;
            appPath = HttpContext.Current.Request.ApplicationPath.Length > 1 ? HttpContext.Current.Request.ApplicationPath
                                                                                + "/Admin/"
                                                                             : String.Empty;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "successMsg",
                "alert('Record Saved Successfully'); window.location='" + appPath
                                                                        + "ClientAgreementForm?ClientAgmtId="
                                                                        + agmtId + "';", true);

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
            //           "ModalScript", PageUtility.LoadEditScript(false), false);
           
            //BindRptClientAgmt(id);

            //Response.Redirect(String.Format("~/Admin/ClientAgreementForm?ClientAgmtId={0}", agmtId));

        }

        /// <summary>
        /// Binds the Client Agreement from the database to the web form.
        /// </summary>
        private void BindRptClientAgmt()
        {
            var dt = CreateAgreementDataTable(true);

            rptClientAgmt.DataSource = dt;
            rptClientAgmt.DataBind();

        }

        /// <summary>
        /// Takes the value of the client agreement id in order to 
        /// save the newly entered values, display a success message, and 
        /// refresh to the same page.
        /// </summary>
        /// <param name="id">Client Agreement ID.</param>
        private void BindRptClientAgmt(int id)
        {

            var dt = CreateAgreementDataTable(true);

            rptClientAgmt.DataSource = dt;
            rptClientAgmt.DataBind();

            if (id > 0)
            {
               
                OpenClientAgmt(id);
                
            }
        }

        /// <summary>
        /// Populates Client Agreement Form based on what has been recorded
        /// in the database and/or obtains values from textboxes on the web form.
        /// </summary>
        /// <param name="isFromDb">Specifies if current client agreement form should be pulled
        ///                        from the database or not.</param>
        /// <returns></returns>
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

                    var query = db.vwAgreement.Where(a => a.CollabCtrId > 0).Join(db.CollabCtr, a => a.CollabCtrId, c => c.Id, (a, c) => new { a, c });

                    //foreach (var q in query.OrderBy(a => a.ca.a.CollabCtr.NameAbbrv).ThenByDescending(a => a.ca.a.Id).ToList())
                    if (collabId > 0)
                    {
                        query = query.Where(c => c.a.CollabCtrId == collabId);
                    }

                    switch (collabCenterType.SelectedValue)
                    {
                        case "active":
                            query = query.Where(y=>y.c.EndDate == null || (y.c.EndDate != null && y.c.EndDate >= DateTime.Today));
                            break;
                        case "inactive":
                            query = query.Where(y=>y.c.EndDate != null && y.c.EndDate < DateTime.Today);
                            break;
                        default:
                            break;
                    }

                    foreach (var q in query.Distinct().ToList())
                    {
                        DataRow dr = dt.NewRow();

                        dr["Id"] = q.a.Id;
                        dr["AgmtId"] = q.a.AgmtId;
                        dr["PIName"] = q.a.PIName;
                        dr["ProjectId"] = q.a.ProjectId;
                        dr["ProjectPhase"] = q.a.ProjectPhase;
                        dr["ProjectTitle"] = q.a.Title;
                        dr["ClientRefNum"] = q.a.ClientRefNum;
                        dr["ApprovedPhdRate"] = q.a.ApprovedPhdRate;
                        dr["ApprovedPhdHr"] = q.a.ApprovedPhdHr;
                        dr["InvoicedPhdHr"] = q.a.PhdInvoiceHr;
                        dr["RemainingPhdHr"] = q.a.PhdRemaining;
                        dr["ApprovedMsRate"] = q.a.ApprovedMsRate;
                        dr["ApprovedMsHr"] = q.a.ApprovedMsHr;
                        dr["InvoicedMsHr"] = q.a.MsInvoiceHr;
                        dr["RemainingMsHr"] = q.a.MsRemaining;
                        dr["RequestDate"] = q.a.RequestDate;
                        dr["ApprovalDate"] = q.a.ApprovalDate;
                        dr["ClientApprovalDate"] = q.a.ClientApprovalDate;
                        dr["CompletionDate"] = q.a.CompletionDate;
                        dr["Status"] = q.a.AgmtStatus;

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

        /// <summary>
        /// Saves current client agreement instance into the database.
        /// </summary>
        /// <param name="ca">Referred Client Agreement</param>
        /// <returns>Integer</returns>
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

        /// <summary>
        /// With the given client agreement information, extracts the client agreement information
        /// from the web form into a new client agreement instance and returns this client
        /// agreement instance.
        /// </summary>
        /// <param name="id">Client Agreement Id.</param>
        /// <param name="strFilename">Name of uploaded Client Agreement file.</param>
        /// <param name="myData">Uploaded client agreement file.</param>
        /// <returns>Instance of client agreement.</returns>
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

        /// <summary>
        /// When "Edit" command is clicked, an existing form corresponding to the one referred to
        /// is populated into the web form for review and editing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptClientAgmt_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {
            if (((Button)e.CommandSource).Text.Equals("Edit"))
            {
                int id = 0;
                int.TryParse(((Button)e.CommandSource).CommandArgument, out id);

                //txtAgmtId.Value = ((Button)e.CommandSource).CommandArgument;

                OpenClientAgmt(id);
            }
        }

        /// <summary>
        /// Opens the pop-up page of the client agreement form.
        /// </summary>
        /// <param name="id">Referred Client Agreement Id.</param>
        protected void OpenClientAgmt(int id)
        {
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


        /// <summary>
        /// With the given client agreement, sets the web form from the provided
        /// instance of the client agreement.
        /// </summary>
        /// <param name="ca">Provided instance of the client agreement.</param>
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

        /// <summary>
        /// Based on the given Client Agreement ID, returns the instance of a client
        /// agreement instance.
        /// </summary>
        /// <param name="id">Given Client Agreement ID.</param>
        /// <returns>Instance of the Client Agreement matched to given ID.</returns>
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

        /// <summary>
        /// Based on the given Client Agreement abbreviation, returns the instance of a client
        /// agreement instance.
        /// </summary>
        /// <param name="agmtAbbrv">Given Client Agreement Name Abbreviation.</param>
        /// <returns>Instance of the Client Agreement matched with its name abbrevation.</returns>
        private ClientAgmt GetClientAgmtByAgmtAbbrv(string agmtAbbrv)
        {
            ClientAgmt ca = null;

            using (var db = new ProjectTrackerContainer())
            {
                ca = db.ClientAgmt
                    .Include(a => a.Project2.Invests)
                    .Include(a => a.CollabCtr)
                    .FirstOrDefault(a => a.AgmtId == agmtAbbrv);
                
            }

            return ca;
        }

        /// <summary>
        /// Prepares a clean new Client Agreement form when "Add Client Agreement" button 
        /// is clicked from the Collaboration Center.
        /// 
        /// Note: There is no way of adding a new client agreement form from the 
        /// Admin > Billing > Client Agreement section of the tracking database at the moment.
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
        /// Clears Client Agreement form by clearing otu the choices of the Project Phase and Status dropdown boxes.
        /// </summary>
        private void ClearEditForm()
        {
            ddlProjectPhase.Items.Clear();
            ddlStatus.Items.Clear();
        }
        
        /// <summary>
        /// Generates JSON response to verify that a valid Client Agreement ID 
        /// has been generated in the system.
        /// </summary>
        /// <param name="agmtId">Referred Client Agreement ID.</param>
        /// <param name="id">Client Agreement ID number (NOT abbreviated AgmtId).</param>
        /// <returns>isValid - Boolean value on whether or not a matching Agreement ID
        ///                    has already been found in the database.</returns>
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

        /// <summary>
        /// Exports the list of Client Agreements that have been listed in the main collabration center data table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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