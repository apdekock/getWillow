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
    
    public partial class PollVotingRecord
    {
        public int Id { get; set; }
        public int PollAnswerId { get; set; }
        public int CustomerId { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual PollAnswer PollAnswer { get; set; }
    }
}
