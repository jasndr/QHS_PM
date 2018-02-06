using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ProjectManagement.Dtos;

namespace ProjectManagement.Model
{
    public class ProjectTrackerRepository : IDisposable, IProjectTrackerRepository
    {
        DateTime _endDate = DateTime.Parse("2099-01-01");

        private ProjectTrackerContainer _dbContext;
        public ProjectTrackerRepository()
        {
            _dbContext = new ProjectTrackerContainer();
        }       
         
        #region Cleanup
        ~ProjectTrackerRepository()
        {          
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }
        
        protected virtual void Dispose(bool isDisposing)
        {			    
            if (isDisposing) GC.SuppressFinalize(this);
            
            _dbContext.Dispose();
        }
        #endregion

        #region Affiliation
        public List<JabsomAffil> GetAffiliations(string affiliationId)
        {
            int id = 0;
            Int32.TryParse(affiliationId, out id);

            var query = _dbContext.JabsomAffils
                        .Where(a => a.Type != "Other" && a.Type != "Degree" && a.Type != "Unknown" && a.Name != "Other");                        

            if (id > 0)
            {
                query = query.Where(q => q.Id == id);
            }

            return query.OrderBy(a => a.Type)
                        .ThenBy(a => a.Name)
                        .ToList();
        }

        public JabsomAffil GetAffiliationByName(string affilName)
        {
            return _dbContext.JabsomAffils.FirstOrDefault(a => a.Name == affilName);
        }

        public int AddAffiliation(JabsomAffil newAffil)
        {
            _dbContext.JabsomAffils.Add(newAffil);
            _dbContext.SaveChanges();

            return newAffil.Id;
        }

        public bool UpdateAffiliation(JabsomAffil affilEdit)
        {
            var affil = _dbContext.JabsomAffils.FirstOrDefault(a => a.Id == affilEdit.Id);

            if (affil.Name != affilEdit.Name)
            {
                affil.Name = affilEdit.Name;
                _dbContext.SaveChanges();

                return true;
            }
            else
                return false;
        }

        #endregion Affiliation


