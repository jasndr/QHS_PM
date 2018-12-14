using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System.Text.RegularExpressions;
using ProjectManagement.Model;
using System.Data;
using System.Text;
using System.Drawing;
using System.Threading;

namespace ProjectManagement
{
    /// <summary>
    /// @File: ImportRequest.aspx.cs
    /// @Front End : ImportRequest.aspx
    /// @Author: Jason Delos Reyes
    /// @Summary: Given the RMATRIX or Ola Hawaii Request form sent to the Biostatistics Core,
    ///           the program scans PDF of the file and creates a new PI and Project entry.
    ///           User is prompted to confirm the information to be uploaded before
    ///           importing into the database, which triggers two emails to be sent
    ///           to the tracking team: a PI and a Project creation email. 
    ///           
    ///           *** Reference to PDF Reader Code (https://www.codeproject.com/Articles/12445/Converting-PDF-to-Text-in-C)
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018OCT05 - Jason Delos Reyes  -  Initial file creation with file upload form on the user interface.
    ///  2018OCT08 - Jason Delos Reyes  -  Able to configure reading PDF file (has to be text-readable, which
    ///                                    the RMATRIX and OLA Hawaii Request forms already are).  Need to 
    ///                                    be able to partition individually, then post to database after
    ///                                    confirmation from user that uploads.
    ///  2018OCT22 - Jason Delos Reyes  -  Added ability to to upload form and post into PI / Project form.
    ///                                    Will need to add "post to database" functionality, and revisit
    ///                                    possibly adding *checked* to read as well.
    /// </summary>
    public partial class ImportRequest : System.Web.UI.Page
    {
        /// <summary>
        /// Activates file upload feature upon opening the page.  Upon postback, also
        /// redirects user to the Project Form of the created file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Initialize form
                BindControl();

            }
            else
            {
                //Reopens modal after page postback.
                ClientScript.RegisterStartupScript(GetType(), "ModalScript", "$('#editModal').modal('show').fadeIn(300);", true);
            }
        }

        /// <summary>
        /// When the "Import" button is selected, the following steps are in place:
        ///     1) The program scans the PDF and obtains the following text 
        ///         [a] (RMATRIX-II Form):
        ///             - PI
        ///             X [Filled Out Previously] - to be used to determine whether or not new PI form will need to be created
        ///             = Last Name
        ///             = First Name
        ///             X Degree
        ///             X Other Degree
        ///             X Investigator Status
        ///             = Email
        ///             = Phone number
        ///             X Affiliation 
        ///                 --> UH at Manoa >> Go to "College/Center"
        ///                 --> UH Cancer Center >> Check Department > "UH Cancer Center"
        ///                 --> UH at Hilo >> "College" Dropdown "UH School/College/Hilo..." > Check "UH at Hilo"
        ///                 --> UH West Oahu >> "College" Dropdown "UH School/College/Hilo..." > Check "University of Hawaii West Oahu"
        ///                 --> UH Cmty College >> "College" Dropdown "UH Community College" 
        ///                                         > Check Community College with matched category called "Community College"
        ///                 --> Local Comunity Partners >> "College" Dropdown "Community Parner" 
        ///                                                 > Check matched category "Community Partners"
        ///                 --> Hospitals >> "Non-UH Hawaii Client" > Check matched hospital in category "Hospitals (On Oahu)"
        ///                 --> Non-UH >> "Non-UH Hawaii Client" 
        ///                                > Check "Other", then fill "Other" dropdown 
        ///                                  with matched category "Name University or College (Not UH System)"
        ///                 --> Other Institution >> (same as "Non-UH")
        ///             *= College/Center (No Match with form; *do not collect)
        ///             X School/Office/Department
        ///                 --> If JABSOM selected, match Department with "JABSOM Department"
        ///                 --> Otherwise, if Office/School/Dept. Match, match Office with office/school selected
        ///             X JABSOM Department (match department as specified in "School/Office/Department" above)
        ///         - Project
        ///             = Project Title
        ///             = Project Summary
        ///             X Ethnic group -->> Link with "Study Population"
        ///             X Project Funding / Grants --> Link with "Funding Source"
        ///         [b] (Ola Hawaii Form):
        ///         - PI
        ///             X [Filled Out Previously] - to be used to determine whether or not new PI form will need to be created
        ///             = Last Name
        ///             = First Name
        ///             X Degree
        ///             X Other Degree
        ///             = Email
        ///             = Phone number
        ///             X Affiliation 
        ///                 --> UH at Manoa >> Go to "College/Center"
        ///                 --> UH Cancer Center >> Check Department > "UH Cancer Center"
        ///                 --> UH at Hilo >> "College" Dropdown "UH School/College/Hilo..." > Check "UH at Hilo"
        ///                 --> UH West Oahu >> "College" Dropdown "UH School/College/Hilo..." > Check "University of Hawaii West Oahu"
        ///                 --> UH Cmty College >> "College" Dropdown "UH Community College" 
        ///                                         > Check Community College with matched category called "Community College"
        ///                 --> Local Comunity Partners >> "College" Dropdown "Community Parner" 
        ///                                                 > Check matched category "Community Partners"
        ///                 --> Hospitals >> "Non-UH Hawaii Client" > Check matched hospital in category "Hospitals (On Oahu)"
        ///                 --> Non-UH >> "Non-UH Hawaii Client" 
        ///                                > Check "Other", then fill "Other" dropdown 
        ///                                  with matched category "Name University or College (Not UH System)"
        ///                 --> Other Institution >> (same as "Non-UH")
        ///             *= College/Center (No Match with form; *do not collect)
        ///             X School/Office/Department
        ///                 --> If JABSOM selected, match Department with "JABSOM Department"
        ///                 --> Otherwise, if Office/School/Dept. Match, match Office with office/school selected
        ///             X JABSOM Department (match department as specified in "School/Office/Department" above)
        ///         - Project
        ///             = Project Title
        ///             = Project Summary
        ///             X Ethnic group -->> Link with "Study Population"
        ///             X Project Funding / Grants --> Link with "Funding Source"
        ///     2) Modal with a pop-up appears that will match the information collected (and will be inputted into PI and
        ///        project forms.).  User has the option to edit the forms before submitting.
        ///     3) When the user clicks "Save", a new PI and Project Record will have been recorded, and appropriate emails
        ///        will be sent to the tracking team for approval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            /// Obtain the PDF from the upload field.
            string strFilename = "";
            string filePathName = "";
            byte[] myData = new byte[0];
            if (fileUpload.PostedFile != null)
            {
                // Get a reference to PostedFile object.
                HttpPostedFile myFile = fileUpload.PostedFile;

                // Get size of uploaded file.
                int nFileLen = myFile.ContentLength;

                // Make sure the size of the file is > 0.
                if (nFileLen > 0)
                {
                    // Allocate a buffer for reading of the file.
                    myData = new byte[nFileLen];

                    // Read uploaded file from the Stream.
                    myFile.InputStream.Read(myData, 0, nFileLen);

                    // Create name for the file to store.
                    strFilename = System.IO.Path.GetFileName(myFile.FileName);

                    // Creates "UploadFiles" folder if does not exist yet.
                    if (!(Directory.Exists(Path.GetTempPath()/*System.IO.Directory.Exists(Server.MapPath("~/UploadFiles/")*/)))
                        System.IO.Directory.CreateDirectory(Path.GetTempPath()/*Server.MapPath("~/UploadFiles/")*/);

                    //if (!(Directory.Exists(Path.GetTempFileName())))
                    //    Directory.CreateDirectory(Path.GetTempFileName());


                    //Write data into a file
                    WriteToFile(Path.GetTempPath() + strFilename/*Server.MapPath("~/UploadFiles/" + strFilename*/, ref myData);

                    filePathName = Path.GetTempPath() + strFilename/*Server.MapPath("~/UploadFiles/" + strFilename)*/;//myFile.FileName; // Equivalent to --> System.IO.Path.GetFullPath(myFile.FileName);
                                                                                                                      // Not working --> Server.MapPath(myFile.FileName);


                    // Read file into string sections*.
                    //string fileData = ExtractTextFromPdf(filePathName);
                    Dictionary<string, string> data = ExtractTextFromPdf(filePathName);

                    //Open modal, displaying PI/Project forms before saving into database.

                    //--- Place into stored data into webform
                    PlaceToWebForm(data);


                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //sb.Append(@"<script type='text/javascript'>");
                    //sb.Append("$('#editModal').modal('show');");
                    //sb.Append(@"</script>");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);

                }
            }
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport2_Click(object sender, EventArgs e)
        {
            

            /// Obtain the PDF from the upload field.
            string strFilename = "";
            string filePathName = "";
            byte[] myData = new byte[0];
            if (fileUpload.PostedFile != null)
            {
                // Get a reference to PostedFile object.
                HttpPostedFile myFile = fileUpload.PostedFile;

                // Get size of uploaded file.
                int nFileLen = myFile.ContentLength;

                // Make sure the size of the file is > 0.
                if (nFileLen > 0)
                {
                    // Allocate a buffer for reading of the file.
                    myData = new byte[nFileLen];

                    // Read uploaded file from the Stream.
                    myFile.InputStream.Read(myData, 0, nFileLen);

                    // Create name for the file to store.
                    strFilename = System.IO.Path.GetFileName(myFile.FileName);

                    // Creates "UploadFiles" folder if does not exist yet.
                    if (!(Directory.Exists(Path.GetTempPath()/*System.IO.Directory.Exists(Server.MapPath("~/UploadFiles/")*/)))
                        System.IO.Directory.CreateDirectory(Path.GetTempPath()/*Server.MapPath("~/UploadFiles/")*/);

                    //if (!(Directory.Exists(Path.GetTempFileName())))
                    //    Directory.CreateDirectory(Path.GetTempFileName());


                    //Write data into a file
                    WriteToFile(Path.GetTempPath() + strFilename/*Server.MapPath("~/UploadFiles/" + strFilename*/, ref myData);

                    filePathName = Path.GetTempPath() + strFilename/*Server.MapPath("~/UploadFiles/" + strFilename)*/;//myFile.FileName; // Equivalent to --> System.IO.Path.GetFullPath(myFile.FileName);
                                                                                                                      // Not working --> Server.MapPath(myFile.FileName);


                    // Read file into string sections*.
                    //string fileData = ExtractTextFromPdf(filePathName);
                   // Dictionary<string, string> data = ExtractTextFromPdf(filePathName);


               //     Bitmap image = new Bitmap(filePathName);
               //     tessnet2.Tesseract ocr = new tessnet2.Tesseract();
               ////     ocr.SetVariable("tessedit_char_whitelist", "0123456789"); // If digit only
               //     //ocr.Init(@"c:\temp", "fra", false); // To use correct tessdata
               //     List<tessnet2.Word> result = ocr.DoOCR(image, Rectangle.Empty);
               //     foreach (tessnet2.Word word in result)
               //         Console.WriteLine("{0} : {1}", word.Confidence, word.Text);

                    //Open modal, displaying PI/Project forms before saving into database.

                    //--- Place into stored data into webform
                    //PlaceToWebForm(data);


                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //sb.Append(@"<script type='text/javascript'>");
                    //sb.Append("$('#editModal').modal('show');");
                    //sb.Append(@"</script>");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);

                }
            }


        }

        /// <summary>
        /// Extracts text from the PDF form being referred to.
        /// </summary>
        /// <param name="path">Location of PDF form.</param>
        /// <returns></returns>
        private static Dictionary<string, string> ExtractTextFromPdf(string path)
        {
            PDDocument doc = null;
            try
            {
                doc = PDDocument.load(path);
                PDFTextStripper stripper = new PDFTextStripper();

                string entirePdfText = stripper.getText(doc);
                var dict = new Dictionary<string, string>();


               //File

                //Obtain fields to enter into the database (only readable characters for now, looking
                //                                          into solution that reads checks in the future).
                var requestType = Regex.Match(entirePdfText, @"(?<=5).+?(?= Request for Resources)").ToString();
                dict.Add("requestType", requestType);

                var lastName = Regex.Match(entirePdfText, @"(?<=1a.\s+Last Name\s+).+(?=\r)").ToString();
                dict.Add("lastName", lastName);

                var firstName = Regex.Match(entirePdfText, @"(?<=1b.\s+First Name\s+).+(?=\r)").ToString();
                dict.Add("firstName", firstName);

                var email = Regex.Match(entirePdfText, @"(?<=5\.\s+Email:\s+).+(?=\r)").ToString();
                dict.Add("email", email);

                var phoneNumber = Regex.Match(entirePdfText, @"(?<=6.\s+Phone number:\s+).+(?=\r)").ToString();
                dict.Add("phoneNumber", phoneNumber);

                int ptFrom = entirePdfText.IndexOf("8a.  Project Title ") + "8a.  Project Title ".Length;
                int ptTo = entirePdfText.IndexOf("(If no current project please type ");
                var projectTitle = entirePdfText.Substring(ptFrom, ptTo - ptFrom);
                projectTitle.Replace(System.Environment.NewLine, " ");
                dict.Add("projectTitle", projectTitle);

                int psFrom = entirePdfText.IndexOf("8b.  Project Summary ") + "8b.  Project Summary ".Length;
                int psTo = entirePdfText.IndexOf("(If no current project type");
                var projectSummary = entirePdfText.Substring(psFrom, psTo - psFrom);
                projectSummary.Replace(System.Environment.NewLine, " ");
                dict.Add("projectSummary", projectSummary);

                return dict;//requestType; //entirePdfText;
                //return stripper.getText(doc); 
            }
            finally
            {
                if (doc != null)
                {
                    doc.close();
                }
            }
        }

        /// <summary>
        /// Obtains extracted pdf form and places them into the fields on the web
        /// form.
        /// </summary>
        /// <param name="data">Dictionary of obtain fields from PDF form.</param>
        private void PlaceToWebForm(Dictionary<string, string> data)
        {
            // Pre-populate PI Form if username already exists in the database.

            Invest currPI;
            ICollection<JabsomAffil> jabsomAffilList;
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                string piNameFromForm = "";

                if (!string.IsNullOrEmpty(data["firstName"]) && !string.IsNullOrEmpty(data["lastName"]))
                    piNameFromForm = Convert.ToString(data["firstName"]) + " " + Convert.ToString(data["lastName"]);

                currPI = db.Invests.Where(x => (x.FirstName + " " + x.LastName)
                                           .Contains(piNameFromForm)).FirstOrDefault();


                if (currPI != null)
                {
                    // PI Information - from request form  
                    TextBoxFirstName.Text = data.ContainsKey("firstName") ? data["firstName"] : currPI.FirstName;
                    TextBoxLastName.Text = data.ContainsKey("lastName") ? data["lastName"] : currPI.LastName;
                    TxtPI.Text = data["firstName"] + " " + data["lastName"]; TxtPI.ReadOnly = true;

                    TextBoxEmail.Text = data.ContainsKey("email") && data["email"] != "__________________________________" ? data["email"] : currPI.Email;
                    TextBoxPhone.Text = data.ContainsKey("phoneNumber") && data["phoneNumber"] != "__________________________________" ? data["phoneNumber"] : currPI.Phone;

                    //if (data.ContainsKey("firstName")) TextBoxFirstName.Text = data["firstName"];
                    //if (data.ContainsKey("lastName")) TextBoxLastName.Text = data["lastName"];
                    //if (data.ContainsKey("email")) TextBoxEmail.Text = data["email"];
                    //if (data.ContainsKey("phoneNumber")) TextBoxPhone.Text = data["phoneNumber"];

                    // PI Information - from database
                    chkPilot.Checked = currPI.IsPilot;
                    ddlStatus.SelectedIndex = currPI.InvestStatusId;
                    TextBoxNonHawaii.Text = currPI.NonHawaiiClient;

                    TextBox txtNonUHOther = GridViewNonUH.FooterRow.FindControl("txtNonUHOther") as TextBox;
                    if (currPI.NonUHClient != null)
                    {
                        txtNonUHOther.Text = currPI.NonUHClient;
                    }
                    else
                    {
                        txtNonUHOther.Text = string.Empty;
                    }

                    TextBox txtDegreeOther = GridViewDegree.FooterRow.FindControl("txtDegreeOther") as TextBox;
                    if (currPI.OtherDegree != null)
                    {
                        txtDegreeOther.Text = currPI.OtherDegree;
                    }
                    else
                    {
                        txtDegreeOther.Text = string.Empty;
                    }

                    TextBox txtCommunityPartnerOther = GridViewCommunityPartner.FooterRow.FindControl("txtCommunityPartnerOther") as TextBox;
                    if (currPI.OtherCommunityPartner != null)
                    {
                        txtCommunityPartnerOther.Text = currPI.OtherCommunityPartner;
                    }
                    else
                    {
                        txtCommunityPartnerOther.Text = string.Empty;
                    }


                    jabsomAffilList = currPI.JabsomAffils;
                    Bind_JabsomAffil(jabsomAffilList);


                }
                else
                {
                    // PI Information - from request form
                    if (data.ContainsKey("firstName")) TextBoxFirstName.Text = data["firstName"];

                    if (data.ContainsKey("lastName")) TextBoxLastName.Text = data["lastName"];
                    TxtPI.Text = data["firstName"] + " " + data["lastName"]; TxtPI.ReadOnly = true;

                    if (data.ContainsKey("email")) TextBoxEmail.Text = data["email"];

                    if (data.ContainsKey("phoneNumber")) TextBoxPhone.Text = data["phoneNumber"];
                }
            }


            // Project Information - from request form
            TxtProject.Text = "Add new project"; TxtProject.ReadOnly = true;

            if (data.ContainsKey("projectTitle")) TxtTitle.Text = data["projectTitle"].Replace(Environment.NewLine, " ");

            if (data.ContainsKey("projectSummary")) TxtSummary.Value = data["projectSummary"].Replace(Environment.NewLine, " ");


        }

        /// <summary>
        /// Populates PI affiliation grid with affiliations from the database
        /// onto the PI Form.
        /// </summary>
        /// <param name="jabsomAffilList">List of PI Affiliations</param>
        private void Bind_JabsomAffil(ICollection<JabsomAffil> jabsomAffilList)
        {
            List<int> idList = new List<int>();

            foreach (JabsomAffil jaf in jabsomAffilList)
            {
                idList.Add(jaf.Id);
            }

            // Check correct PI affiliations (mult. sections, e.g., Degree Checkbox List) 
            PageUtility.SelectRow_GridView(GridViewDept, idList);

            PageUtility.SelectRow_GridView(GridViewOffice, idList);

            PageUtility.SelectRow_GridView(GridViewNonUH, idList);

            PageUtility.SelectRow_GridView(GridViewDegree, idList);

            PageUtility.SelectRow_GridView(GridViewCommunityCollege, idList);

            PageUtility.SelectRow_GridView(GridViewCommunityPartner, idList);

            int selectedJabsomId = 0;

            GridViewUHDept.Style["display"] = "none";
            foreach (ListItem item in ddlJabsomOther.Items)
            {
                int index;
                bool result = Int32.TryParse(item.Value, out index);
                if (result)
                {
                    if (idList.Contains(index))
                    {
                        selectedJabsomId = index;

                        if (item.Text.Contains("UH School"))
                        {
                            GridViewUHDept.Style["display"] = "inherit";
                        }

                    }
                }
            }


            if (selectedJabsomId > 0)
            {
                ddlJabsomOther.SelectedValue = selectedJabsomId.ToString();
                PageUtility.SelectRow_GridView(GridViewUHDept, idList);
            }
            else
            {
                ddlJabsomOther.ClearSelection();
            }

            ddlUHFaculty.ClearSelection();
            if (ddlStatus.SelectedItem.Text == "UH Faculty")
            {
                foreach (ListItem item in ddlUHFaculty.Items)
                {
                    int index;
                    bool result = Int32.TryParse(item.Value, out index);
                    if (result)
                    {
                        if (idList.Contains(index))
                        {
                            ddlUHFaculty.SelectedValue = index.ToString();
                        }
                    }
                }
            }

        }


        /// <summary>
        /// Intializes from dropdown, checkmark choices, and file upload fields.
        /// </summary>
        private void BindControl()
        {
            //Enable file input 
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
            "FileUploadScript", "$('#MainContent_fileUpload').fileinput('enable');", true);

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                ///-- PI Section --///

                // "Status" Dropdown
                var statusQuery = db.InvestStatus
                                    .OrderBy(d => d.DisplayOrder);
                ddlStatus.DataSource = statusQuery.ToList();
                ddlStatus.DataValueField = "Id";
                ddlStatus.DataTextField = "StatusValue";
                ddlStatus.DataBind();

                // "UH Faculty" Dropdown
                var uhFacultyQuery = db.JabsomAffils
                                     .Where(d => d.Type == "UHFaculty")
                                     .OrderBy(d => d.Id);
                ddlUHFaculty.DataSource = uhFacultyQuery.ToList();
                ddlUHFaculty.DataValueField = "Id";
                ddlUHFaculty.DataTextField = "Name";
                ddlUHFaculty.DataBind();
                ddlUHFaculty.Items.Insert(0, new ListItem(String.Empty, String.Empty));

                // "Degree" Checkbox List
                var queryDegree = db.JabsomAffils
                                    .Where(d => d.Type == "Degree");
                GridViewDegree.DataSource = queryDegree.ToList();
                GridViewDegree.DataBind();

                // [Affiliations -> Department] Checkbox List
                var queryDept = db.JabsomAffils
                                  .Where(d => d.Type == "Department");
                GridViewDept.DataSource = queryDept.ToList();
                GridViewDept.DataBind();

                // [Affiliations -> Office] Checkbox List
                var queryOffice = db.JabsomAffils
                                    .Where(d => d.Type == "Office");
                GridViewOffice.DataSource = queryOffice.ToList();
                GridViewOffice.DataBind();

                // "College" Dropdown
                dropDownSource = db.JabsomAffils
                                   .Where(d => d.Type == "College")
                                   .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlJabsomOther, dropDownSource, string.Empty);

                // [College -> UH School/College/Hilo/West Oahu/Other Programs] Checkbox List
                var queryUHDept = db.JabsomAffils
                                    .Where(d => d.Type == "UHSchool");
                GridViewUHDept.DataSource = queryUHDept.ToList();
                GridViewUHDept.DataBind();

                // [College -> UH Community College] Checkbox List
                var queryCommunityCollege = db.JabsomAffils
                                    .Where(d => d.Type == "CommunityCollege");
                GridViewCommunityCollege.DataSource = queryCommunityCollege.ToList();
                GridViewCommunityCollege.DataBind();

                // [College -> Community Partner] Checkbox List
                var queryCommunityPartner = db.JabsomAffils
                                              .Where(d => d.Type == "CommunityPartner");
                GridViewCommunityPartner.DataSource = queryCommunityPartner.ToList();
                GridViewCommunityPartner.DataBind();

                // "Non-UH Hawaii Client" Checkbox List
                var queryNonUH = db.JabsomAffils
                                   .Where(d => d.Type == "MajorHospital");
                GridViewNonUH.DataSource = queryNonUH.ToList();
                GridViewNonUH.DataBind();

                ///-- Project Section --///

                var dropDownSource2 = new Dictionary<int, string>();

                // Populates Lead member dropdown (current, active QHS faculty/staff and not 'N/A' marker).
                var query = db.BioStats
                              .Where(b => b.EndDate >= DateTime.Now);

                dropDownSource2 = query
                                    .Where(b => b.Id > 0 && b.Name != "N/A")
                                    .OrderBy(b => b.Name)
                                    .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlLeadBiostat, dropDownSource2, String.Empty);

                // Populates dropdown of checkbox grid of other members.
                dropDownSource2 = query
                                    .Where(b => b.Id > 0)
                                    .OrderBy(b => b.Id == 99 ? 2 : 1)
                                    .ToDictionary(c => (int)c.BitValue, c => c.Name);

                BindTable2(dropDownSource2, rptBiostat);

                // Populates "Study Area" checkbox grid.
                var qProjectField = db.ProjectField.Where(f => f.IsStudyArea == true).ToList();

                rptStudyArea.DataSource = qProjectField;
                rptStudyArea.DataBind();

                // Populates "Health Data" checkbox grid.
                qProjectField = db.ProjectField.Where(f => f.IsHealthData == true).ToList();
                rptHealthData.DataSource = qProjectField;
                rptHealthData.DataBind();

                // Populates "Study Type" checkbox grid.
                qProjectField = db.ProjectField.Where(f => f.IsStudyType == true).ToList();
                rptStudyType.DataSource = qProjectField;
                rptStudyType.DataBind();

                // Populates "Study Population" checkbox grid.
                qProjectField = db.ProjectField.Where(f => f.IsStudyPopulation == true).ToList();
                rptStudyPopulation.DataSource = qProjectField;
                rptStudyPopulation.DataBind();

                // Populates "Service" checkbox grid.
                qProjectField = db.ProjectField.Where(f => f.IsService == true).ToList();
                rptService.DataSource = qProjectField;
                rptService.DataBind();

                // Creates the default phase, since the Project # -1 is a pseudo-project for initialiation.
                BindPhaseByProject(-1);

                // Populates "Grant" (Changed to "Funding Source") checkbox grid.
                dropDownSource2 = db.ProjectField
                                   .Where(f => f.IsGrant == true && f.IsFundingSource == true)
                                   .OrderBy(b => b.Id)
                                   .ToDictionary(c => c.BitValue, c => c.Name);

                BindTable2(dropDownSource2, rptGrant);

                // Populates "Acknowledgements" checkbox grid.
                dropDownSource2 = db.ProjectField
                                   .Where(f => f.IsGrant == true && f.IsAcknowledgment == true)
                                   .OrderBy(b => b.Id)
                                   .ToDictionary(c => c.BitValue, c => c.Name);

                BindTable2(dropDownSource2, rptAkn);

                // Populates Funding Source > Department Funding dropdown. 
                dropDownSource2 = db.JabsomAffils
                                   .Where(f => f.Name == "Obstetrics, Gynecology, and Women's Health"
                                           || f.Name == "School of Nursing & Dental Hygiene"
                                           || f.Id == 96 /*Other*/)
                                   .OrderBy(b => b.Id)
                                   .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlDepartmentFunding, dropDownSource2, String.Empty);
                PageUtility.BindDropDownList(ddlAknDepartmentFunding, dropDownSource2, String.Empty);



            }

        }

        ///// <summary>
        ///// NOTE - CURRENTLY NOT IN USE
        ///// Given the a search parameter for a given string,
        ///// returns the index of the "second occurence" of the given search 
        ///// parameter.
        ///// </summary>
        ///// <param name="theString">Entire string being searched.</param>
        ///// <param name="toFind">String that needs to be found.</param>
        ///// <returns>Index of the second occurence of the string "toFind".</returns>
        //private static int IndexOfSecond(string theString, string toFind)
        //{
        //    int first = theString.LastIndexOf(toFind);
        //    if (first == -1) return -1;

        //    // Find the "next" occurence by starting just past the first
        //    int second = theString.IndexOf(toFind, first + toFind.Length + 1);
        //    return second;
        //}

        /// <summary>
        /// Temporarily uploads file from upload into server location.
        /// </summary>
        /// <param name="strPath">Location of where to save the file.</param>
        /// <param name="Buffer">The file data.</param>
        private void WriteToFile(string strPath, ref byte[] Buffer)
        {
            // Create a file
            FileStream newFile = new FileStream(strPath, FileMode.Create);



            // Write data to the file
            newFile.Write(Buffer, 0, Buffer.Length);

            // Close file
            newFile.Close();
        }

        /// <summary>
        /// Binds grid of checkboxes with choices for selected areas (faculty/staff, etc).
        /// </summary>
        /// <param name="collection">List of choices for given selected area.</param>
        /// <param name="rpt">Grid for selected area.</param>
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

        /// <summary>
        /// Binds the phase information based on the current project for the project form.
        /// </summary>
        /// <param name="projectId">Referenced Project ID.</param>
        private void BindPhaseByProject(int projectId)
        {
            DataTable dt = CreatePhaseTable(projectId);

            gvPhase.DataSource = dt;
            gvPhase.DataBind();
        }


        /// <summary>
        /// If there is a referenced project, sets up a new phase grid/table with the default Phase-0 
        /// consultation hours of 1 MS and 1 PhD hours.
        /// </summary>
        /// <param name="projectId">Referenced project ID.</param>
        /// <returns>New table with phase ID, Phase Name, Title, Estimated MS Hours, Estimated 
        ///          PhD Hours, Start Date, Completion Date, and corresponding Agreement ID.</returns>
        private DataTable CreatePhaseTable(int projectId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Title");
            dt.Columns.Add("MsHrs");
            dt.Columns.Add("PhdHrs");
            dt.Columns.Add("StartDate");
            dt.Columns.Add("CompletionDate");
            dt.Columns.Add("AgmtId");

            if (projectId != 0)
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    var phases = db.ProjectPhase
                                   .Where(p => p.ProjectId == projectId && p.IsDeleted == false)
                                   //.OrderBy(p=>p.Name.Substring(p.Name.IndexOf("-")+1, p.Name.Length))
                                   .OrderBy(p => p.Name.Length).ThenBy(p => p.Name)
                                   .ToList();

                    if (phases == null || phases.Count == 0)
                    {
                        phases = new List<ProjectPhase>();
                        var newPhase = new ProjectPhase()
                        {
                            Name = "Phase-0",
                            Title = "Consultation",
                            MsHrs = 1.0m,
                            PhdHrs = 1.0m
                        };

                        phases.Add(newPhase);
                    }

                    foreach (var phase in phases)
                    {
                        var agmt = db.ClientAgmt.FirstOrDefault(a => a.Project2Id == projectId && a.ProjectPhase == phase.Name);

                        DataRow dr = dt.NewRow();
                        dr[0] = phase.Id;
                        dr[1] = phase.Name;
                        dr[2] = phase.Title;
                        dr[3] = phase.MsHrs;
                        dr[4] = phase.PhdHrs;
                        dr[5] = phase.StartDate != null ? Convert.ToDateTime(phase.StartDate).ToShortDateString() : "";
                        dr[6] = phase.CompletionDate != null ? Convert.ToDateTime(phase.CompletionDate).ToShortDateString() : "";
                        dr[7] = agmt != null ? agmt.AgmtId : "";

                        //decimal dMsOut = 0.0m,
                        //        dPhdOut = 0.0m;
                        //if (phase.ProjectId > 0)
                        //{
                        //    //total spent hours                        
                        //    DateTime startDate = new DateTime(2000, 1, 1), endDate = new DateTime(2099, 1, 1);
                        //    //ObjectParameter startDate = new ObjectParameter("StartDate", typeof(DateTime?));
                        //    //ObjectParameter endDate = new ObjectParameter("EndDate", typeof(DateTime?));
                        //    ObjectParameter phdHours = new ObjectParameter("PhdHours", typeof(decimal));
                        //    ObjectParameter msHours = new ObjectParameter("MSHours", typeof(decimal));
                        //    var i = db.P_PROJECTPHASE_HOURS(phase.ProjectId, phase.Name, startDate, endDate, phdHours, msHours);
                        //    db.SaveChanges();

                        //    Decimal.TryParse(phdHours.Value.ToString(), out dPhdOut);
                        //    Decimal.TryParse(msHours.Value.ToString(), out dMsOut);
                        //}

                        //dr[8] = dMsOut;
                        //dr[9] = dPhdOut;

                        dt.Rows.Add(dr);
                    }

                }
            }

            return dt;
        }

        /// <summary>
        /// Initializes a new row for the phase grid/table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddPhase_Click(object sender, EventArgs e)
        {
            BindPhaseByIndex(-1);

            ClientScript.GetPostBackEventReference(upPhase, "");
        }

        /// <summary>
        /// Obtains the written phase information on the form and adds them to the database.
        /// </summary>
        /// <param name="rowIndex">Referenced row index of Phase table/grid.</param>
        private void BindPhaseByIndex(int rowIndex)
        {
            DataTable dt = CreatePhaseTable(0);
            DataRow dr;

            int lastPhase = 0;
            foreach (GridViewRow row in gvPhase.Rows)
            {
                if (row.RowIndex != rowIndex)
                {
                    Label lblId = row.FindControl("lblId") as Label;
                    Label lblPhase = row.FindControl("lblPhase") as Label;
                    TextBox txtStartDate = row.FindControl("txtStartDate") as TextBox;
                    TextBox txtCompletionDate = row.FindControl("txtCompletionDate") as TextBox;
                    TextBox txtTitle = row.FindControl("txtTitle") as TextBox;
                    TextBox txtMsHrs = row.FindControl("txtMsHrs") as TextBox;
                    TextBox txtPhdHrs = row.FindControl("txtPhdHrs") as TextBox;
                    Label agmtId = row.FindControl("lblAgmtId") as Label;

                    dr = dt.NewRow();
                    dr["Id"] = lblId.Text;
                    dr["Name"] = lblPhase.Text;
                    dr["Title"] = txtTitle.Text;
                    dr["MsHrs"] = txtMsHrs.Text;
                    dr["PhdHrs"] = txtPhdHrs.Text;
                    dr["StartDate"] = txtStartDate.Text;
                    dr["CompletionDate"] = txtCompletionDate.Text;
                    dr["AgmtId"] = agmtId.Text;

                    dt.Rows.Add(dr);

                    string[] sPhase = lblPhase.Text.Split('-');
                    Int32.TryParse(sPhase[1], out lastPhase);
                }
            }

            if (rowIndex < 0 || dt.Rows.Count == 0)
            {
                lastPhase += 1;
                dr = dt.NewRow();
                dr[1] = "Phase-" + lastPhase;
                dt.Rows.Add(dr);
            }

            gvPhase.DataSource = dt;
            gvPhase.DataBind();

            //rptPhaseCompletion.DataSource = dt;
            //rptPhaseCompletion.DataBind();
        }


        /// <summary>
        /// Auto-populates Phase and Agreement sections of the phase grid/table with data specific
        /// to the referred data row. Creates delete button if there is not an attached 
        /// agreement to a phase yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPhase_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAgmtId = e.Row.FindControl("lblAgmtId") as Label;
                Label lblPhase = e.Row.FindControl("lblPhase") as Label;

                if (lblPhase != null && lblAgmtId != null)
                {
                    if (lblPhase.Text == "Phase-0" || lblAgmtId.Text != string.Empty)
                    {
                        LinkButton lb = e.Row.FindControl("lnkDelete") as LinkButton;
                        if (lb != null)
                            lb.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Fundamentally deletes row being referred to by creating a new instance of the grid,
        /// copying all the other rows (phase 0 can't be deleted), and replacing the information
        /// from the new grid into the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPhase_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvPhase.Rows[e.RowIndex];

            if (row != null)
            {
                BindPhaseByIndex(e.RowIndex);
            }
        }


        ///////////////////////////////////
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // Post PI and Project Forms into database.
        }


    }
}