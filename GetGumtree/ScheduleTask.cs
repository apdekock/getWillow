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
    
    public partial class ScheduleTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Seconds { get; set; }
        public string Type { get; set; }
        public bool Enabled { get; set; }
        public bool StopOnError { get; set; }
        public string LeasedByMachineName { get; set; }
        public Nullable<System.DateTime> LeasedUntilUtc { get; set; }
        public Nullable<System.DateTime> LastStartUtc { get; set; }
        public Nullable<System.DateTime> LastEndUtc { get; set; }
        public Nullable<System.DateTime> LastSuccessUtc { get; set; }
    }
}