        #region Paper
        internal DataTable GetPaperAll(int pubType, int biostatId)
        {
            DataTable dt = new DataTable("paperTable");

            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("PubType", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Title", System.Type.GetType("System.String"));
            dt.Columns.Add("Author", System.Type.GetType("System.String"));
            dt.Columns.Add("Status", System.Type.GetType("System.String"));
            dt.Columns.Add("PaperDate", System.Type.GetType("System.String"));

            var query = _dbContext.Publications
                        .Where(p => p.Id > 0);

            if (biostatId > 0)
            {
                query = _dbContext.PublicationBioStats
                        .Join(_dbContext.Publications, b => b.Publications_Id, p => p.Id, (b, p) => new { b, p })
                        .Where(i => i.b.BioStats_Id == biostatId)
                        .Select(a => a.p);
            }

            if (pubType > 0)
            {
                query = query.Where(p => p.PubType == pubType);
            }

            foreach (var p in query.OrderByDescending(p => p.Id).ToList())
            {
                DataRow row = dt.NewRow();

                row[0] = p.Id;
                row[1] = p.PubType;
                row[2] = p.Title;
                row[3] = p.Author;

                switch (p.Status)
                {
                    case 1:
                        row[4] = "Submitted";
                        row[5] = p.SubmitDate.ToShortDateString();
                        break;
                    case 2:
                        row[4] = "Accepted";
                        row[5] = p.AcceptDate != null ? Convert.ToDateTime(p.AcceptDate).ToShortDateString() : "n/a";
                        break;
                    case 3:
                        row[4] = "Published";
                        row[5] = p.PubDate != null ? Convert.ToDateTime(p.PubDate).ToShortDateString() : "n/a";
                        break;
                    default:
                        row[4] = "Not Accepted";
                        row[5] = p.AcceptDate != null ? Convert.ToDateTime(p.AcceptDate).ToShortDateString() : "n/a";
                        break;
                }

                dt.Rows.Add(row);
            }

            return dt;
        }       

        internal IDictionary<string, IDictionary<int, string>> GetPaperControlSource()
        {
            IDictionary<string, IDictionary<int, string>> controlSource = new Dictionary<string, IDictionary<int, string>>();
            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

            dropDownSource = _dbContext.PublishTypes
                                .Where(p => p.Id != "-1")
                                .OrderBy(b => b.Id)
                                .ToDictionary(c => int.Parse(c.Id), c => c.Name);
            controlSource.Add("ddlPubType", dropDownSource);

            dropDownSource = _dbContext.Project2
                            .OrderByDescending(d => d.Id)
                            .Select(x => new { x.Id, FullName = x.Id + " " + x.Title })
                            .ToDictionary(c => c.Id, c => c.FullName);
            controlSource.Add("ddlProject", dropDownSource);

            dropDownSource = _dbContext.BioStats
                            .Where(b => b.EndDate > DateTime.Now && b.Id > 0 && b.Id != 99)
                            .OrderBy(b => b.Name)
                            .ToDictionary(c => c.Id, c => c.Name);
            controlSource.Add("ddlBiostat", dropDownSource);

            dropDownSource = _dbContext.GrantAffils
                            .Where(d => d.IsPaperGrant == true)
                            .OrderBy(d => d.Id)
                            .ToDictionary(g => g.Id, g => g.GrantAffilName);
            controlSource.Add("GridViewGrant", dropDownSource);
                       
            dropDownSource = _dbContext.JabsomAffils
                            .Where(a => a.Type != "Other" && a.Type != "Degree" && a.Type != "Unknown" && a.Name != "Other")
                            .OrderBy(a => a.Name)
                            .ToDictionary(c => c.Id, c => c.Name);
            controlSource.Add("rptAffiliation", dropDownSource);

            return controlSource;
        }        

        internal Publication GetPaper(int paperId)
        {
            var paper = _dbContext.Publications
                        .Include("Journal")
                        .Include("Conference")
                        .Include("PublicationGrants")
                        .Include("PublicationBioStats")
                //.Select(p => new 
                //            {
                //                p,
                //                PublicationBioStats = p.PublicationBioStats.Where(b => b.EndDate == _endDate)                                
                //            }
                //        )
                //.AsEnumerable()
                //.Select(x => x.p)
                        .FirstOrDefault(i => i.Id == paperId);

            paper.PublicationBioStats = paper.PublicationBioStats.Where(b => b.EndDate > DateTime.Now).ToList();

            //paper.Journal = db.Journals.FirstOrDefault(j => j.Id == paper.JounralId);
            //paper.Conference = db.Conferences.FirstOrDefault(c => c.Id == paper.ConferenceId);           

            //paper.PublicationCoAuthorAffils = paper.PublicationCoAuthorAffils;
            //paper.PublicationGrants = paper.PublicationGrants;

            return paper;
        }

        internal List<PaperHistory> GetPaperHistory(int paperId)
        {
            List<PaperHistory> paperHistoryList = new List<PaperHistory>();

            var query = from h in _dbContext.PubHistories
                        join p in _dbContext.Publications on h.PublicationId equals p.Id
                        join s in _dbContext.PublishStatus on h.StatusId equals s.Id
                        join j in _dbContext.Journals on h.JournalId equals j.Id
                        join c in _dbContext.Conferences on h.ConfId equals c.Id
                        where p.Id == paperId
                        select new PaperHistory { Id = h.Id, PubType = h.PubType, PaperDate = h.PaperDate, Name = s.Name, JournalName = j.JournalName, ConfName = c.ConfName };

            if (query.Count() > 0)
            {
                paperHistoryList = query.ToList();
            }

            return paperHistoryList;
        }

        internal List<JabsomAffil> GetCoAuthorAffil2(List<int> affiliationList)
        {
            List<JabsomAffil> lst = new List<JabsomAffil>();

            if (affiliationList.Count > 0)
            {
                var query = _dbContext.JabsomAffils
                            .Where(a => affiliationList.Contains(a.Id))
                            .ToList();

                if (query != null && query.Count() > 0)
                {
                    lst = query;
                }
            }

            return lst;
        }

        internal void DeletePaperHistory(int historyId)
        {
            PubHistory paperHistory = _dbContext.PubHistories.First(t => t.Id == historyId);
            _dbContext.PubHistories.Remove(paperHistory);
            _dbContext.SaveChanges();
        }

        private void UpdateGrants(Publication paper, List<int> newGrantList)
        {
            ICollection<PublicationGrant> prevGrants = paper.PublicationGrants
                                                        .Where(b => b.EndDate > DateTime.Now).ToList();

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
                _dbContext.PublicationGrants.Remove(pg);
            }
            foreach (var newGrantId in newNotPrevGrantList)
            {
                paper.PublicationGrants.Add(new PublicationGrant()
                {
                    PublicationId = paper.Id,
                    GrantAffilId = newGrantId,
                    StartDate = DateTime.Now,
                    EndDate = _endDate
                });
            }
        }

