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
    
    public partial class InvoiceItem2
    {
        public int Id { get; set; }
        public string StaffType { get; set; }
        public decimal InvoiceHr { get; set; }
        public string Creator { get; set; }
        public System.DateTime CreationDate { get; set; }
        public int Invoice1Id { get; set; }
        public int ClientAgmtId { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Invoice1 Invoice1 { get; set; }
        public virtual ClientAgmt ClientAgmt { get; set; }
    }
}
