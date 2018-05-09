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
    /// @File: AcademicForm.aspx.cs
    /// @Author: Yang Rui
    /// @Summary: Academic Form of Project Tracking System.
    /// 
    ///           Keeps track of academic tracking information such as seminar/workshop/training/lecture given, 
    ///           education courses taught by faculty, service in thesis and/or dissertation committees, 
    ///           participation in a panel or committee, journal and/or grant review activities, honors and/or
    ///           awards received, professional training attended by faculty or staff, mentor for K awards or
    ///           other grants, participation in the data safety monitoring committee, serving as a mentor
    ///           for student, or any other professional activities pertinent.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018MAY09 - Jason Delos Reyes  -  Added comments/documentation for easier legibility and
    ///                                    easier data structure view and management.
    ///  2018MAY09 - Jason Delos Reyes  -  Added "mentor for student" section to be able to distinguish 
    ///                                    student mentor from other activities (such as participation 
    ///                                    in their thesis/dissertation committees).
    /// </summary>
    public partial class AcademicForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();
            }    
        }        

        protected void ddlEvent_Changed(Object sender, EventArgs e)
        {
            //BindProject(-1);

            int eventId = 0;

            Int32.TryParse(ddlEvent.SelectedValue, out eventId);

            BindControl(eventId);

            //lblMsg.Text = selectedProjectId.ToString();
        }

        private void BindControl(int eventId)
        {
            //if (eventId > 0)
            //{
                var academic = new Academic();
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    var ae = db.Academic.FirstOrDefault(a => a.Id == eventId);

                    if (ae != null)
                    {
                        academic = ae;
                    }
                }

                BindControl(academic);
            //}
        }

        private void BindControl(Academic ae)
        {
            int typeId = 0;
            Int32.TryParse(ddlAcademicType.SelectedValue, out typeId);

            if (ae.Id == 0)
            {
                ae.AcademicTypeId = typeId;
                ae.Title = "";
                ae.Organization = "";
                ae.BiostatBitSum = 0;
                ae.Comments = "";
            }

            //if (ae.Id > 0)
            //{
            if (ae.AcademicTypeId > 0)
                ddlAcademicType.SelectedValue = ae.AcademicTypeId.ToString();

                txtComments.Value = ae.Comments;
                //txtCheckCode.Value = "";

                BindTable(rptBiostat, (int)ae.BiostatBitSum);

                string Title = ae.Title
                      , Organization = ae.Organization
                      , StartDate = ae.StartDate != null ? DateTime.Parse(ae.StartDate.ToString()).ToShortDateString() : ""
                      , StartYear = ae.StartDate != null ? DateTime.Parse(ae.StartDate.ToString()).Year.ToString() : ""
                      , EndDate = ae.EndDate != null ? DateTime.Parse(ae.EndDate.ToString()).ToShortDateString() : ""
                      , EndYear = ae.EndDate != null ? DateTime.Parse(ae.EndDate.ToString()).Year.ToString() : ""
                      , NumOfAttendees = ae.NumOfAttendees != null ? ae.NumOfAttendees.ToString() : ""
                      , CourseNum = ae.CourseNum
                      , NumOfCredits = ae.NumOfCredits != null ? ae.NumOfCredits.ToString() : ""
                      , StudentName = ae.Name
                      , StudentAdvisorName = ae.Advisor
                      , Location = ae.Location
                      , AcademicDesc = ae.AcademicDesc
                      , StartSemesterId = ae.StartSemesterId == null ? "" : ae.StartSemesterId.ToString()
                      , EndSemesterId = ae.EndSemesterId == null ? "" : ae.EndSemesterId.ToString()
                      , FieldId = ae.FieldId != null ? ae.FieldId.ToString() : "";

                switch (ae.AcademicTypeId)
                {
                    case 1:
                        txtOrganization.Value = Organization;
                        txtTitle.Value = Title;
                        txtStartDate.Text = StartDate;
                        txtEndDate.Text = EndDate;
                        txtNumOfAttendees.Value = NumOfAttendees;
                        txtCourseNum.Value = CourseNum;
                        break;
                    case 2:
                        ddlSemester.SelectedValue = StartSemesterId;
                        txtYear.Value = StartYear;
                        txtCourseTitle.Value = Title;
                        txtCourseNumTeaching.Value = CourseNum;
                        txtNumOfCredits.Value = NumOfCredits;
                        txtNumOfStudents.Value = NumOfAttendees;
                        break;
                    case 3:
                        ddlCommitteeType.SelectedValue = FieldId;
                        txtDepartment.Value = Organization;
                        txtStudentName.Value = StudentName;
                        txtAdvisorName.Value = StudentAdvisorName;
                        txtProjectTitle.Value = Title;
                        ddlStartSemester.SelectedValue = StartSemesterId;
                        ddlEndSemester.SelectedValue = EndSemesterId;
                        txtStartYear.Value = StartYear;
                        txtEndYear.Value = EndYear;
                        break;
                    case 4:
                        txtPanelOrg.Value = Organization;
                        txtPanelName.Value = Title;
                        txtPanelStartDate.Text = StartDate;
                        txtPanelEndDate.Text = EndDate;
                        ddlPanelCommittee.SelectedValue = FieldId;
                        break;
                    case 5:
                        txtJournalName.Value = Title;
                        txtJournalDate.Text = StartDate;
                        break;
                    case 6:
                        ddlGrantAgency.SelectedValue = FieldId;
                        txtGrantDate.Text = StartDate;
                        break;
                    case 7:
                        txtAwardAgency.Value = Organization;
                        txtAwardTitle.Value = Title;
                        txtAwardDate.Text = StartDate;
                        break;
                    case 8:
                        txtTrainingProvider.Value = Organization;
                        ddlTrainingType.SelectedValue = FieldId;
                        txtTrainingTitle.Value = Title;
                        txtTrainingDate.Text = StartDate;
                        txtTrainingloc.Value = Location;
                        break;
                    case 9:
                        txtMentorTitle.Value = Title;
                        txtMenteeName.Value = StudentName;
                        txtMenteeAffil.Value = Organization;
                        txtMentorStartDate.Text = StartDate;
                        txtMentorEndDate.Text = EndDate;
                        break;
                    case 10:
                        txtSafeyTitle.Value = Title;
                        txtPI.Value = StudentName;
                        txtPIAffil.Value = Organization;
                        txtSafetyStartDate.Text = StartDate;
                        txtSafetyEndDate.Text = EndDate;
                        break;
                    case 11:
                        txtOtherTitle.Value = Title;
                        txtOtherDesc.Value = AcademicDesc;
                        txtOtherDate.Text = StartDate;
                        break;
                    case 12:
                        txtStudentMentorStudentName.Value = StudentName;
                        txtStudentMentorDepartment.Value = Organization;
                        txtStudentMentorTitle.Value = Title;
                        txtStudentMentorDesc.Value = AcademicDesc;
                        txtStudentMentorStartDate.Text = StartDate;
                        txtStudentMentorEndDate.Text = EndDate;
                        break;
                    default:
                        break;
                }
            //}
            
        }

        private void BindControl()
        {                        
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var dropDownSource = new Dictionary<int, string>();

                dropDownSource = db.Academic
                                .Where(a => a.Id > 0)
                                .OrderByDescending(d => d.Id)
                                .Select(x => new { x.Id, FullName = (x.Id + " " + x.Title).Substring(0, 99) })
                                .ToDictionary(c => c.Id, c => c.FullName);

                PageUtility.BindDropDownList(ddlEvent, dropDownSource, "Add new event");

                dropDownSource = db.Academic
                                .Join(db.AcademicField, a => a.AcademicTypeId, f => f.Id, (a, f) => new { a.Id, f.Name, f.Category})
                                .Where(a => a.Id > 0 && a.Category == "AcademicType")
                                .OrderByDescending(d => d.Id)
                                .Select(x => new { x.Id, FullName = (x.Id + " " + x.Name)})
                                .ToDictionary(c => c.Id, c => c.FullName);

                PageUtility.BindDropDownList(ddlEventHdn, dropDownSource, "Add new event");

                dropDownSource = db.BioStats
                                .Where(b => b.BitValue > 0 && b.EndDate >= DateTime.Now && b.Name != "N/A")
                                .OrderBy(b => b.Id)
                                .ToDictionary(c => (int)c.BitValue, c => c.Name);

                BindTable2(dropDownSource, rptBiostat);
               
                //dropDownSource = db.AcademicField.OfType<AcademicType>().OrderBy(f => f.DisplayOrder).ToDictionary(c => c.Id, c => c.Name);
                dropDownSource = db.AcademicField.Where(f => f.Category == "AcademicType").OrderBy(f => f.DisplayOrder).ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlAcademicType, dropDownSource, "--- please select ---");
                
                //dropDownSource = db.AcademicField.OfType<AcademicSemester>().OrderBy(f => f.DisplayOrder).ToDictionary(c => c.Id, c => c.Name);
                dropDownSource = db.AcademicField.Where(f => f.Category == "Semester").OrderBy(f => f.DisplayOrder).ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlSemester, dropDownSource, string.Empty);
                PageUtility.BindDropDownList(ddlStartSemester, dropDownSource, string.Empty);
                PageUtility.BindDropDownList(ddlEndSemester, dropDownSource, string.Empty);

                dropDownSource = db.AcademicField.Where(f => f.Category == "CommitteeType").OrderBy(f => f.DisplayOrder).ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlCommitteeType, dropDownSource, string.Empty);

                dropDownSource = db.AcademicField.Where(f => f.Category == "RoleInCommittee").OrderBy(f => f.DisplayOrder).ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlPanelCommittee, dropDownSource, string.Empty);

                dropDownSource = db.AcademicField.Where(f => f.Category == "Agency").OrderBy(f => f.DisplayOrder).ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlGrantAgency, dropDownSource, string.Empty);

                dropDownSource = db.AcademicField.Where(f => f.Category == "TypeOfTraining").OrderBy(f => f.DisplayOrder).ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlTrainingType, dropDownSource, string.Empty);

            }

            //var random = new Random();
            //int randomNum = random.Next(100, 999);
            //txtConfirmCode.Value = randomNum.ToString();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //int biostatBitSum = GetBiostatBitSum();

            int typeId = 0;
            Int32.TryParse(ddlAcademicType.SelectedValue, out typeId);
            int bitSum = 0;
            Int32.TryParse(txtBiostatBitSum.Value, out bitSum);


            Academic ae = GetAcademic(typeId);
            if (typeId > 0 && bitSum > 0)
            {
                //SaveAcademic(academicTpye, biostatBitSum);                

                Academic acad = SaveAcademic(ae);

                if (acad.Id > 0)
                {
                    Response.Write("<script>alert('Academic event is saved.');</script>");

                    BindControl(acad);
                                        
                    ddlAcademicType.SelectedValue = ae.AcademicTypeId.ToString();

                    ddlEvent.Items.Add(new ListItem(acad.Id + " " + acad.Title, acad.Id.ToString()));
                    ddlEvent.SelectedValue = ae.Id.ToString();

                    BindTable(rptBiostat, (int)ae.BiostatBitSum);
                }                
            }
            else
            {
                Response.Write("<script>alert('Academic type and staff are mandatory fields.');</script>");
            }
        }

        private Academic SaveAcademic(Academic ae)
        {
            Academic acad;

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                if (0 == ae.Id)
                {
                    db.Academic.Add(ae);
                    db.SaveChanges();

                    //ddlEvent.Items.Add(new ListItem(ae.Id + " " + ae.Title, ae.Id.ToString()));
                    //ddlEvent.SelectedValue = ae.Id.ToString();
                    //ddlEvent.Items.Insert(1, new ListItem(ae.Id + " " + ae.Title, ae.Id.ToString()));
                    //ddlEventHdn.Items.Insert(1, new ListItem(ae.Id + " " + ae.Title, ae.Id.ToString()));
                    //ddlEvent.SelectedIndex = 1;
                }
                else if (ae.Id > 0)
                {
                    var prevAcademic = db.Academic.FirstOrDefault(a => a.Id == ae.Id);
                    if (prevAcademic != null)
                    {
                        db.Entry(prevAcademic).CurrentValues.SetValues(ae);
                        db.SaveChanges();
                    }
                }
                
                acad = ae;
            }

            return acad;
        }

        private Academic GetAcademic(int typeId)
        {
            int id = 0,
                biostatBitSum = 0,
                numOfAttendees = 0,
                startSemesterId = 0,
                endSemesterId = 0,
                fieldId = 0,
                yearStart = DateTime.Now.Year,
                yearEnd = DateTime.Now.Year;

            DateTime startDate, endDate;

            Decimal numOfCredits = 0.0m;

            Int32.TryParse(ddlEvent.SelectedValue, out id);

            Academic a = new Academic() { 
                Id = id,
                AcademicTypeId = typeId,
                BiostatBitSum = Int32.TryParse(txtBiostatBitSum.Value, out biostatBitSum) ? biostatBitSum : 0,
                Title = "",
                Organization = "",
                Comments = txtComments.Value,
                Creator = User.Identity.Name,
                CreationDate = DateTime.Now
            };

            switch (typeId)
            {
                case 1:
                    a.Organization = txtOrganization.Value;
                    a.Title = txtTitle.Value;
                    a.StartDate = DateTime.TryParse(txtStartDate.Text, out startDate) ? startDate : (DateTime?)null;
                    a.EndDate = DateTime.TryParse(txtEndDate.Text, out endDate) ? endDate : (DateTime?)null;
                    a.NumOfAttendees = Int32.TryParse(txtNumOfAttendees.Value, out numOfAttendees) ? numOfAttendees : (int?)null;
                    a.CourseNum = txtCourseNum.Value;
                    break;
                case 2:
                    a.StartSemesterId = Int32.TryParse(ddlSemester.SelectedValue, out startSemesterId) ? startSemesterId : (int?)null;
                    //a.StartDate = DateTime.TryParse(txtYear.Value, out startDate) ? startDate : (DateTime?)null;
                    a.StartDate = Int32.TryParse(txtYear.Value, out yearStart) ? new DateTime(yearStart, 1, 1) : (DateTime?)null;
                    a.Title = txtCourseTitle.Value;
                    a.CourseNum = txtCourseNumTeaching.Value;
                    a.NumOfCredits = Decimal.TryParse(txtNumOfCredits.Value, out numOfCredits) ? numOfCredits : (Decimal?)null;
                    a.NumOfAttendees = Int32.TryParse(txtNumOfStudents.Value, out numOfAttendees) ? numOfAttendees : (int?)null;
                    break;
                case 3:
                    a.FieldId = Int32.TryParse(ddlCommitteeType.SelectedValue, out fieldId) ? fieldId : (int?)null;
                    a.Organization = txtDepartment.Value;
                    a.Name = txtStudentName.Value;
                    a.Advisor = txtAdvisorName.Value;
                    a.Title = txtProjectTitle.Value;
                    a.StartSemesterId = Int32.TryParse(ddlStartSemester.SelectedValue, out startSemesterId) ? startSemesterId : (int?)null;
                    a.EndSemesterId = Int32.TryParse(ddlEndSemester.SelectedValue, out endSemesterId) ? endSemesterId : (int?)null;
                    //a.StartDate = DateTime.TryParse(txtStartYear.Value, out startDate) ? startDate : (DateTime?)null;
                    //a.EndDate = DateTime.TryParse(txtEndYear.Value, out endDate) ? endDate : (DateTime?)null;
                    a.StartDate = Int32.TryParse(txtStartYear.Value, out yearStart) ? new DateTime(yearStart, 1, 1) : (DateTime?)null;
                    a.EndDate = Int32.TryParse(txtEndYear.Value, out yearEnd) ? new DateTime(yearEnd, 1, 1) : (DateTime?)null;
                    break;
                case 4:
                    a.Organization = txtPanelOrg.Value;
                    a.Title = txtPanelName.Value;
                    a.StartDate = DateTime.TryParse(txtPanelStartDate.Text, out startDate) ? startDate : (DateTime?)null;
                    a.EndDate = DateTime.TryParse(txtPanelEndDate.Text, out endDate) ? endDate : (DateTime?)null;
                    a.FieldId = Int32.TryParse(ddlPanelCommittee.SelectedValue, out fieldId) ? fieldId : (int?)null;
                    break;
                case 5:
                    a.Title = txtJournalName.Value;
                    a.StartDate = DateTime.TryParse(txtJournalDate.Text, out startDate) ? startDate : (DateTime?)null;                   
                    break;
                case 6:
                    a.FieldId = Int32.TryParse(ddlGrantAgency.SelectedValue, out fieldId) ? fieldId : (int?)null;
                    a.StartDate = DateTime.TryParse(txtGrantDate.Text, out startDate) ? startDate : (DateTime?)null;
                    break;
                case 7:
                    a.Organization = txtAwardAgency.Value;
                    a.Title = txtAwardTitle.Value;
                    a.StartDate = DateTime.TryParse(txtAwardDate.Text, out startDate) ? startDate : (DateTime?)null;
                    break;
                case 8:
                    a.Organization = txtTrainingProvider.Value;
                    a.FieldId = Int32.TryParse(ddlTrainingType.SelectedValue, out fieldId) ? fieldId : (int?)null;
                    a.Title = txtTrainingTitle.Value;
                    a.StartDate = DateTime.TryParse(txtTrainingDate.Text, out startDate) ? startDate : (DateTime?)null;
                    a.Location = txtTrainingloc.Value;
                    break;
                case 9:
                    a.Title = txtMentorTitle.Value;
                    a.Name = txtMenteeName.Value;
                    a.Organization = txtMenteeAffil.Value;
                    a.StartDate = DateTime.TryParse(txtMentorStartDate.Text, out startDate) ? startDate : (DateTime?)null;
                    a.EndDate = DateTime.TryParse(txtMentorEndDate.Text, out endDate) ? endDate : (DateTime?)null;
                    break;
                case 10:
                    a.Title = txtSafeyTitle.Value;
                    a.Name = txtPI.Value;
                    a.Organization = txtPIAffil.Value;
                    a.StartDate = DateTime.TryParse(txtSafetyStartDate.Text, out startDate) ? startDate : (DateTime?)null;
                    a.EndDate = DateTime.TryParse(txtSafetyEndDate.Text, out endDate) ? endDate : (DateTime?)null;
                    break;
                case 11:
                    a.Title = txtOtherTitle.Value;
                    a.AcademicDesc = txtOtherDesc.Value;
                    a.StartDate = DateTime.TryParse(txtOtherDate.Text, out startDate) ? startDate : (DateTime?)null;
                    break;
                case 12:
                    a.Name = txtStudentMentorStudentName.Value;
                    a.Organization = txtStudentMentorDepartment.Value;
                    a.Title = txtStudentMentorTitle.Value;
                    a.AcademicDesc = txtStudentMentorDesc.Value;
                    a.StartDate = DateTime.TryParse(txtStudentMentorStartDate.Text, out startDate) ? startDate : (DateTime?)null;
                    a.EndDate = DateTime.TryParse(txtStudentMentorEndDate.Text, out endDate) ? endDate : (DateTime?)null;
                    break;
                default:
                    break;
            }


            return a;
        }

        private void SaveAcademic(int academicTpyeId, int biostatBitSum)
        {
            Academic newAca = new Academic()
            {
                AcademicTypeId = academicTpyeId,
                BiostatBitSum = biostatBitSum,
                Title = "",
                Organization = "",
                Comments = txtComments.Value,
                Creator = Page.User.Identity.Name,
                CreationDate = DateTime.Now
            };

            DateTime dt1, dt2;
            int intOutput;
            switch(academicTpyeId)
            {
                case 1:                    
                    newAca.Organization = txtOrganization.Value;
                    newAca.Title = txtTitle.Value;
                    newAca.StartDate = DateTime.TryParse(txtStartDate.Text, out dt1) ? dt1 : (DateTime?)null;
                    newAca.EndDate = DateTime.TryParse(txtEndDate.Text, out dt2) ? dt2 : (DateTime?)null;
                    newAca.NumOfAttendees = Int32.TryParse(txtNumOfAttendees.Value, out intOutput) ? intOutput : (Int32?)null;
                    newAca.CourseNum = txtCourseNum.Value;
                    break;
            }

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                db.Academic.Add(newAca);
                db.SaveChanges();
            }
        }

        private void BindTable(Repeater rpt, int bitSum)
        {
            foreach (RepeaterItem i in rpt.Items)
            {
                CheckBox cb, cb1, cb2;
                HiddenField hdnBitValue, hdnBitValue1, hdnBitValue2;

                cb = (CheckBox)i.FindControl("chkId");
                hdnBitValue = (HiddenField)i.FindControl("BitValue");

                if (cb != null && hdnBitValue != null)
                {
                    cb.Checked = bitSum > 0 ? CheckBitValue(bitSum, hdnBitValue) : false;
                }

                if (cb == null)
                {
                    cb1 = (CheckBox)i.FindControl("FirstchkId");
                    cb2 = (CheckBox)i.FindControl("SecondchkId");

                    hdnBitValue1 = (HiddenField)i.FindControl("FirstBitValue");
                    hdnBitValue2 = (HiddenField)i.FindControl("SecondBitValue");

                    if (cb1 != null && hdnBitValue1 != null)
                    {
                        cb1.Checked = bitSum > 0 ? CheckBitValue(bitSum, hdnBitValue1) : false;
                    }

                    if (cb2 != null && hdnBitValue2 != null)
                    {
                        cb2.Checked = bitSum > 0 ? CheckBitValue(bitSum, hdnBitValue2) : false;
                    }
                }
            }
        }

        private void BindTable2(Dictionary<int, string> collection, Repeater rpt)
        {
            DataTable dt = new DataTable("tblRpt");

            dt.Columns.Add("Id1", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Name1", System.Type.GetType("System.String"));
            dt.Columns.Add("BitValue1", System.Type.GetType("System.Int32"));

            dt.Columns.Add("Id2", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Name2", System.Type.GetType("System.String"));
            dt.Columns.Add("BitValue2", System.Type.GetType("System.Int32"));

            var query = collection.ToArray();

            for (int i = 0; i < query.Length; i += 2)
            {
                DataRow dr = dt.NewRow();

                dr[0] = query[i].Key;
                dr[1] = query[i].Value;
                dr[2] = query[i].Key;

                if (i < query.Length - 1)
                {
                    dr[3] = query[i + 1].Key;
                    dr[4] = query[i + 1].Value;
                    dr[5] = query[i + 1].Key;
                }
                else
                {
                    dr[3] = 0;
                    dr[4] = "";
                    dr[5] = 0;
                }

                dt.Rows.Add(dr);
            }

            rpt.DataSource = dt;
            rpt.DataBind();
        }

        private bool CheckBitValue(int bitSum, HiddenField hdnBitValue)
        {
            int bitValue = 0;
            Int32.TryParse(hdnBitValue.Value, out bitValue);

            int c = bitSum & bitValue;

            return c == bitValue;
        }

        //private int GetBiostatBitSum()
        //{
        //    Int32 biostatSum = 0;

        //    //then add biostats
        //    foreach (RepeaterItem i in rptBiostat.Items)
        //    {
        //        CheckBox cb1 = (CheckBox)i.FindControl("chkId1");
        //        CheckBox cb2 = (CheckBox)i.FindControl("chkId2");

        //        int biostatId = 0;

        //        if (cb1 != null && cb1.Checked)
        //        {
        //            HiddenField hiddenId1 = (HiddenField)i.FindControl("Id1");

        //            //Response.Write(hiddenId1.Value + "; ");

        //            Int32.TryParse(hiddenId1.Value, out biostatId);

        //            biostatSum += (int)Math.Pow(2, biostatId);
        //        }

        //        if (cb2 != null && cb2.Checked)
        //        {
        //            HiddenField hiddenId2 = (HiddenField)i.FindControl("Id2");

        //            //Response.Write(hiddenId2.Value + "; ");

        //            Int32.TryParse(hiddenId2.Value, out biostatId);

        //            biostatSum += (int)Math.Pow(2, biostatId);
        //        }
        //    }

        //    //Response.Write(biostatSum);
        //    return biostatSum;
        //}

    }
}