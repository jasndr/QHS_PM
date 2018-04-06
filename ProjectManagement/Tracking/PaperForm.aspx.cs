using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ProjectManagement.Dtos;

namespace ProjectManagement.Admin
{
    /// <summary>
    /// @File: PaperForm.aspx.cs
    /// @Author: Yang Rui
    /// @Summary: Lists publications with QHS involvement, both paper and abstracts. 
    /// 
    ///           The initial view displays a list of all the papers that QHS has collaborated in
    ///           sequential order of by the latest "add" date.  The list can be filtered by paper type
    ///           (manuscript or abstract) as well as by QHS faculty or staff member.
    ///           
    ///           When users click either the "Add a new paper" or "Edit" buttons, users are able to 
    ///           determing whether or not a paper was an abstract or manuscript, add the paper information
    ///           such as title, authors, affiliations, journal or conference accepted to, and its status 
    ///           (submitted, accepted, published, or accepted) and date of status, as well as add paper-related 
    ///           information such as the project in the database that corresponds to the paper, the affiliations
    ///           of the authors (to be able to track QHS collaborations), as well as QHS faculty and staff that has been
    ///           involved in the paper.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  2018APR02 - Jason Delos Reyes  -  Added documentation to be able to describe the methods' functionalities. 
    /// </summary>
    public partial class PaperForm : System.Web.UI.Page
    {
        ProjectTrackerRepository _repository = new ProjectTrackerRepository();

        static readonly DateTime _endDate = DateTime.Parse("2099-01-01");
        string _currentDate = DateTime.Now.ToShortDateString();
        //int paperId = 0;
        int rowIndex = 1;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string s = ddlPubType.SelectedValue;
            string t = ddlBiostat.SelectedValue;
        }

        /// <summary>
        /// Binds dropdowns and checkboxes of PaperForm page with
        /// currently stored information in the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {         
                BindControl();

                //if (Request.QueryString["id"] != null)
                //{
                //    var PK = Convert.ToString(Request.QueryString["id"]);
                    
                //    int paperId = 0;
                //    Int32.TryParse(PK, out paperId);

