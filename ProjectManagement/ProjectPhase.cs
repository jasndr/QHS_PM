//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectManagement
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectPhase
    {
        public ProjectPhase()
        {
            this.TimeEntries = new HashSet<TimeEntry>();
        }
    
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string Title { get; set; }
        public Nullable<decimal> MsHrs { get; set; }
        public Nullable<decimal> PhdHrs { get; set; }
        public string Creator { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Project Projects { get; set; }
        public virtual ICollection<TimeEntry> TimeEntries { get; set; }
        public virtual Project2 Project2 { get; set; }
    }
}
