using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace ProjectManagement.Model
{
    public sealed class GrantManager
    {
        private static readonly GrantManager mgr = new GrantManager();

        static GrantManager()
        {
        }

        private GrantManager()
        {
        }

        public static GrantManager Mgr
        {
            get
            {
                return mgr;
            }
        }

        internal IDictionary<string, IDictionary<int, string>> GetControlSource()
        {
            IDictionary<string, IDictionary<int, string>> controlSource = new Dictionary<string, IDictionary<int, string>>();
            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                //ddlInvestor
                dropDownSource = db.Invests
                                .OrderBy(d => d.FirstName).ThenBy(l => l.LastName)
                                .Select(x => new { x.Id, Name = x.FirstName + " " + x.LastName })
                                .ToDictionary(c => c.Id, c => c.Name);
                controlSource.Add("ddlInvestor", dropDownSource);

                dropDownSource = db.GrantFundStatus
                                .Where(p => p.IsGrant)
                                .OrderBy(b => b.Id)
                                .ToDictionary(c => c.Id, c => c.StatusValue);
                controlSource.Add("ddlGrantStatus", dropDownSource);

                dropDownSource = db.GrantFundStatus
                                .Where(p => p.IsFund)
                                .OrderBy(b => b.Id)
                                .ToDictionary(c => c.Id, c => c.StatusValue);
                controlSource.Add("ddlFundStatus", dropDownSource);

                dropDownSource = db.Project2
                                .OrderByDescending(d => d.Id)
                                .Select(x => new { x.Id, FullName = (x.Id + " " + x.Title).Substring(0, 118) })
                                .ToDictionary(c => c.Id, c => c.FullName);
                controlSource.Add("ddlProject", dropDownSource);

                dropDownSource = db.BioStats
                                //.Where(b => b.EndDate > DateTime.Now)
                                .OrderByDescending(b => b.Id)
                                .ToDictionary(c => c.Id, c => c.Name);
                controlSource.Add("ddlBiostat", dropDownSource);               

            }

            //dropDownSource.Clear();
            dropDownSource = new Dictionary<int, string>(); 
            dropDownSource.Add(new KeyValuePair<int, string>(1, "External"));
            dropDownSource.Add(new KeyValuePair<int, string>(2, "Internal"));
            controlSource.Add("ddlInternal", dropDownSource);

            return controlSource;
        }

        internal DataTable GetGrantAll(int piId, int biostatId, int fundStatusId, int exinternal)
        {
            DataTable dt = new DataTable("grantTable");

            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("PI", System.Type.GetType("System.String"));
            dt.Columns.Add("ProjectId", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Biostat", System.Type.GetType("System.String"));
            dt.Columns.Add("GrantTitle", System.Type.GetType("System.String"));
            dt.Columns.Add("GrantStatus", System.Type.GetType("System.String"));
            dt.Columns.Add("FundStatus", System.Type.GetType("System.String"));
            dt.Columns.Add("FundDate", System.Type.GetType("System.String"));
            dt.Columns.Add("InitDate", System.Type.GetType("System.String"));

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                //var pi, biostat, fundStatus;
                string piName = string.Empty, biostatName = string.Empty, fundStatusName = string.Empty;
                bool isInternal = false;               

                var query = db.ViewGrants.Where(g => g.Id > 0);

                if (piId > 0)
                {
                    var pi = db.Invests.FirstOrDefault(p => p.Id == piId);
                    if (pi != null)
                    {
                        piName = pi.FirstName + ' ' + pi.LastName;
                        query = query.Where(q => q.PI.Contains(piName));
                    }
                    
                }

                if (biostatId > 0)
                {
                    var biostat = db.BioStats.FirstOrDefault(b => b.Id == biostatId);
                    if (biostat != null)
                    {
                        biostatName = biostat.Name;
                        query = query.Where(q => q.Biostat.Contains(biostatName));
                    }
                }

                if (fundStatusId != 0)
                {
                    var fundStatus = db.GrantFundStatus.FirstOrDefault(f => f.Id == fundStatusId);
                    if (fundStatus != null)
                    {
                        fundStatusName = fundStatus.StatusValue;
                        query = query.Where(q => q.FundStatus == fundStatusName);
                    }
                }

                if (exinternal > 0)
                {
                    isInternal = exinternal == 1 ? false : true;
                    query = query.Where(q => q.IsInternal == isInternal);
                }

                foreach (var p in query.OrderByDescending(p => p.Id).ToList())
                {
                    DataRow row = dt.NewRow();

                    row["Id"] = p.Id;
                    row["PI"] = p.PI;
                    row["Biostat"] = p.Biostat;
                    row["GrantTitle"] = p.GrantTitle;
                    row["GrantStatus"] = p.GrantStatus;
                    row["FundStatus"] = p.FundStatus;
                    row["FundDate"] = p.FundDate;
                    row["InitDate"] = p.InitDate;
                    row["ProjectId"] = p.ProjectId;

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        internal Grant GetGrantById(int grantId)
        {
            Grant grant;

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                grant = db.Grants.FirstOrDefault(g => g.Id == grantId);

                if (grant != null)
                {
                    //ICollection<GrantPI> grantPIList;

                    if (grant.ProjectId > 0)
                    {
                        var project = db.Project2.FirstOrDefault(p => p.Id == grant.ProjectId);

                        if (project != null)
                        {
                            List<GrantPI> lstPI = new List<GrantPI>();
                            GrantPI projectPI = new GrantPI();
                            projectPI.Invest = db.Invests.FirstOrDefault(i => i.Id == project.PIId);
                            lstPI.Add(projectPI);
                            grant.GrantPIs.Clear();
                            grant.GrantPIs = lstPI;
                        }
                    }
                    else
                    {
                        foreach (var grantPi in grant.GrantPIs)
                        {
                            if (grantPi.Invest == null)
                            {
                                grantPi.Invest = db.Invests.First(i => i.Id == grantPi.PiId);
                            }
                        }
                    }
                    
                    grant.GrantPIs = grant.GrantPIs.ToList();

                    //foreach (var grantBiostat in grant.GrantBiostats)
                    //{
                    //    if (grantBiostat.BioStat == null)
                    //    {
                    //        grantBiostat.BioStat = db.BioStats.First(b => b.Id == grantBiostat.BiostatId);
                    //    }
                    //}
                    grant.GrantBiostats = grant.GrantBiostats.ToList();
                }
            }

            return grant;
        }

        internal void DeleteGrantBiostatById(int grantBiostatId)
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                GrantBiostat gb = db.GrantBiostats.FirstOrDefault(i => i.Id == grantBiostatId);

                if (gb != null)
                {
                    db.GrantBiostats.Remove(gb);
                    db.SaveChanges();
                }
            }
        }

        internal int SaveGrant(int grantId, Grant newGrant)
        {
            int gid = 0;
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                if (grantId > 0) //update
                {
                    var orgGrant = db.Grants.FirstOrDefault(g => g.Id == grantId);

                    if (orgGrant != null)
                    {
                        db.Entry(orgGrant).CurrentValues.SetValues(newGrant);                        
                    }
                }
                else //new
                {
                    db.Grants.Add(newGrant);
                }

                db.SaveChanges();
                gid = newGrant.Id;
            }

            return gid;
        }

        internal void SaveGrantBiostat(int grantId, List<GrantBiostat> lstBiostat)
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                //var grantBiostats = db.GrantBiostats.Where(b => b.GrantId == grantId);
                
                //List<int> lstNewBiostat = new List<int>();
                
                foreach (var biostat in lstBiostat)
                {
                    var biostatDB = db.GrantBiostats.FirstOrDefault(g => g.Id == biostat.Id);
                        //(g => g.GrantId == biostat.GrantId && g.BiostatId == biostat.BiostatId);

                    if (biostatDB != null)
                    {
                        biostatDB.Pct = biostat.Pct;
                        biostatDB.Fee = biostat.Fee;
                        biostatDB.Year = biostat.Year;
                        biostatDB.Note = biostat.Note;
                    }
                    else
                    {
                        db.GrantBiostats.Add(biostat);
                    }

                    //lstNewBiostat.Add(biostat.BiostatId);
                }

                //foreach (var grantBiostat in grantBiostats.ToList())
                //{
                //    if (lstNewBiostat.IndexOf(grantBiostat.BiostatId) == -1)
                //    {
                //        db.GrantBiostats.Remove(grantBiostat);
                //    }
                //}

                db.SaveChanges();
            }
        }

        internal void SaveGrantPI(int grantId, string[] piNames, string creator, DateTime dateTimeNow)
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var prevPIs = db.GrantPIs.Where(p => p.GrantId == grantId).ToList();
                List<string> prevPILst = new List<string>();

                foreach(var pi in prevPIs)
                {
                    string name = pi.Invest.FirstName + ' ' + pi.Invest.LastName;
                    prevPILst.Add(name);

                    if (Array.IndexOf(piNames, name) < 0)   //(!piNames.All(name.Contains))
                    {
                        db.GrantPIs.Remove(pi);
                    }
                }

                foreach (string piName in piNames)
                {
                    string s = piName.Trim();
                    if (s.Length > 1)
                    {                    
                        if (Array.IndexOf(prevPILst.ToArray(), piName.Trim()) < 0 )//(!prevPILst.All(piName.Contains))
                        {
                            string[] names = s.Split(' ');
                            string firstName = names[0];
                            string lastName = names.Length > 1 ? names[1] : string.Empty;

                            var pi = db.Invests.FirstOrDefault(i => i.FirstName == firstName && i.LastName == lastName);
                            if (pi != null)
                            {
                                GrantPI newPI = new GrantPI() { GrantId = grantId, PiId = pi.Id, Creator = creator, CreateDate = dateTimeNow };
                                db.GrantPIs.Add(newPI);
                            }
                        }                        
                    }
                }

                db.SaveChanges();
            }
        }
    }
}