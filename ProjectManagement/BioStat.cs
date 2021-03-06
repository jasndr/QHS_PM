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
    
    public partial class BioStat
    {
        public BioStat()
        {
            this.ProjectBioStats = new HashSet<ProjectBioStat>();
            this.PublicationBioStats = new HashSet<PublicationBioStat>();
            this.GrantBiostats = new HashSet<GrantBiostat>();
            this.Tasks = new HashSet<PTTask>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string LogonId { get; set; }
        public string Email { get; set; }
        public System.DateTime EndDate { get; set; }
        public long BitValue { get; set; }
    
        public virtual ICollection<ProjectBioStat> ProjectBioStats { get; set; }
        public virtual ICollection<PublicationBioStat> PublicationBioStats { get; set; }
        public virtual ICollection<GrantBiostat> GrantBiostats { get; set; }
        public virtual ICollection<PTTask> Tasks { get; set; }
    }
}
