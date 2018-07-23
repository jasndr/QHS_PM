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
    
    public partial class Academic
    {
        public int Id { get; set; }
        public long BiostatBitSum { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> NumOfAttendees { get; set; }
        public string CourseNum { get; set; }
        public Nullable<decimal> NumOfCredits { get; set; }
        public string Location { get; set; }
        public string AcademicDesc { get; set; }
        public int AcademicTypeId { get; set; }
        public Nullable<int> StartSemesterId { get; set; }
        public Nullable<int> EndSemesterId { get; set; }
        public Nullable<int> FieldId { get; set; }
        public string Name { get; set; }
        public string Advisor { get; set; }
        public string Comments { get; set; }
        public string Creator { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Audience { get; set; }
    }
}
