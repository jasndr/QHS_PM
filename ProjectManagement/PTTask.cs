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
    
    public partial class PTTask
    {
        public PTTask()
        {
            this.TaskHistories = new HashSet<TaskHistory>();
        }
    
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int BiostatId { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public string TaskStatus { get; set; }
        public string TaskPriority { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string Creator { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual ICollection<TaskHistory> TaskHistories { get; set; }
        public virtual BioStat BioStat { get; set; }
    }
}