        private void UpdateBiostats(Publication paper, List<int> newMemberList)
        {
            ICollection<PublicationBioStat> prevBiostats = paper.PublicationBioStats
                                                            .Where(b => b.EndDate > DateTime.Now).ToList();

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
                paper.PublicationBioStats.Last(b => b.BioStats_Id == expiredId).EndDate = DateTime.Now;
            }
            foreach (var newMemberId in newNotPrevList)
            {
                paper.PublicationBioStats.Add(new PublicationBioStat()
                {
                    Publications_Id = paper.Id,
                    BioStats_Id = newMemberId,
                    StartDate = DateTime.Now,
                    EndDate = _endDate
                });
            }
        }

        private void AddNewPaperHistory(Publication paper, string userName)
        {
            PubHistory pubHistory = new PubHistory()
            {
                PublicationId = paper.Id,
                PubType = paper.PubType,
                StatusId = paper.Status,
                PaperDate = paper.SubmitDate,
                JournalId = paper.JounralId,
                ConfId = paper.ConferenceId,
                Creator = userName,
                CreateDate = DateTime.Now
            };

            switch (paper.Status)
            {
                case 1:
                    pubHistory.PaperDate = paper.SubmitDate;
                    break;
                case 3:
                    pubHistory.PaperDate = paper.PubDate != null ? (DateTime)paper.PubDate : Convert.ToDateTime("1900-01-01");
                    break;
                default:
                    pubHistory.PaperDate = paper.AcceptDate != null ? (DateTime)paper.AcceptDate : Convert.ToDateTime("1900-01-01");
                    break;
            }

            _dbContext.PubHistories.Add(pubHistory);
            _dbContext.SaveChanges(); 
        }        

        private int GetJournalId(string journalName)
        {
            int journalId = -1;

            if (!journalName.Equals(string.Empty))
            {
                var journalExists = _dbContext.Journals
                                    .Any(j => j.JournalName == journalName);

                if (journalExists)
                {
                    var journal = _dbContext.Journals
                              .First(j => j.JournalName == journalName);

                    journalId = journal.Id;
                }
                else
                {
                    Journal newJournal = new Journal()
                    {
                        JournalName = journalName
                    };

                    _dbContext.Journals.Add(newJournal);

                    _dbContext.SaveChanges();

                    journalId = newJournal.Id;
                }
            }

            return journalId;
        }

        private int GetConfId(Conference conf)
        {
            int confId = -1;

            if (conf != null)
            {
                var _conf = _dbContext.Conferences
                            .FirstOrDefault(c => c.ConfName == conf.ConfName && c.ConfLoc == conf.ConfLoc
                                            && c.StartDate == conf.StartDate && c.EndDate == conf.EndDate);
                if (_conf != null)
                {
                    confId = _conf.Id;
                }
                else
                {
                    Conference newConf = new Conference()
                    {
                        ConfName = conf.ConfName,
                        ConfLoc = conf.ConfLoc,
                        StartDate = conf.StartDate,
                        EndDate = conf.EndDate
                        //IsPoster = optionsPoster.Checked                                            
                    };

                    _dbContext.Conferences.Add(newConf);

                    _dbContext.SaveChanges();

                    confId = newConf.Id;
                }
            }

            return confId;
        }
        