                //    if (paperId > 0)
                //    {
                //        LoadPaperForEdit(paperId);
                //        LoadEditScript();
                //    }
                //}
            }

            if (!HiddenFieldCurrentDate.Value.Equals(string.Empty))
            {
                _currentDate = HiddenFieldCurrentDate.Value;
            }
        }       

        /// <summary>
        /// Button that activates function to pull list of papers from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumbit_Click(object sender, EventArgs e)
        {
            BindGridViewPaperAll();            
        }

        /// <summary>
        /// When "Submit" button is selected, a full list of papers with either a specified paper type 
        /// (abstract or manuscript), QHS faculty and staff, both, or neither is indicated for selection.
        /// </summary>
        private void BindGridViewPaperAll()
        {
            int pubType, biostatId;

            Int32.TryParse(ddlPubType.SelectedValue, out pubType);
            Int32.TryParse(ddlBiostat.SelectedValue, out biostatId);

            DataTable paperTable = _repository.GetPaperAll(pubType, biostatId);

            GridViewPaper.DataSource = paperTable;
            GridViewPaper.DataBind();
        }

        /// <summary>
        /// Saves the newly-entered infomation on the Paper form onto the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //var names = Request.Form["Name"];

            StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            if (ValidatePaper().Equals(string.Empty))
            {             
                Publication newPaper = GetPaper(PaperId);
                Journal newJournal = GetJournal();
                Conference newConf = GetConference();

                List<int> newMemberList = PageUtility.GetSelectedRow_GridView(GridViewBioStat);
                List<int> newGrantList = PageUtility.GetSelectedRow_GridView(GridViewGrant);
                //List<int> newAuthorAffilList = PageUtility.GetSelectedRow_GridView(GridView1);                
                
                var paperDto = new PaperDto
                {
                    Publication = newPaper,
                    Journal = newJournal,
                    Conference = newConf,
                    Members = newMemberList,
                    Grants = newGrantList,
                    Creator = Creator
                };

                if (PaperId > 0)
                {
                    _repository.UpdatePaper(paperDto);
                }
                else
                {
                    _repository.CreatePaper(paperDto);
                }

                if (optionManuscript.Checked)
                {
                    sb.Append("$('#divAbstract').hide();");
                }                

                sb.Append("alert('Record Updated Successfully');");

                sb.Append("$('#editModal').modal('hide');");

                BindGridViewPaperAll();

            }
            else
            {
                sb.Append("alert('" + ValidatePaper() + "');");
            }

            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
        }

        /// <summary>
        /// NOTE: Currently not being used.
        /// 
        /// Gets selected id of paper affiliations.
        /// </summary>
        /// <param name="rpt"></param>
        /// <returns></returns>
        private List<int> GetSelectedId_Repeater(Repeater rpt)
        {
            HashSet<int> list = new HashSet<int>();

            foreach (RepeaterItem i in rpt.Items)
            {
                CheckBox cb1, cb2;
                HiddenField hdnValue1, hdnValue2;

                cb1 = (CheckBox)i.FindControl("FirstchkId");
                cb2 = (CheckBox)i.FindControl("SecondchkId");

                hdnValue1 = (HiddenField)i.FindControl("FirstId");
                hdnValue2 = (HiddenField)i.FindControl("SecondId");

                if (cb1 != null && hdnValue1 != null && cb1.Checked)
                {
                    list.Add(Convert.ToInt32(hdnValue1.Value));
                }

                if (cb2 != null && hdnValue2 != null && cb2.Checked)
                {
                    list.Add(Convert.ToInt32(hdnValue2.Value));
                }
            }

            List<int> sortedList = new List<int>(list);

            sortedList.Sort();

            return sortedList;
        }

        /// <summary>
        /// Searches database for the affiliation id of the noted affiliations.
        /// Adds the id to the list, to be lated applied to the grid, of the
        /// matched affiliation.
        /// </summary>
        /// <param name="gvAffil">Grid with the affiliations.</param>
        /// <returns>List of ids with affiliations.</returns>
        private List<int> GetAffiliationId(GridView gvAffil)
        {
            HashSet<int> list = new HashSet<int>();

            foreach (GridViewRow row in gvAffil.Rows)
            {
                int id = 0;
                TextBox lblId = row.FindControl("lblId") as TextBox;
                if (Int32.TryParse(Request.Form[lblId.UniqueID], out id))
                {
                    list.Add(id);
                }               
            }

            List<int> sortedList = new List<int>(list);

            sortedList.Sort();

            return sortedList;
        }        

        /// <summary>
        /// NOTE: Currently not being used.
        /// 
        /// Updates conference information.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private int UpdateConf(ProjectTrackerContainer db)
        {
            int confId = -1;

            if (ValidateConf())
            {
                DateTime sd = Convert.ToDateTime(TextBoxStartDate.Text);
                DateTime ed = Convert.ToDateTime(TextBoxEndDate.Text);

                var confExists = db.Conferences.Any(c => c.ConfName == TextBoxName.Text && c.ConfLoc == TextBoxLocation.Text 
                                             && c.StartDate == sd && c.EndDate == ed);

                if (confExists)
                {
                    var conf = db.Conferences.First(c => c.ConfName == TextBoxName.Text && c.ConfLoc == TextBoxLocation.Text
                                             && c.StartDate == sd && c.EndDate == ed);

                    confId = conf.Id;
                }
                else
                {
                    Conference newConf = new Conference()
                                        {
                                            ConfName = TextBoxName.Text,
                                            ConfLoc = TextBoxLocation.Text,
                                            StartDate = sd,
                                            EndDate = ed
                                            //IsPoster = optionsPoster.Checked                                            
                                        };

                    db.Conferences.Add(newConf);

                    db.SaveChanges();

                    confId = newConf.Id;
                }
            }

            return confId;
        }


        /// <summary>
        /// NOTE: Currently not being used.
        /// 
        /// Updates journal information.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private int UpdateJournal(ProjectTrackerContainer db)
        {
            int journalId = -1;

            if (ValidateJournal())
            {
                var journalExists = db.Journals
                              .Any(j => j.JournalName == TextBoxJournal.Text);

                if (journalExists)
                {
                    var journal = db.Journals
                              .First(j => j.JournalName == TextBoxJournal.Text);

                    journalId = journal.Id;
                }
                else
                {
                    Journal newJournal = new Journal()
                                        {
                                            JournalName = TextBoxJournal.Text                                           
                                        }; 

                    db.Journals.Add(newJournal);

                    db.SaveChanges();

                    journalId = newJournal.Id;
                }
            }

            return journalId;
        }


        /// <summary>
        /// NOTE: Currently not being used.
        /// 
        /// Updates coauthor information.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="paper"></param>
        private void UpdateCoAuthoAffil(ProjectTrackerContainer db, Publication paper)
        {
            ICollection<PublicationCoAuthorAffil> prevAffils = paper.PublicationCoAuthorAffils
                                                        .Where(b => b.EndDate > DateTime.Parse(_currentDate)).ToList();

            //List<int> newAffils = PageUtility.GetSelectedRow_GridView(GridViewCoAuthotAffil);

            List<int> prevAffilList = new List<int>();
            if (prevAffils.Count > 0)
            {
                foreach (PublicationCoAuthorAffil affil in prevAffils)
                {
                    prevAffilList.Add(affil.CoAuthorAffilId);
                }
            }

            //var newNotPrevList = newAffils.Except(prevAffilList).ToList();
            //var prevNotNewList = prevAffilList.Except(newAffils).ToList();

            //foreach (var removeId in prevNotNewList)
            //{
            //    PublicationCoAuthorAffil pg = paper.PublicationCoAuthorAffils.First(d => d.PublicationId == paper.Id && d.CoAuthorAffilId == removeId);
            //    db.PublicationCoAuthorAffils.Remove(pg);
            //}
            //foreach (var newId in newNotPrevList)
            //{
            //    paper.PublicationCoAuthorAffils.Add(new PublicationCoAuthorAffil()
            //    {
            //        PublicationId = paper.Id,
            //        CoAuthorAffilId = newId,
            //        StartDate = DateTime.Parse(_currentDate),
            //        EndDate = _endDate
            //    });
            //}

        }

        /// <summary>
        /// NOTE: Currently not being used.
        /// 
        /// Updates grant information.
        /// </summary>
        /// <param name="db">Project Tracking Container</param>
        /// <param name="paper">Paper</param>
        private void UpdateGrants(ProjectTrackerContainer db, Publication paper)
        {
            ICollection<PublicationGrant> prevGrants = paper.PublicationGrants
                                                        .Where(b => b.EndDate > DateTime.Parse(_currentDate)).ToList();

            List<int> newGrantList = PageUtility.GetSelectedRow_GridView(GridViewGrant);

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
                PublicationGrant pg = paper.PublicationGrants.First(d => d.PublicationId == paper.Id && d.GrantAffilId == removeId);
                db.PublicationGrants.Remove(pg);
            }
            foreach (var newGrantId in newNotPrevGrantList)
            {
                paper.PublicationGrants.Add(new PublicationGrant()
                {
                    PublicationId = paper.Id,
                    GrantAffilId = newGrantId,
                    StartDate = DateTime.Parse(_currentDate),
                    EndDate = _endDate
                });
            }
        }

        /// <summary>
        /// NOTE: Currently not being used.
        /// 
        /// Updates paper information.
        /// </summary>
        /// <param name="paper">Instance of paper (publication).</param>
        private void UpdateBiostats(Publication paper)
        {
            ICollection<PublicationBioStat> prevBiostats = paper.PublicationBioStats
                                                            .Where(b => b.EndDate > DateTime.Parse(_currentDate)).ToList();

            List<int> newMemberList = PageUtility.GetSelectedRow_GridView(GridViewBioStat);

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
                paper.PublicationBioStats.First(b => b.BioStats_Id == expiredId).EndDate = DateTime.Parse(_currentDate);
            }
            foreach (var newMemberId in newNotPrevList)
            {
                paper.PublicationBioStats.Add(new PublicationBioStat()
                {
                    Publications_Id = paper.Id,
                    BioStats_Id = newMemberId,
                    StartDate = DateTime.Parse(_currentDate),
                    EndDate = _endDate
                });
            }
        }

        /// <summary>
        /// Checks to see if everything required on the Paper (Publication in database) form is
        /// complete. Returns an error message if incomplete / not correct.
        /// </summary>
        /// <returns>Returns error message if one of the required fields are missing;
        ///          Returns an empty string if all required feilds are filled out.</returns>
        private string ValidatePaper()
        {
            System.Text.StringBuilder validateResult = new System.Text.StringBuilder();

            if (ddlProject.SelectedValue.Equals(string.Empty))
            {
                validateResult.Append("Project is required. \\n");
            }

            if (TextBoxTitle.Text.Equals(string.Empty))
            {
                validateResult.Append("Paper title is required. \\n");
            }

            if (TextBoxAuthor.Text.Equals(string.Empty))
            {
                validateResult.Append("Author is required. \\n");
            }

            DateTime dateValue;
            if (!DateTime.TryParse(TextBoxSubmitDate.Text, out dateValue))
            {
                validateResult.Append("Submit date is required. \\n");
            }

            if (lblPaperId.Text.Equals(string.Empty) && optionSubmitted.Checked == false)
            {
                validateResult.Append("New paper status should be submitted. \\n");
            }

            return validateResult.ToString();
        }

        /// <summary>
        ///  - Pre-populates the following dropdowns:
        ///         (1) Paper Type - Abstract or Manuscript
        ///         (2) Project - List of exisitng projects in the database
        ///         (3) Biostat - List of QHS faculty and staff currently employed
        ///  - Populates the following checkboxes:
        ///         (1) Biostat - List of QHS faculty and staff currently employed
        ///         (2) Grant - List of grants as paper grants currently marked as paper 
        ///                     (RMATRIX, G12 Bridges, INBRE, Mountain West)
        /// </summary>
        private void BindControl()
        {
            if (!Creator.Equals(string.Empty))
            { 
                IDictionary<string, IDictionary<int, string>> controlSource = _repository.GetPaperControlSource();

                PageUtility.BindDropDownList(ddlPubType, controlSource["ddlPubType"], "--- Select All ---");

                PageUtility.BindDropDownList(ddlProject, controlSource["ddlProject"], string.Empty);

                PageUtility.BindDropDownList(ddlBiostat, controlSource["ddlBiostat"], "--- Select All ---");

                GridViewBioStat.DataSource = controlSource["ddlBiostat"];
                GridViewBioStat.DataBind();

                GridViewGrant.DataSource = controlSource["GridViewGrant"];
                GridViewGrant.DataBind();

                var affils = (Dictionary<int, string>)controlSource["rptAffiliation"];
                BindTable2(affils, rptAffiliation);

                var affilsJson = affils.Select(x => new { Id = x.Key, Name = x.Value });
                    //affils.Select(d => string.Format("\"Id\": {0}", d.Key, string.Join(",", string.Format("\"Name\": {0}", d.Value))));
                textAreaAffil.Value = Newtonsoft.Json.JsonConvert.SerializeObject(affilsJson);
            }
        }

        //protected void ddlBiostat_Changed(Object sender, EventArgs e)
        //{
        //    BindGridViewPaper();
        //}

        //private void BindGridViewPaper()
        //{
        //    int bioStatId;
        //    Int32.TryParse(ddlBiostat.SelectedValue, out bioStatId);

        //    using (ProjectTrackerContainer context = new ProjectTrackerContainer())
        //    {
        //        if (bioStatId > 0)
        //        {
        //            var query = context.PublicationBioStats
        //                        .Join(context.Publications, b => b.Publications_Id, p => p.Id, (b, p) => new { b, p })
        //                        .Where(i => i.b.BioStats_Id == bioStatId)
        //                        .Select(a => a.p);

        //            GridViewPaper.DataSource = query.ToList();
        //        }
        //        else
        //        {
        //            var query = context.Publications;

        //            GridViewPaper.DataSource = query.ToList();
        //        }

        //        GridViewPaper.DataBind();

        //    }
        //}


        /// <summary>
        /// Initialize Paper History grid (captures changes for paper status, journal, or conference info).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DropDownList ddl = null;

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ddl = e.Row.FindControl("ddlStatusNew") as DropDownList;
            }

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ddl = e.Row.FindControl("ddlStatus") as DropDownList;
            //}

            //if (ddl != null)
            //{
            //    using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            //    {
            //        ddl.DataSource = context.PublishStatus.Where(p => !p.Id.Equals("-1")).ToList();       // && !p.Id.Equals("1")).ToList();
            //        ddl.DataTextField = "Name";
            //        ddl.DataValueField = "Id";
            //        ddl.DataBind();
            //        ddl.Items.Insert(0, new ListItem(""));
            //    }
            //    //if (e.Row.RowType == DataControlRowType.DataRow)
            //    //{
            //    //    ddl.SelectedValue = ((Invest)(e.Row.DataItem)).InvestStatu.Id.ToString();
            //    //}
            //}
        }

        //public void AddNewRow(object sender, GridViewRowEventArgs e)
        //{
        //    GridView GridView1 = (GridView)sender;
        //    GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
        //    NewTotalRow.Font.Bold = true;
        //    NewTotalRow.BackColor = System.Drawing.Color.Aqua;
        //    TableCell HeaderCell = new TableCell();
        //    HeaderCell.Height = 10;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderCell.ColumnSpan = 4;
        //    NewTotalRow.Cells.Add(HeaderCell);
        //    GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, NewTotalRow);
        //    rowIndex++;
        //}
    
        /// <summary>
        /// Intializes Paper History grid row command (does nothing; used to be able to add history manually).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "InsertNew")
            //{
            //    GridViewRow row = GridViewHistory.FooterRow;

            //    DropDownList ddlStatus = row.FindControl("ddlStatusNew") as DropDownList;
            //    TextBox txtDateNew = row.FindControl("txtDateNew") as TextBox;

            //    if (txtDateNew != null && ddlStatus != null)
            //    {
            //        using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            //        {
            //            int pubId = Convert.ToInt32(lblPaperId.Text);
            //            PubHistory pubHistory = new PubHistory()
            //            {
            //                PublicationId = pubId,
            //                StatusId = Convert.ToInt32(ddlStatus.SelectedValue),
            //                PaperDate = Convert.ToDateTime(txtDateNew.Text),
            //                Creator = this.Creator,
            //                CreateDate = Convert.ToDateTime(HiddenFieldCurrentDate.Value)
            //            };

            //            db.PubHistories.Add(pubHistory);
            //            db.SaveChanges();

            //            //BindGridViewHistory(db, paper, GridViewHistory);
            //        }

                   
            //    }
            //}
        }

        /// <summary>
        /// Initialized list of Papers grid (ability to edit paper information with "Edit" button).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewPaper_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("editRecord"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int paperId = (int)GridViewPaper.DataKeys[index].Value;

                if (paperId > 0)
                {
                    ClearEditForm();

                    LoadPaperForEdit(paperId);

                    LoadEditScript();
                }
            }
        }

        /// <summary>
        /// Pulls paper based on the paper ID given.
        ///     - If there is a paper matching the given paper ID,
        ///         = Paper History is populated,
        ///         = Paper Affiliation is populated,
        ///         = SET paper,
        /// </summary>
        /// <param name="paperId"></param>
        private void LoadPaperForEdit(int paperId)
        {
            Publication paper = _repository.GetPaper(paperId);

            if (paper != null)
            {
                //List<PaperCoAuthorAffil> coAuthorAffils = mgr.GetCoAuthorAffil(paper.Id);
                //List<PaperHistory> paperHistorys = mgr.GetPaperHistory(paper.Id);

                List<PaperHistory> paperHistorys = _repository.GetPaperHistory(paper.Id);

                List<int> affiliationList = new List<int>();
                if (!string.IsNullOrEmpty(paper.Affiliation))
                {
                    affiliationList = paper.Affiliation.Split(';').Select(int.Parse).ToList();
                }

                //List<JabsomAffil> affiliations = mgr.GetCoAuthorAffil2(affiliationList);
                List<JabsomAffil> affiliations = _repository.GetCoAuthorAffil2(affiliationList);

                SetPaper(paper, affiliations, paperHistorys);
            }                       
        }  

        
        /// <summary>
        /// NOTE: Currently not being used.
        /// 
        /// Creates an empty grid view.
        /// </summary>
        private void GridViewBindEmpty()
        {
            DataTable dt = new DataTable("emptyTable");

            dt.Columns.Add("Id", System.Type.GetType("System.String"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            dt.Columns.Add("JournalName", System.Type.GetType("System.String"));
            dt.Columns.Add("PaperDate", System.Type.GetType("System.String"));

            DataRow row = dt.NewRow();
            row[0] = "";
            row[1] = "";
            row[2] = "";
            row[3] = "";

            dt.Rows.Add(row);
            GridViewHistory.DataSource = dt;
            GridViewHistory.DataBind();
        }

        //private void BindGridViewHistory(ProjectTrackerContainer db, int paperId, GridView GridViewHistory)
        //{
        //    var histories = from h in db.PubHistories
        //                    join p in db.Publications on h.PublicationId equals p.Id
        //                    join s in db.PublishStatus on h.StatusId equals s.Id
        //                    join j in db.Journals on h.JournalId equals j.Id
        //                    join c in db.Conferences on h.ConfId equals c.Id
        //                    where p.Id == paperId
        //                    select new { h.Id, h.PubType, h.PaperDate, s.Name, j.JournalName, c.ConfName };

        //    GridViewHistory.DataSource = histories.ToList();

        //    GridViewHistory.DataBind();
        //}

        /// <summary>
        /// Binds the paper changes history (status, journal, or conference info.) to the current
        /// paper form.
        /// </summary>
        /// <param name="paperHistorys"></param>
        /// <param name="GridViewHistory"></param>
        private void BindGridViewHistory(List<PaperHistory> paperHistorys, GridView GridViewHistory)
        {
            GridViewHistory.DataSource = paperHistorys;

            GridViewHistory.DataBind();
        }

        //private void BindGridViewCoAuthorAffil(int paperId)
        //{
        //    PaperManager mgr = new PaperManager();
        //    List<PaperCoAuthorAffil> authorList = mgr.GetCoAuthorAffil(paperId);

        //    BindGridView(authorList, GridView1);
        //}

        //private void BindGridView(List<PaperCoAuthorAffil> list, GridView gv)
        //{
        //    //List<int> myList = new List<int>();
        //    //foreach (var affil in affils)
        //    //{
        //    //    if (((PublicationCoAuthorAffil)affil).EndDate > DateTime.Parse(_currentDate))
        //    //    {
        //    //        myList.Add(((PublicationCoAuthorAffil)affil).CoAuthorAffilId);
        //    //    }
        //    //}
        //    //if (myList.Count > 0)
        //    //{
        //    //    PageUtility.SelectRow_GridView(gv, myList);
        //    //}

        //    if (list.Count() > 0)
        //    {
        //        gv.DataSource = list;
        //        gv.DataBind();
        //    }
        //    else
        //    {
        //        //GridView1BindEmpty(); 
        //        var obj = new List<CoAuthorAffil>();
        //        obj.Add(new CoAuthorAffil());
        //        // Bind the DataTable which contain a blank row to the GridView
        //        gv.DataSource = obj;
        //        gv.DataBind();
        //        //int columnsCount = GridView1.Columns.Count;
        //        gv.Rows[0].Cells.Clear();// clear all the cells in the row
        //        //GridView1.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
        //        //GridView1.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

        //        ////You can set the styles here
        //        //GridView1.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        //        //GridView1.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        //        //GridView1.Rows[0].Cells[0].Font.Bold = true;
        //        ////set No Results found to the new added cell
        //        //GridView1.Rows[0].Cells[0].Text = "NO RESULT FOUND!";
        //    }
        //}

        private void BindGridViewBioStat(ICollection<PublicationBioStat> biostats, GridView GridViewBioStat)
        {
            List<int> myList = new List<int>();
            foreach (var pubBiostat in biostats)
            {
                if (((PublicationBioStat)pubBiostat).EndDate > DateTime.Parse(_currentDate))
                {
                    myList.Add(((PublicationBioStat)pubBiostat).BioStats_Id);
                }
            }
            if (myList.Count > 0)
            {
                PageUtility.SelectRow_GridView(GridViewBioStat, myList);
            }
        }

        private void BindGridViewGrant(ICollection<PublicationGrant> grants, GridView GridViewGrant)
        {
            List<int> myList = new List<int>();
            foreach (var pubGrant in grants)
            {
                if (((PublicationGrant)pubGrant).EndDate > DateTime.Parse(_currentDate))
                {
                    myList.Add(((PublicationGrant)pubGrant).GrantAffilId);
                }
            }
            if (myList.Count > 0)
            {
                PageUtility.SelectRow_GridView(GridViewGrant, myList);
            }
        }        

        /// <summary>
        /// Controls the deletion of the Paper History grid.  This method should not be called
        /// as there is no delete control within the grid.  However, if this function is called,
        /// it deletes the specific paper history item from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewHistory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridViewHistory.Rows[e.RowIndex];

            Label lblId = row.FindControl("lblId") as Label;

            int paperId;
            if (lblId != null && Int32.TryParse(lblPaperId.Text, out paperId))
            {
                int historyId;
                if (Int32.TryParse(lblId.Text, out historyId))
                {
                    _repository.DeletePaperHistory(historyId);

                    //List<PaperHistory> paperHistorys = mgr.GetPaperHistory(paperId);
                    List<PaperHistory> paperHistorys = _repository.GetPaperHistory(paperId);
                    BindGridViewHistory(paperHistorys, GridViewHistory);
                }

                //using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                //{
                //    int id = Convert.ToInt32(lblId.Text);
                //    PubHistory hist = db.PubHistories.First(t => t.Id == id);
                //    db.PubHistories.Remove(hist);
                //    db.SaveChanges();

                //    BindGridViewHistory(db, hist.PublicationId, GridViewHistory);
                //}

                if (GridViewHistory.Rows.Count > 0)
                {
                    GridViewHistory.SelectRow(GridViewHistory.Rows.Count - 1);
                    GridViewHistory.SelectedRow.Focus();
                }
                //else
                //{
                //    GridViewBindEmpty();
                //}


            }
        }

        /// <summary>
        /// Get conference information.
        /// </summary>
        /// <returns>New instance of conference with newly entered Conference information.</returns>
        private Conference GetConference()
        {
            DateTime startDate, endDate;           

            Conference conf = null;
            if (DateTime.TryParse(TextBoxStartDate.Text, out startDate)
                && DateTime.TryParse(TextBoxEndDate.Text, out endDate))
            {
                conf = new Conference()
                {
                    ConfName = TextBoxName.Text,
                    ConfLoc = TextBoxLocation.Text,
                    StartDate = startDate,
                    EndDate = endDate
                };
            }

            return conf;
        }

        /// <summary>
        /// Applies the conference information from the database to the paper form.
        /// If there is not conference information, the form is prepared blank.
        /// </summary>
        /// <param name="conf">Instance of conference information.</param>
        private void SetConference(Conference conf)
        {
            if (conf != null && conf.Id > 0)
            {
                TextBoxName.Text = conf.ConfName;
                TextBoxLocation.Text = conf.ConfLoc;
                TextBoxStartDate.Text = conf.StartDate.ToShortDateString();
                TextBoxEndDate.Text = conf.EndDate.ToShortDateString();               
            }
            else
            {
                TextBoxName.Text = string.Empty;
                TextBoxLocation.Text = string.Empty;
                TextBoxStartDate.Text = string.Empty;
                TextBoxEndDate.Text = string.Empty;
            }
        }

        /// <summary>
        /// Get name of journal.
        /// </summary>
        /// <returns>New instance of journal with newly entered journal information.</returns>
        private Journal GetJournal()
        {
            Journal journal = new Journal()
            {
                JournalName = TextBoxJournal.Text                
            };

            return journal;
        }

        /// <summary>
        /// Sets the name of the journal from the database's paper journal information.
        /// If there is no instance of journal information from the database, then
        /// an empty journal section is prepared for the paper form view.
        /// </summary>
        /// <param name="journal">Instance of journal information for paper.</param>
        private void SetJournal(Journal journal)
        {
            if (journal != null && journal.Id > 0)
            {
                TextBoxJournal.Text = journal.JournalName;               
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

        /// <summary>
        /// Get new instance of paper. 
        /// </summary>
        /// <param name="paperId">Exisitng paper ID value if current paper.</param>
        /// <returns>Returns new instance of paper with newly-entered information.</returns>
        private Publication GetPaper(int paperId)
        {
            Publication paper = new Publication()
            {
                Id = paperId,
                PubType = optionAbstract.Checked == true ? 1 : 2,
                ProjectId = Convert.ToInt32(ddlProject.SelectedValue),
                SubmitDate = Convert.ToDateTime(TextBoxSubmitDate.Text),
                Status = GetPubStatus,
                Title = TextBoxTitle.Text,
                Author = TextBoxAuthor.Text,
                Comment = TextBoxComment.Text,
                StartDate = Convert.ToDateTime(DateTime.Now),  //(HiddenFieldCurrentDate.Value),
                EndDate = Convert.ToDateTime("2099-01-01"),
                JounralId = -1,
                Volume = TextBoxVolumeNo.Text,
                Issue = TextBoxIssueNo.Text,
                Page = TextBoxPages.Text,
                DOI = TextBoxDOI.Text,
                PMID = TextBoxPmid.Text,
                PMCID = TextBoxPmcid.Text,
                SampleAccept = TextBoxSampleAccept.Text,
                ConferenceId = -1,
                ParentId = -1
            };

            if (optionsPoster.Checked)
            {
                paper.IsPosterConf = true;
            }
            else if (optionsPresentation.Checked)
            {
                paper.IsPosterConf = false;
            }

            List<int> affiliationList = GetAffiliationId(gvAffil);  //GetSelectedId_Repeater(rptAffiliation);
            string affiliation = string.Join(";", affiliationList);
            paper.Affiliation = affiliation;
            paper.AffiliationOther = txtAffiliationOther.Value;

            return paper;
        }

        /// <summary>
        /// Pre-fills paper form with exisiting paper information from the database.
        /// </summary>
        /// <param name="paper">Paper being referred to.</param>
        /// <param name="affils">Author affiliations (JABSOM-related) of paper.</param>
        /// <param name="paperHistorys">Paper change history of status, journal, or conference info.</param>
        private void SetPaper(Publication paper, List<JabsomAffil> affils, List<PaperHistory> paperHistorys)
        {
            if (paper != null)
            {
                //Journal journal;
                //Conference conf;

                ICollection<PublicationBioStat> biostats = paper.PublicationBioStats;
                ICollection<PublicationGrant> grants = paper.PublicationGrants;

                lblPaperId.Text = paper.Id.ToString();

                optionManuscript.Checked = (paper.PubType == 1) ? false : true;
                //optionManuscript.Disabled = (paper.PubType == 1) ? true : false;
                optionAbstract.Checked = (paper.PubType == 1) ? true : false;
                //optionAbstract.Disabled = (paper.PubType == 1) ? false : true;                             

                ddlProject.SelectedValue = paper.ProjectId.ToString();

                TextBoxTitle.Text = paper.Title;

                TextBoxComment.Text = paper.Comment;

                TextBoxAuthor.Text = paper.Author;

                TextBoxSampleAccept.Text = paper.SampleAccept;

                ResetPubStatus();
                SetPubStatus(paper.Status);

                switch (paper.Status)
                {
                    case 1:
                        TextBoxSubmitDate.Text = paper.SubmitDate.ToShortDateString();
                        break;
                    case 3:
                        TextBoxSubmitDate.Text = paper.PubDate != null ? Convert.ToDateTime(paper.PubDate).ToShortDateString() : "n/a";
                        break;
                    default:
                        TextBoxSubmitDate.Text = paper.AcceptDate != null ? Convert.ToDateTime(paper.AcceptDate).ToShortDateString() : "n/a";
                        break;
                }

                SetJournal(paper.Journal);
                TextBoxVolumeNo.Text = paper.Volume;
                TextBoxIssueNo.Text = paper.Issue;
                TextBoxPages.Text = paper.Page;
                TextBoxDOI.Text = paper.DOI;
                TextBoxPmid.Text = paper.PMID;
                TextBoxPmcid.Text = paper.PMCID;

                SetConference(paper.Conference);
                if (paper.IsPosterConf.HasValue)
                {
                    if (paper.IsPosterConf.Value)
                    {
                        optionsPoster.Checked = true;
                    }
                    else
                    {
                        optionsPresentation.Checked = true;
                    }
                }

                PageUtility.UncheckGrid(GridViewBioStat);
                if (biostats.Count > 0)
                {
                    BindGridViewBioStat(biostats, GridViewBioStat);
                }

                PageUtility.UncheckGrid(GridViewGrant);
                if (grants.Count > 0)
                {
                    BindGridViewGrant(grants, GridViewGrant);
                }

                //if (coAuthorAffils.Count > 0)
                //{
                //    BindGridView(coAuthorAffils, GridView1);
                //}

                if (paperHistorys.Count > 0)
                {
                    BindGridViewHistory(paperHistorys, GridViewHistory);
                }

                txtAffiliationOther.Value = paper.AffiliationOther;

                if (paper.Affiliation.Length > 0)
                {
                    txtAffiliationId.Value = paper.Affiliation;

                    List<int> affiliations = paper.Affiliation.Split(';').Select(int.Parse).ToList();
                                       
                    foreach (RepeaterItem i in rptAffiliation.Items)
                    {
                        CheckBox cb1, cb2;
                        HiddenField hdnValue1, hdnValue2;

                        cb1 = (CheckBox)i.FindControl("FirstchkId");
                            cb2 = (CheckBox)i.FindControl("SecondchkId");

                            hdnValue1 = (HiddenField)i.FindControl("FirstId");
                            hdnValue2 = (HiddenField)i.FindControl("SecondId");

                            if (cb1 != null && hdnValue1 != null)
                            {
                                cb1.Checked = affiliations.Contains(Convert.ToInt32(hdnValue1.Value)) ? true : false;
                            }

                            if (cb2 != null && hdnValue2 != null)
                            {
                                cb2.Checked = affiliations.Contains(Convert.ToInt32(hdnValue2.Value)) ? true : false;
                            }
                    }

                    if (affils.Count > 0)
                    {
                        rptAffil.DataSource = affils;
                        rptAffil.DataBind();

                        gvAffil.DataSource = affils;
                        gvAffil.DataBind();
                    }
                }
            }
        }

        /// <summary>
        /// Opens the paper form with the preparation for new paper entry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearEditForm();

            LoadEditScript();
        }

        /// <summary>
        /// Clears edit Paper form for a fresh new entry.
        /// </summary>
        private void ClearEditForm()
        {
            optionAbstract.Checked = true;
            optionAbstract.Disabled = false;
            optionManuscript.Checked = false;            
            optionManuscript.Disabled = false;

            optionSubmitted.Checked = true;

            ddlProject.ClearSelection();

            PageUtility.UncheckGrid(GridViewBioStat);
            PageUtility.UncheckGrid(GridViewGrant);
            //PageUtility.UncheckGrid(GridViewCoAuthotAffil);

            ResetPubStatus();
            SetPubStatus((int)StatusEnum.SubResub);

            TextBoxSubmitDate.Text = string.Empty;

            TextBoxAuthor.Text = string.Empty;

            TextBoxTitle.Text = string.Empty;
            TextBoxComment.Text = string.Empty;

            lblPaperId.Text = string.Empty;

            SetConference(null);

            SetJournal(null);

            GridViewHistory.DataSource = null;
            GridViewHistory.DataBind();

            TextBoxSampleAccept.Text = string.Empty;

            optionsPoster.Checked = false;
            optionsPresentation.Checked = false;

            //using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            //{
            //    var pubAffils = db.PublicationCoAuthorAffils.                           
            //        Where(p => p.PublicationId == -1);

            //    foreach (var pubAffil in pubAffils)
            //    {
            //        db.PublicationCoAuthorAffils.Remove(pubAffil);
            //    }

            //    db.SaveChanges();
            //}
            //BindGrid(-1);
            //BindGridViewCoAuthorAffil(-1);

            foreach (RepeaterItem i in rptAffiliation.Items)
            {
                CheckBox cb1, cb2;
                cb1 = (CheckBox)i.FindControl("FirstchkId");
                cb2 = (CheckBox)i.FindControl("SecondchkId");
                
                if (cb1 != null)
                {
                    cb1.Checked = false;
                }

                if (cb2 != null)
                {
                    cb2.Checked = false;
                }
            }

            txtAffiliationId.Value = string.Empty;
            txtAffiliationOther.Value = string.Empty;

            BindAffilByIndex(99);
        }       

        /// <summary>
        /// NOTE: Currently not being used.
        /// 
        /// Validates if conference value is true.
        /// </summary>
        /// <returns>False if conference name, location, start or end dates are empty, otherwise true.</returns>
        private bool ValidateConf()
        {
            if (TextBoxName.Text.Equals(string.Empty) || TextBoxLocation.Text.Equals(string.Empty)
             || TextBoxStartDate.Text.Equals(string.Empty) || TextBoxEndDate.Text.Equals(string.Empty))
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// NOTE: Currently not being used.
        /// 
        /// Validates if journal value is true.
        /// </summary>
        /// <returns>False if journal title is empty; otherwise true.</returns>
        private bool ValidateJournal()
        {
            if (TextBoxJournal.Text.Equals(string.Empty) )
                //|| TextBoxVolumeNo.Text.Equals(string.Empty) || TextBoxIssueNo.Text.Equals(string.Empty) || TextBoxPages.Text.Equals(string.Empty))
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Gets Publication status: 
        ///     - Returns 1 if "Submitted" checked,
        ///     - Returns 2 if "Accepted" checked,
        ///     - Returns 3 if "Published" checked,
        ///     - Returns 4 if "Not Accepted" checked.
        /// </summary>
        protected int GetPubStatus
        {
            get
            {
                if (optionSubmitted.Checked) return (int)StatusEnum.SubResub;           //"Submitted"
                else if (optionAccepted.Checked) return (int)StatusEnum.Accepted;       //"Accepted"
                else if (optionPublished.Checked) return (int)StatusEnum.Published;     //"Published";
                else return (int)StatusEnum.NotAccepted;                                //"NotAccepted";              
            }
        }

        /// <summary>
        /// Sets radio button (submitted, accpeted, published, or not accepted) to true if not accepted.
        ///     - If pubStatus = 1, then "Submitted" checked,
        ///     - If pubStatus = 2, then "Accpeted" checked,
        ///     - If pubStatus = 3, then "Published" checked,
        ///     - If pubStatus = 4, then "Not Accepted" checked,
        /// </summary>
        /// <param name="pubStatus"></param>
        private void SetPubStatus(int pubStatus)
        {
            if (pubStatus == (int)StatusEnum.SubResub) optionSubmitted.Checked = true;
            else if (pubStatus == (int)StatusEnum.Accepted) optionAccepted.Checked = true;
            else if (pubStatus == (int)StatusEnum.Published) optionPublished.Checked = true;
            else optionNotAccepted.Checked = true;
        }

        /// <summary>
        /// Clears pre-filled publication status, unmarking all radio buttons by default.
        /// </summary>
        void ResetPubStatus()
        {
            optionSubmitted.Checked = false;
            optionAccepted.Checked = false;
            optionPublished.Checked = false;
            optionNotAccepted.Checked = false;
        }

        /// <summary>
        /// Loads Javascript pop-up screen for editable paper form.
        /// </summary>
        private void LoadEditScript()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }

        //private void BindGrid(int paperId)
        //{
        //    using (ProjectTrackerContainer db = new ProjectTrackerContainer())
        //    {
        //        DateTime currentDate = DateTime.Parse(_currentDate);

        //        var query = from a in db.PublicationCoAuthorAffils
        //                    join p in db.CoAuthorAffils on a.CoAuthorAffilId equals p.Id
        //                    where a.PublicationId == paperId && a.EndDate > currentDate
        //                    select new { a.Id, p.Name, p.Type };

        //        if (query.Count() > 0)
        //        {
        //            GridView1.DataSource = query.ToList();
        //            GridView1.DataBind();
        //        }
        //        else
        //        {
        //            //GridView1BindEmpty(); 
        //            var obj = new List<CoAuthorAffil>();
        //            obj.Add(new CoAuthorAffil());
        //            // Bind the DataTable which contain a blank row to the GridView
        //            GridView1.DataSource = obj;
        //            GridView1.DataBind();
        //            //int columnsCount = GridView1.Columns.Count;
        //            GridView1.Rows[0].Cells.Clear();// clear all the cells in the row
        //            //GridView1.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
        //            //GridView1.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

        //            ////You can set the styles here
        //            //GridView1.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        //            //GridView1.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        //            //GridView1.Rows[0].Cells[0].Font.Bold = true;
        //            ////set No Results found to the new added cell
        //            //GridView1.Rows[0].Cells[0].Text = "NO RESULT FOUND!";
        //        }
        //    }
        //}

        //private void GridView1BindEmpty()
        //{
        //    DataTable dt = new DataTable("emptyTable");

        //    dt.Columns.Add("Id", System.Type.GetType("System.String"));
        //    dt.Columns.Add("Name", System.Type.GetType("System.String"));
        //    dt.Columns.Add("Type", System.Type.GetType("System.String"));
        //    dt.Columns.Add("", System.Type.GetType("System.String"));

        //    DataRow row = dt.NewRow();
        //    //row[0] = "";
        //    //row[1] = "";
        //    //row[2] = "";
        //    //row[3] = "";
        //    //row[4] = "";
        //    //row[5] = "";

        //    dt.Rows.Add(row);
        //    GridView1.DataSource = dt;
        //    GridView1.DataBind();
        //}

        //protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "InsertNew")
        //    {             
        //        GridViewRow row = GridView1.FooterRow;

        //        TextBox txtName = row.FindControl("txtNameNew") as TextBox;
        //        TextBox txtType = row.FindControl("txtTypeNew") as TextBox;

        //        if (txtName != null)
        //        {
        //            try
        //            {
        //                PaperManager mgr = new PaperManager();

        //                mgr.AddCoAuthorAffil(PaperId, txtName.Text, txtType.Text, Creator);

        //                List<PaperCoAuthorAffil> authorList = mgr.GetCoAuthorAffil(PaperId);

        //                BindGridView(mgr.GetCoAuthorAffil(PaperId), GridView1);

        //                //BindGrid(paperId);
        //                ////BindGridViewPaperAll();
        //            }
        //            catch (EntityException ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //    }

        //    if (e.CommandName == "CancelNew")
        //    {
        //        GridView1.EditIndex = -1;
        //        BindGridViewCoAuthorAffil(PaperId);
        //    }
        //}

        //protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    //GridView1.EditIndex = e.NewEditIndex;
        //    ////int paperId;
        //    ////Int32.TryParse(lblPaperId.Text, out paperId);
        //    ////BindGrid(paperId);
        //    //BindGridViewCoAuthorAffil(PaperId);
        //}

        //protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    GridView1.EditIndex = -1;
        //    //int paperId;
        //    //Int32.TryParse(lblPaperId.Text, out paperId);
        //    //BindGrid(paperId);
        //    BindGridViewCoAuthorAffil(PaperId);
        //}

        //protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    GridViewRow row = GridView1.Rows[e.RowIndex];

        //    Label lblId = row.FindControl("lblId") as Label;
        //    TextBox txtName = row.FindControl("txtName") as TextBox;
        //    TextBox txtType = row.FindControl("txtType") as TextBox;

        //    //int paperId;
        //    //Int32.TryParse(lblPaperId.Text, out paperId);

        //    if (txtName != null)
        //    {
        //        PaperManager mgr = new PaperManager();

        //        int paperAuthorAffilId;
        //        Int32.TryParse(lblId.Text, out paperAuthorAffilId);

        //        mgr.UpdateCoAuthorAffil(paperAuthorAffilId, txtName.Text, txtType.Text, Creator);

        //        GridView1.EditIndex = -1;

        //        BindGridView(mgr.GetCoAuthorAffil(PaperId), GridView1);
        //        //BindGrid(Convert.ToInt32(lblPaperId.Text));
        //        //BindGridViewPaperAll();
        //    }
        //}

        //protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    GridViewRow row = GridView1.Rows[e.RowIndex];

        //    //int paperId;
        //    //Int32.TryParse(lblPaperId.Text, out paperId);

        //    Label lblId = row.FindControl("lblId") as Label;

        //    if (lblId != null)
        //    {
        //        int paperAuthorAffilId;
        //        Int32.TryParse(lblId.Text, out paperAuthorAffilId);

        //        PaperManager mgr = new PaperManager();

        //        if (paperAuthorAffilId > 0)
        //        {                    
        //            mgr.DeleteCoAuthorAffil(paperAuthorAffilId, Creator);
        //        }                

        //        //if (GridViewHistory.Rows.Count > 0)
        //        //{
        //        //    GridViewHistory.SelectRow(GridViewHistory.Rows.Count - 1);
        //        //    GridViewHistory.SelectedRow.Focus();
        //        //}
        //        ////else
        //        ////{
        //        ////    GridViewBindEmpty();
        //        ////}

        //        BindGridView(mgr.GetCoAuthorAffil(PaperId), GridView1);

        //        //BindGrid(Convert.ToInt32(lblPaperId.Text));
        //        //BindGridViewPaperAll();
        //    }
        //}

            /// <summary>
            /// Action taken when row is being deleted in the affiliations section of the paper form.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
        protected void gvAffil_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvAffil.Rows[e.RowIndex];

            if (row != null)
            {
                BindAffilByIndex(e.RowIndex);
            }
        }

        /// <summary>
        /// Takes index of row of affiliation and binds and adds row to
        /// data table.
        /// 
        /// Adds new row if a negative number or a new row is indicated.
        /// </summary>
        /// <param name="rowIndex">Index of row of affiliation.</param>
        private void BindAffilByIndex(int rowIndex)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");

            if (rowIndex != 99)
            {
                foreach (GridViewRow row in gvAffil.Rows)
                {
                    if (row.RowIndex != rowIndex)
                    {
                        TextBox lblId = row.FindControl("lblId") as TextBox;
                        TextBox txtName = row.FindControl("txtTitle") as TextBox;

                        DataRow dr = dt.NewRow();
                        dr["Id"] = Request.Form[lblId.UniqueID]; //lblId.Text;
                        dr["Name"] = txtName.Text;

                        dt.Rows.Add(dr);
                    }
                }
            }

            if (rowIndex < 0 || dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }    

            gvAffil.DataSource = dt;
            gvAffil.DataBind();
        }

        /// <summary>
        /// Adds new affiliation row when the "add new affiliation" button is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddAffil_Click(object sender, EventArgs e)
        {
            BindAffilByIndex(-1);
        }

        //protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        //{

        //}       

        //private void WriteInvest(ProjectTrackerContainer context, BioStat biostat)
        //{
        //    var result = context.Entry(biostat).GetValidationResult();
        //    if (!result.IsValid)
        //    {
        //        lblMsg.Text = result.ValidationErrors.First().ErrorMessage;
        //    }
        //    else
        //    {
        //        context.SaveChanges();
        //        lblMsg.Text = "Saved successfully.";
        //    }
        //}

        //protected void GridViewCoAuthotAffil_PreRender(object sender, EventArgs e)
        //{
        //    MergeGridviewRows(GridViewCoAuthotAffil);
        //}

        //private void MergeGridviewRows(GridView gridView)
        //{
        //    for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        //    {
        //        GridViewRow row = gridView.Rows[rowIndex];
        //        GridViewRow previousRow = gridView.Rows[rowIndex + 1];

        //        string s1 = ((Label)row.Cells[0].FindControl("lblAffilType")).Text;
        //        string s2 = ((Label)previousRow.Cells[0].FindControl("lblAffilType")).Text;
        //        if (s1 == s2)
        //        {
        //            row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 : previousRow.Cells[0].RowSpan + 1;

        //            previousRow.Cells[0].Visible = false;
        //        }
        //    } 
        //}

        /// <summary>
        /// NOTE: Currently not being used.
        /// 
        /// Old method of combinding affiliations table to paper form.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="rpt"></param>
        private void BindTable(Dictionary<int, string> collection, Repeater rpt)
        {
            DataTable dt = new DataTable("tblRpt");

            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));

            var query = collection.ToArray();

            for (int i = 0; i < query.Length; i += 2)
            {
                DataRow dr = dt.NewRow();

                dr[0] = query[i].Key;
                dr[1] = query[i].Value;

                if (i < query.Length - 1)
                {
                    dr[2] = query[i + 1].Key;
                    dr[3] = query[i + 1].Value;
                }
                else
                {
                    dr[2] = 0;
                    dr[3] = "";
                }

                dt.Rows.Add(dr);
            }

            rpt.DataSource = dt;
            rpt.DataBind();
        }

        /// <summary>
        /// Primarily used to bind affiliation table to paper form.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="rpt"></param>
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
        
        public string Creator 
        { 
            get 
            { 
                return Page.User.Identity.Name; 
            } 
        }

        public int PaperId
        {
            get
            {
                int paperId;
                Int32.TryParse(lblPaperId.Text, out paperId);

                paperId = paperId == 0 ? -1 : paperId;

                return paperId;
            }
        }
    }
}