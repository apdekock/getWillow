//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GetGumtree
{
    using System;
    using System.Collections.Generic;
    
    public partial class VendorNote
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string Note { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
    
        public virtual Vendor Vendor { get; set; }
    }
}
