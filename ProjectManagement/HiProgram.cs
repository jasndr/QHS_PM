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
    
    public partial class HiProgram
    {
        public HiProgram()
        {
            this.ProjectStudies = new HashSet<ProjectStudy>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isStudyArea { get; set; }
        public bool isHealthData { get; set; }
    
        public virtual ICollection<ProjectStudy> ProjectStudies { get; set; }
    }
}
