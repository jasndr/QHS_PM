using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement
{
    public partial class Publication1 : System.Web.UI.Page
    {
        static DateTime _endDate = DateTime.Parse("2099-01-01");
        static string _currentDate = "2015-01-01";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();
                //DisableOptions();
            }

            if (!HiddenFieldCurrentDate.Value.Equals(string.Empty))
            {
                _currentDate = HiddenFieldCurrentDate.Value;
            }
        }

        //protected string GetAbstractChecked(object hiddenVariable)
        //{
        //    return hiddenVariable.Equals("Abstract") ? "checked='checked'" : string.Empty;
        //}

        //protected string GetManuscriptChecked(object hiddenVariable)
        //{
        //    return hiddenVariable.Equals("Manuscript") ? "checked='checked'" : string.Empty;
        //}

        //if (Request.Form["pubRadios"] != null)
        //{
        //    selectedPubType = Request.Form["pubRadios"].ToString();
        //}

        protected void ButtonUpload_Click(object sender, EventArgs e)
        {
            if (inputFile.PostedFile != null)
            {
                // Get a reference to PostedFile object
                HttpPostedFile file = inputFile.PostedFile;
                // Create a name for the file to store                

                string fileName = Path.GetFileName(file.FileName);

                string pathToCreate = "~/UploadFiles/"+ddlPublication.SelectedValue + "/";
                if (!Directory.Exists(Server.MapPath(pathToCreate)))
                {
                    Directory.CreateDirectory(Server.MapPath(pathToCreate));
                }

                string filePath = Server.MapPath(pathToCreate + fileName);
                inputFile.PostedFile.SaveAs(filePath);
            }

            BindGridViewFile();
            ButtonUpload.Focus();
        }

        private void BindGridViewFile()
        {
            string pathToCreate = "~/UploadFiles/" + ddlPublication.SelectedValue + "/";
            if (!Directory.Exists(Server.MapPath(pathToCreate)))
            {
                Directory.CreateDirectory(Server.MapPath(pathToCreate));
            }

            string[] filePaths = Directory.GetFiles(Server.MapPath(pathToCreate));
            List<ListItem> files = new List<ListItem>();
            foreach (string filePath in filePaths)
            {
                files.Add(new ListItem(Path.GetFileName(filePath), filePath));
            }

            GridViewFile.DataSource = files;
            GridViewFile.DataBind();

            //if (!ddlPublication.SelectedValue.Equals(string.Empty))
            //{
            //    string filePath = Server.MapPath("~/UploadFiles/"); // + ddlPublication.SelectedValue);

            //    string[] filePaths = Directory.GetFiles(filePath);
            //    List<ListItem> files = new List<ListItem>();

            //    foreach (string _filePath in filePaths)
            //    {
            //        files.Add(new ListItem(Path.GetFileName(_filePath), filePath));
            //    }
            //    GridViewFile.DataSource = files;
            //    GridViewFile.DataBind();
            //}
            
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }

        protected void DeleteFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            File.Delete(filePath);
            //Response.Redirect(Request.Url.AbsoluteUri);
            BindGridViewFile();
            btnSubmit.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int pubId = 0,
                subPubId = 0;

            Int32.TryParse(ddlPublication.SelectedValue, out pubId);
            //Int32.TryParse(ddlChildPublication.SelectedValue, out subPubId);                     
                     
            if (SavePublication(pubId, subPubId))
            {
                Response.Write("<script>alert('Publication has been saved.');</script>");
                ClearForm();
            }
            else
            {
                Response.Write("<script>alert('Publication exists.');</script>");
            }          
               
        }

        private bool SavePublication(int pubId, int subPubId)
        {
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                if (pubId > 0)
                {
                    Publication pub = context.Publications.First(p => p.Id == pubId);

                    if (pub != null)
                    {
                        //if (subPubId > 0)
                        //{
                        //    //update sub pub
                        //    Publication subPub = context.Publications.First(p => p.Id == subPubId);
                        //    bool isSub = true;
                        //    return UpdatePublication(context, subPub, isSub);
                        //}
                        //else
                        //{
                        //    if (PubStatus == (int)StatusEnum.SubResub)
                        //    {
                        //        if (Convert.ToDateTime(TextBoxSubmitDate.Text) > pub.SubmitDate) //resubmit
                        //        {
                        //            pub.EndDate = Convert.ToDateTime(TextBoxSubmitDate.Text);   //expire current pub
                        //            return CreateNewPublication(context, pub);
                        //        }
                        //        else
                        //        {
                        //            return UpdatePublication(context, pub, false);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        return CreateNewPublication(context, pub);
                        //    }
                        //}
                        
                        return UpdatePublication(context, pub, false);
                        
                    }

                }
                else
                {
                    if (PubStatus != (int)StatusEnum.SubResub)
                    {
                        lblDebugMsg.Text = "Can't insert a new publication without submit status.";
                    }
                    else
                    {                        
                        return CreateNewPublication(context, null);
                    }
                }

                context.SaveChanges();

                return true;
            }
        }        

        private bool CreateNewPublication(ProjectTrackerContainer context, Publication pub)
        {
            Publication newPub = new Publication();

            //if (IsAbstract)
            //{
            //    newPub = new PubAbstract();
            //}
            //else
            //{
            //    newPub = new PubManuscript();                
            //}
                        
            newPub.Status = PubStatus;       
            newPub.StartDate = Convert.ToDateTime(HiddenFieldCurrentDate.Value);
            newPub.EndDate = Convert.ToDateTime("2099-01-01");
              

            DateTime pubDate = Convert.ToDateTime(TextBoxSubmitDate.Text);
            newPub.SubmitDate = pubDate;

            //switch (newPub.Status)
            //{
            //    case (int)StatusEnum.Accepted:
            //        newPub.AcceptDate = pubDate;
            //        break;
            //    case (int)StatusEnum.Published:
            //        newPub.PubDate = pubDate;
            //        break;
            //    default:
            //        newPub.SubmitDate = pubDate;
            //        break;
            //}          
         
            if (pub != null && pub.Id > 0)
            {
                newPub.ParentId = pub.Id;
            }          

            //if (PubStatus!=(int)StatusEnum.SubResub) // sub publication
            //{
            //    newPub.ProjectId = pub.ProjectId;
            //    newPub.Title = pub.Title;
            //    newPub.Author = pub.Author;
            //    newPub.SubmitDate = pub.SubmitDate;

            //    if (pub.ConferenceId != null)
            //    {
            //        newPub.ConferenceId = pub.ConferenceId;
            //    }

            //    if (pub.JounralId != null)
            //    {
            //        newPub.JounralId = pub.JounralId;
            //    }

            //    //check if newpub already exists based on type, projectid, status and submitdate
            //    bool existingPub = context.Publications
            //                         .Any(p => p.ProjectId == ProjectId && p.Status == PubStatus && p.SubmitDate == newPub.SubmitDate);
            //    if (existingPub)
            //    {
            //        return false;
            //    }

            //    context.Publications.Add(newPub);
            //    //context.SaveChanges();

            //    //ddlChildPublication.Enabled = true;
            //    //ddlChildPublication.Items.Insert(ddlChildPublication.Items.Count, new ListItem(newPub.Id.ToString() + " " + newPub.Title, newPub.Id.ToString()));
            //}
            //else // new publication
            //{

            //check if pub already exists
            bool existingPub = context.Publications
                               .Any(p => p.ProjectId == ProjectId && p.Status == PubStatus && p.SubmitDate == newPub.SubmitDate);
            if (existingPub)
            {
                return false;
            }

            newPub.ProjectId = ProjectId;
            newPub.Title = TextBoxTitle.Text;
            newPub.Author = TextAreaAuthors.InnerText;

            if (IsAbstract)
            {
                // save conference
                if (ddlConference.SelectedValue.Equals(string.Empty))
                {
                    Conference newConf = GetConference();

                    context.Conferences.Add(newConf);
                    context.SaveChanges();
                    newPub.ConferenceId = newConf.Id;
                }
                else
                {
                    newPub.ConferenceId = Convert.ToInt32(ddlConference.SelectedValue);
                }
            }

            //journal
            if (ValidateJournalControl())
            {
                bool journal = context.Journals
                                .Any(j => j.JournalName == TextBoxJournal.Text);

                if (journal)
                {
                    Journal x = context.Journals
                                .First(j => j.JournalName == TextBoxJournal.Text);

                    newPub.JounralId = x.Id;
                                        
                }
                else
                {
                    //new journal
                    Journal newJ = GetJournal();

                    context.Journals.Add(newJ);

                    context.SaveChanges();

                    newPub.JounralId = newJ.Id;
                }
            }
            else
            {
                newPub.JounralId = -1;
            }                    

            context.Publications.Add(newPub);
            context.SaveChanges();

            ddlPublication.Items.Insert(ddlPublication.Items.Count, new ListItem(newPub.Id.ToString() + " " + newPub.Title, newPub.Id.ToString()));

            //PublicationBioStat
            ICollection<PublicationBioStat> pubBiostatList = new Collection<PublicationBioStat>();
            foreach (GridViewRow row in GridViewBioStat.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Label lblId = row.FindControl("lblId") as Label;
                        int biostatId = Convert.ToInt32(lblId.Text);

                        PublicationBioStat pubBiostat = new PublicationBioStat()
                        {
                            BioStats_Id = biostatId,
                            Publications_Id = newPub.Id,
                            StartDate = Convert.ToDateTime(HiddenFieldCurrentDate.Value),
                            EndDate = Convert.ToDateTime("2099-01-01")
                        };

                        pubBiostatList.Add(pubBiostat);
                    }
                }
            }

            newPub.PublicationBioStats = pubBiostatList;

            //PublicationGrant
            ICollection<PublicationGrant> pubGrantList = new Collection<PublicationGrant>();
            foreach (GridViewRow row in GridViewGrant.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Label lblId = row.FindControl("lblId") as Label;
                        int grantId = Convert.ToInt32(lblId.Text);

                        PublicationGrant pubGrant = new PublicationGrant()
                        {
                            GrantAffilId = grantId,
                            PublicationId = newPub.Id,
                            StartDate = Convert.ToDateTime(HiddenFieldCurrentDate.Value),
                            EndDate = Convert.ToDateTime("2099-01-01")
                        };

                        pubGrantList.Add(pubGrant);
                    }
                }
            }

            newPub.PublicationGrants = pubGrantList;
               
            //}

            context.SaveChanges();

            return true;
            
        }

        private bool ValidateJournalControl()
        {
            if (TextBoxJournal.Text.Equals(string.Empty)) return false;
            else if (TextBoxVolumeNo.Text.Equals(string.Empty)) return false;
            else if (TextBoxIssueNo.Text.Equals(string.Empty)) return false;
            else if (TextBoxPages.Text.Equals(string.Empty)) return false;
            else
                return true;
        }

        private bool UpdatePublication(ProjectTrackerContainer context, Publication pub, bool isSub)
        {
            if (ValidateJournalControl())
            {
                var journalExists = context.Journals
                              .Any(j => j.JournalName == TextBoxJournal.Text);

                if (journalExists)
                {
                    var journal = context.Journals
                              .First(j => j.JournalName == TextBoxJournal.Text);

                    pub.JounralId = journal.Id;
                }
                else
                {
                    //new journal
                    Journal newJournal = GetJournal();

                    context.Journals.Add(newJournal);

                    context.SaveChanges();

                    pub.JounralId = newJournal.Id;
                }
            }

            if (isSub)
            {
                //sub-publication can change date
                DateTime pubDate = Convert.ToDateTime(TextBoxSubmitDate.Text);
                switch (pub.Status)
                {
                    case (int)StatusEnum.Accepted:
                        pub.AcceptDate = pubDate;
                        break;
                    case (int)StatusEnum.Published:
                        pub.PubDate = pubDate;
                        break;
                    default:
                        pub.SubmitDate = pubDate;
                        break;
                }
                              
            }
            else
            {
                //update PublicationBioStat
                #region Update ProjectBioStat
                ICollection<PublicationBioStat> prevBiostats = pub.PublicationBioStats
                                                            .Where(b => b.EndDate > DateTime.Parse(_currentDate)).ToList();

                List<int> newMemberList = GetSelectedRow_GridView(GridViewBioStat);

                List<int> prevMemberList = new List<int>();

                if (prevBiostats.Count > 0)
                {
                    foreach (PublicationBioStat biostat in prevBiostats)
                    {
                        prevMemberList.Add(biostat.BioStats_Id);
                    }
                }

                var newNotPrevList = newMemberList.Except(prevMemberList).ToList();
                var prevNotNewList = prevMemberList.Except(newMemberList).ToList();

                foreach (var expiredId in prevNotNewList)
                {
                    pub.PublicationBioStats.First(b => b.BioStats_Id == expiredId).EndDate = DateTime.Parse(_currentDate);
                }
                foreach (var newMemberId in newNotPrevList)
                {
                    pub.PublicationBioStats.Add(new PublicationBioStat()
                    {
                        Publications_Id = pub.Id,
                        BioStats_Id = newMemberId,
                        StartDate = DateTime.Parse(_currentDate),
                        EndDate = _endDate
                    });
                }
                #endregion

                // update ProjectGrant
                #region Update Project Grants
                ICollection<PublicationGrant> prevGrants = pub.PublicationGrants
                                                            .Where(b => b.EndDate > DateTime.Parse(_currentDate)).ToList();

                List<int> newGrantList = GetSelectedRow_GridView(GridViewGrant);

                List<int> prevGrantList = new List<int>();
                if (prevGrants.Count > 0)
                {
                    foreach (PublicationGrant grant in prevGrants)
                    {
                        prevGrantList.Add(grant.GrantAffilId);
                    }
                }

                var newNotPrevGrantList = newGrantList.Except(prevGrantList).ToList();
                var prevNotNewGrantList = prevGrantList.Except(newGrantList).ToList();

                foreach (var removeId in prevNotNewGrantList)
                {
                    PublicationGrant pg = pub.PublicationGrants.First(d => d.PublicationId == pub.Id && d.GrantAffilId == removeId);
                    context.PublicationGrants.Remove(pg);
                }
                foreach (var newGrantId in newNotPrevGrantList)
                {
                    pub.PublicationGrants.Add(new PublicationGrant()
                    {
                        PublicationId = pub.Id,
                        GrantAffilId = newGrantId,
                        StartDate = DateTime.Parse(_currentDate),
                        EndDate = _endDate
                    });
                }
                #endregion

                //update conference 
                if (IsAbstract)
                {
                    // save conference
                    if (ddlConference.SelectedValue.Equals(string.Empty))
                    {
                        Conference newConf = GetConference();

                        context.Conferences.Add(newConf);
                        context.SaveChanges();
                        pub.ConferenceId = newConf.Id;
                    }
                    else
                    {
                        pub.ConferenceId = Convert.ToInt32(ddlConference.SelectedValue);
                    }
                }
            }

            context.SaveChanges();

            return true;
        }

        private Conference GetConference()
        {
            Conference newConf = new Conference()
            {
                ConfName = TextBoxName.Text,
                ConfLoc = TextBoxLocation.Text,
                StartDate = Convert.ToDateTime(TextBoxStartDate.Text),
                EndDate = Convert.ToDateTime(TextBoxEndDate.Text)
                //IsPoster = true
            };

            //if (Request.Form["optionsRadios"] != null)
            //{
            //    string confType = Request.Form["optionsRadios"].ToString();
            //    if (confType == "option2")
            //    {
            //        newConf.IsPoster = false;
            //    }
            //}
            return newConf;
        }

        private void SetConference(Conference conf)
        {
            if (conf != null)
            {
                TextBoxName.Text = conf.ConfName;
                TextBoxLocation.Text = conf.ConfLoc;
                TextBoxStartDate.Text = conf.StartDate.ToShortDateString();
                TextBoxEndDate.Text = conf.EndDate.ToShortDateString();

                //if (conf.IsPoster)
                //{
                //    ClientScript.RegisterStartupScript(GetType(), "Javascript",
                //       "<script>if (!document.getElementById('optionsRadios1').checked){document.getElementById('optionsRadios1').checked=true;}</script>");
                //}
                //else
                //{
                //    ClientScript.RegisterStartupScript(GetType(), "Javascript",
                //        "<script>if (!document.getElementById('optionsRadios2').checked){document.getElementById('optionsRadios2').checked = true;}</script>");
                //}
            }
            else
            {
                TextBoxName.Text = string.Empty;
                TextBoxLocation.Text = string.Empty;
                TextBoxStartDate.Text = string.Empty;
                TextBoxEndDate.Text = string.Empty;
            }
        }

        private Journal GetJournal()
        {
            Journal journal = new Journal()
            {
                JournalName = TextBoxJournal.Text,
                //Volume = TextBoxVolumeNo.Text,
                //Issue = TextBoxIssueNo.Text,
                //Page = TextBoxPages.Text,
                //DOI = TextBoxDOI.Text,
                //PMID = TextBoxPmid.Text,
                //PMCID = TextBoxPmcid.Text,
                //Type = IsAbstract == true ? "1" : "2"
            };

            return journal;
        }

        private void SetJournal(Journal journal)
        {
            if (journal != null)
            {
                TextBoxJournal.Text = journal.JournalName;
                //TextBoxVolumeNo.Text = journal.Volume;
                //TextBoxIssueNo.Text = journal.Issue;
                //TextBoxPages.Text = journal.Page;
                //TextBoxDOI.Text = journal.DOI;
                //TextBoxPmid.Text = journal.PMID;
                //TextBoxPmcid.Text = journal.PMCID;
            }
            else
            {
                TextBoxJournal.Text = string.Empty;
                TextBoxVolumeNo.Text = string.Empty;
                TextBoxIssueNo.Text = string.Empty;
                TextBoxPages.Text = string.Empty;
                TextBoxDOI.Text = string.Empty;
                TextBoxPmid.Text = string.Empty;
                TextBoxPmcid.Text = string.Empty;
            }
        }

        private void BindControl()
        {
            //ddlPubType
            foreach (Enum pubType in Enum.GetValues(typeof(PubTypeEnum)))
            {
                ListItem item = new ListItem(Enum.GetName(typeof(PubTypeEnum), pubType), pubType.ToString());
                ddlPubType.Items.Add(item);
            }

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                var dbQuery = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DateTime>("CurrentDateTime() ");
                HiddenFieldCurrentDate.Value = dbQuery.AsEnumerable().First().ToShortDateString();
                DateTime currentDate = Convert.ToDateTime(HiddenFieldCurrentDate.Value);


                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();                             

                //ddlProject
                dropDownSource = context.Projects
                            .OrderByDescending(d => d.Id)
                            .Select(x => new { x.Id, FullName = x.Id + " " + x.Title })
                            .ToDictionary(c => c.Id, c => c.FullName);

                BindDropDownList(ddlProject, dropDownSource, string.Empty);

                //GridViewGrant
                var query = context.GrantAffils
                            .Where(d => d.Id < 4)
                            .OrderBy(d => d.Id);

                GridViewGrant.DataSource = query.ToList();
                GridViewGrant.DataBind();                

                ////ddlBiostat
                dropDownSource = context.BioStats
                                .OrderBy(b => b.Id)
                                .ToDictionary(c => c.Id, c => c.Name);

                //BindDropDownList(ddlBiostat, dropDownSource, String.Empty);

                GridViewBioStat.DataSource = dropDownSource;
                GridViewBioStat.DataBind();
                
                if (PubType ==  PubTypeEnum.Abstract.ToString())
                {
                    //ddlConference
                    dropDownSource = context.Conferences
                                    .Where(d => d.EndDate >= currentDate)
                                    .ToDictionary(c => c.Id, c => (c.ConfName + " | " + c.ConfLoc));
                    
                    BindDropDownList(ddlConference, dropDownSource, "Add a new conference");

                    //dropDownSource = context.Publications.OfType<PubAbstract>()
                    //                .Where(p => p.Status == (int)StatusEnum.SubResub)
                    //                .ToDictionary(c => c.Id, c => (c.Id + " " + c.Title));
                }
                else
                {
                    //dropDownSource = context.Publications.OfType<PubManuscript>()
                    //                .Where(p => p.Status == (int)StatusEnum.SubResub)
                    //                .ToDictionary(c => c.Id, c => (c.Id + " " + c.Title));
                }

                BindDropDownList(ddlPublication, dropDownSource, NewPubItem);              
                               
            }
        }


        protected void ddlPubType_Changed(Object sender, EventArgs e)
        {
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();
                if (PubType == PubTypeEnum.Abstract.ToString())
                {
                    //dropDownSource = context.Publications.OfType<PubAbstract>()
                    //                .Where(p => p.ParentId == null)
                    //                .ToDictionary(c => c.Id, c => (c.Id + " " + c.Title));

                    //CheckBoxConvert.Enabled = true;
                }
                else
                {
                    //dropDownSource = context.Publications.OfType<PubManuscript>()
                    //                .Where(p => p.ParentId == null)
                    //                .ToDictionary(c => c.Id, c => (c.Id + " " + c.Title));

                    //CheckBoxConvert.Enabled = false;
                }

                BindDropDownList(ddlPublication, dropDownSource, NewPubItem);
            }

            //ClientScript.RegisterStartupScript(GetType(), "Javascript",
            //                   "<script>divAbstractToggle(this)</script>");

            ClearForm();
        }

        protected void ddlConference_Changed(Object sender, EventArgs e)
        {
            int id = 0;
            bool result = Int32.TryParse(ddlConference.SelectedValue, out id);

            if (id > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    var conf = context.Conferences.First(c => c.Id == id);

                    SetConference(conf);
                }
            }
            else
            {
                SetConference(null);
            }

            ddlConference.Focus();

        }

        //protected void ddlInvestor_Changed(Object sender, EventArgs e)
        //{
        //    int id;
        //    bool result = Int32.TryParse(ddlInvestor.SelectedValue, out id);

        //    using (ProjectTrackerContainer context = new ProjectTrackerContainer())
        //    {
        //        IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

        //        if (result && id > 0)
        //        {
        //            var invest = context.Invests.First(i => i.Id == id);

        //            dropDownSource = invest.Projects
        //                            .OrderByDescending(d => d.Id)
        //                            .ToDictionary(c => c.Id, c => c.Title);  
        //        }
        //        else
        //        {
        //            dropDownSource = context.Projects
        //                            .OrderByDescending(d => d.Id)
        //                            //.Select(x => new { x.Id, FullName = x.Id + " " + x.Title });
        //                            .ToDictionary(c => c.Id, c => c.Title);                    
        //        }

        //        BindDropDownList(ddlProject, dropDownSource, string.Empty);

        //    }

        //    //ClearForm();

        //    ddlProject.Focus();

        //    ddlPublication.Items.Clear();
        //    ddlPublication.Items.Insert(0, new ListItem(NewPubItem, String.Empty));
        //}

        protected void ddlProject_Changed(Object sender, EventArgs e)
        {
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                if (PubType == PubTypeEnum.Abstract.ToString())
                {
                    //dropDownSource = context.Publications.OfType<PubAbstract>()
                    //                .Where(p => p.ProjectId == ProjectId)
                    //                .ToDictionary(c => c.Id, c => (c.Id + " " + c.Title));
                }
                else
                {
                    //dropDownSource = context.Publications.OfType<PubManuscript>()
                    //                .Where(p => p.ProjectId == ProjectId)
                    //                .ToDictionary(c => c.Id, c => (c.Id + " " + c.Title));
                }

                BindDropDownList(ddlPublication, dropDownSource, NewPubItem);
            }
            //lblMsg.Text = selectedProjectId.ToString();

            ddlProject.Focus();
            //HiddenFieldPubType.Value = selectedPubType;
        }
        
        protected void ddlPublication_Changed(Object sender, EventArgs e)
        {
            int pubId;
            bool result = Int32.TryParse(ddlPublication.SelectedValue, out pubId);

            if (result)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    Publication pub = context.Publications.First(p => p.Id == pubId);

                    IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                    if (pub.ProjectId > 0)
                    {
                        ddlProject.SelectedValue = pub.ProjectId.ToString();

                        BindGridViewFile();
                    }
                    else
                    {
                        ddlProject.ClearSelection();
                    }
                    
                    TextBoxSubmitDate.Text = pub.SubmitDate.ToString();
                    TextBoxTitle.Text = pub.Title;
                    TextAreaAuthors.InnerText = pub.Author;

                    if (PubType == PubTypeEnum.Abstract.ToString())
                    {
                        Conference conf = context.Conferences.First(c => c.Id == pub.ConferenceId);

                        SetConference(conf);

                        //dropDownSource = context.Publications.OfType<PubAbstract>()
                        //                .Where(p => p.ParentId == pub.Id)
                        //                .ToDictionary(c => c.Id, c => (c.Id + " " + (StatusEnum)c.Status));
                    }
                    else
                    {
                        //dropDownSource = context.Publications.OfType<PubManuscript>()
                        //                .Where(p => p.ParentId == pub.Id)
                        //                .ToDictionary(c => c.Id, c => (c.Id + " " + (StatusEnum)c.Status));
                    }

                    //ddlChildPublication.Enabled = true;
                    //BindDropDownList(ddlChildPublication, dropDownSource, "Add a new sub");

                    List<int> biostats = new List<int>();
                    foreach (var pubBiostat in pub.PublicationBioStats)
                    {
                        if (pubBiostat.EndDate > DateTime.Parse(_currentDate))
                        {
                            biostats.Add(pubBiostat.BioStats_Id);
                        }
                    }
                    if (biostats.Count > 0)
                    {
                        SelectRow_GridView(GridViewBioStat, biostats);
                    }

                    SetPubStatus(pub.Status);

                    List<int> grants = new List<int>();
                    foreach (var pubGrant in pub.PublicationGrants)
                    {
                        if (pubGrant.EndDate > DateTime.Parse(_currentDate))
                        {
                            grants.Add(pubGrant.GrantAffilId);
                        }
                    }
                    if (grants.Count > 0)
                    {
                        SelectRow_GridView(GridViewGrant, grants);
                    }

                    if (pub.JounralId > 0)
                    {
                        Journal journal = context.Journals.First(j => j.Id == pub.JounralId);

                        if (journal != null)
                        {
                            SetJournal(journal);

                        }
                    }

                }
            }
            else
            {
                ClearForm();               
            }
        }

       

        private void ClearForm()
        {
            //ddlChildPublication.Items.Clear();
            ddlPublication.ClearSelection();
            //ddlChildPublication.Enabled = false;
            //CheckBoxConvert.Checked = false;

            UncheckGrid(GridViewBioStat);

            UncheckGrid(GridViewGrant);

            SetPubStatus((int)StatusEnum.SubResub);

            TextBoxSubmitDate.Text = string.Empty;

            TextAreaAuthors.InnerText = string.Empty;

            TextBoxTitle.Text = string.Empty;

            ddlConference.ClearSelection();

            SetConference(null);

            SetJournal(null);

            lblDebugMsg.Text = string.Empty;
        }

        //protected void ddlChildPublication_Changed(Object sender, EventArgs e)
        //{
        //    int id;
        //    bool result = Int32.TryParse(ddlChildPublication.SelectedValue, out id);

        //    if (result)
        //    {
        //        using (ProjectTrackerContainer context = new ProjectTrackerContainer())
        //        {
        //            Publication pub = context.Publications.First(p => p.Id == id);

        //            if (pub != null)
        //            {
        //                Set_Status(pub.Status);
        //                TextBoxSubmitDate.Text = pub.SubmitDate.ToString();

        //                switch (pub.Status)
        //                {
        //                    case (int)StatusEnum.Accepted:
        //                        TextBoxSubmitDate.Text = pub.AcceptDate.ToString();
        //                        break;
        //                    case (int)StatusEnum.Published:
        //                        TextBoxSubmitDate.Text = pub.PubDate.ToString();
        //                        break;
        //                    default:
        //                        TextBoxSubmitDate.Text = pub.SubmitDate.ToShortDateString();
        //                        break;
        //                }

        //                //if (pub.Status == (int)StatusEnum.Published)
        //                //{
        //                //    if (pub.JounralId > 0)
        //                //    {
        //                //        Journal journal = context.Journals.First(i => i.Id == pub.JounralId);

        //                //        if (journal != null)
        //                //        {
        //                //            TextBoxJournal.Text = journal.Name;
        //                //            TextBoxVolumeNo.Text = journal.Volume;
        //                //            TextBoxIssueNo.Text = journal.Issue;
        //                //            TextBoxPages.Text = journal.Page;
        //                //        }
        //                //    }
        //                //}
        //            }
        //        }
        //    }
        //}

        private void SetPubStatus(int pubStatus)
        {
            if (pubStatus == (int)StatusEnum.SubResub) optionSubmitted.Checked = true;
            else if (pubStatus == (int)StatusEnum.Accepted) optionAccepted.Checked = true;
            else if (pubStatus == (int)StatusEnum.Published) optionPublished.Checked = true;
            else optionNotAccepted.Checked = true;
        }

        private void SetControlValue()
        {
            throw new NotImplementedException();
        }

        private void BindDropDownList(DropDownList ddl, IDictionary<int, string> dataSource, string firstItemText)
        {
            ddl.DataSource = dataSource;
            ddl.DataValueField = "Key";
            ddl.DataTextField = "Value";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(firstItemText, String.Empty));
        }

        protected string PubType
        {
            //get { return optionAbstract.Checked ? "Abstract" : "Manuscript"; }
            get { return ddlPubType.SelectedValue; }
        }
        protected int ProjectId 
        {
            get { return ddlProject.SelectedValue.Equals(string.Empty) ? -1 : Convert.ToInt32(ddlProject.SelectedValue); }
        }

        protected string NewPubItem
        {
            get { return string.Concat("Add a New ", PubType); }
        }

        protected bool IsAbstract
        {
            get { return ddlPubType.SelectedValue.Equals(PubTypeEnum.Abstract.ToString()); }
        }

        protected int PubStatus
        {
            get
            {             
                if (optionSubmitted.Checked) return (int)StatusEnum.SubResub;           //"Submitted"
                else if (optionAccepted.Checked) return (int)StatusEnum.Accepted;       //"Accepted"
                else if (optionPublished.Checked) return (int)StatusEnum.Published;     //"Published";
                else return (int)StatusEnum.NotAccepted;                                //"NotAccepted";              
            }
        }

        private void DisableOptions()
        {
            optionAccepted.Disabled = true;
            optionPublished.Disabled = true;
            optionNotAccepted.Disabled = true;
        }

        private void SelectRow_GridView(GridView gridView, List<int> idList)
        {
            foreach (GridViewRow row in gridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    chkRow.Checked = false;
                    Label lblId = row.FindControl("lblId") as Label;

                    if (idList.Contains(Convert.ToInt32(lblId.Text)))
                    {
                        chkRow.Checked = true;
                    }
                }
            }
        }

        List<int> GetSelectedRow_GridView(GridView gv)
        {
            List<int> list = new List<int>();

            foreach (GridViewRow row in gv.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    Label lblId = row.FindControl("lblId") as Label;

                    if (chkRow.Checked == true)
                    {
                        list.Add(Convert.ToInt32(lblId.Text));
                    }
                }
            }

            return list;
        }

        private void UncheckGrid(GridView gv)
        {
            foreach (GridViewRow row in gv.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    chkRow.Checked = false;
                }
            }
        }        

        protected void linkButtonDownload_Click(object sender, EventArgs e)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "TimeEntry_2015_April.xlsx");
            //Response.TransmitFile(Server.MapPath("~/UploadFiles/TimeEntry_2015_April.xlsx"));
            Response.WriteFile("~/UploadFiles/TimeEntry_2015_April.xlsx");
            Response.End();
        }
    }
}