        internal void CreatePaper(PaperDto paperDto)
        {
            var newPaper = paperDto.Publication;
            var newJournal = paperDto.Journal;
            var newConf = paperDto.Conference;
            var newMemberList = paperDto.Members;
            var newGrantList = paperDto.Grants;

            int journalId = GetJournalId(newJournal.JournalName);
            int confId = GetConfId(newConf);

            newPaper.ConferenceId = confId;
            newPaper.JounralId = journalId;

            _dbContext.Publications.Add(newPaper);

            _dbContext.SaveChanges();

            foreach (var newMemberId in newMemberList)
            {
                newPaper.PublicationBioStats.Add(new PublicationBioStat()
                {
                    Publications_Id = newPaper.Id,
                    BioStats_Id = newMemberId,
                    StartDate = DateTime.Now,
                    EndDate = _endDate
                });
            }

            foreach (var newGrantId in newGrantList)
            {
                newPaper.PublicationGrants.Add(new PublicationGrant()
                {
                    PublicationId = newPaper.Id,
                    GrantAffilId = newGrantId,
                    StartDate = DateTime.Now,
                    EndDate = _endDate
                });
            }

            _dbContext.SaveChanges();
        }

        internal void UpdatePaper(PaperDto paperDto)
        {
            var newPaper = paperDto.Publication;
            var newJournal = paperDto.Journal;
            var newConf = paperDto.Conference;
            var newMemberList = paperDto.Members;
            var newGrantList = paperDto.Grants;
            int pubType = newPaper.PubType;
            var creator = paperDto.Creator;

            Publication paper = _dbContext.Publications.FirstOrDefault(p => p.Id == newPaper.Id);
            if (paper != null)
            {
                DateTime currentDate = DateTime.Now;

                DateTime submitDate = paper.SubmitDate;
                if (newPaper.Status == 1)
                {
                    submitDate = newPaper.SubmitDate;
                }

                int newJournalId = GetJournalId(newJournal.JournalName);

                int newConfId = -1;
                if (pubType == 1)
                {
                    newConfId = GetConfId(newConf);
                    paper.ConferenceId = newConfId;
                    paper.IsPosterConf = newPaper.IsPosterConf;
                }

                if ((paper.Status != newPaper.Status) ||
                    (paper.JounralId > 0 && (paper.JounralId != newJournalId || paper.SubmitDate != submitDate)) ||
                    (pubType == 1 && paper.ConferenceId > 0 && paper.ConferenceId != newConfId))
                {
                    AddNewPaperHistory(paper, creator);
                }

                paper.PubType = pubType;

                paper.ProjectId = newPaper.ProjectId;

                paper.Title = newPaper.Title;

                paper.Author = newPaper.Author;
                paper.Affiliation = newPaper.Affiliation;
                paper.AffiliationOther = newPaper.AffiliationOther;

                paper.Comment = newPaper.Comment;

                paper.Status = newPaper.Status;

                switch (paper.Status)
                {
                    case 1:
                        paper.SubmitDate = newPaper.SubmitDate;
                        break;
                    case 3:
                        paper.PubDate = newPaper.SubmitDate;
                        break;
                    default:
                        paper.AcceptDate = newPaper.SubmitDate;
                        break;
                }

                UpdateBiostats(paper, newMemberList);

                UpdateGrants(paper, newGrantList);

                paper.JounralId = newJournalId;
                paper.Volume = newPaper.Volume;
                paper.Issue = newPaper.Issue;
                paper.Page = newPaper.Page;
                paper.DOI = newPaper.DOI;
                paper.PMID = newPaper.PMID;
                paper.PMCID = newPaper.PMCID;
                paper.SampleAccept = newPaper.SampleAccept;

                _dbContext.SaveChanges();
            }
        }

        #endregion Paper
        
    }
}