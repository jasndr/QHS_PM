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
    
    public partial class P_INVOICE_HOURS2_Result
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string PI { get; set; }
        public string Title { get; set; }
        public string Agreement { get; set; }
        public string Type { get; set; }
        public Nullable<decimal> Approved { get; set; }
        public decimal Invoiced { get; set; }
        public Nullable<decimal> Remain { get; set; }
        public decimal TimeTracking { get; set; }
        public Nullable<decimal> TobeBilled { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> RemainAfter { get; set; }
        public int ranking { get; set; }
    }
}
