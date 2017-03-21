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
    
    public partial class QueuedEmail
    {
        public int Id { get; set; }
        public int PriorityId { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public string To { get; set; }
        public string ToName { get; set; }
        public string ReplyTo { get; set; }
        public string ReplyToName { get; set; }
        public string CC { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentFilePath { get; set; }
        public string AttachmentFileName { get; set; }
        public int AttachedDownloadId { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public Nullable<System.DateTime> DontSendBeforeDateUtc { get; set; }
        public int SentTries { get; set; }
        public Nullable<System.DateTime> SentOnUtc { get; set; }
        public int EmailAccountId { get; set; }
    
        public virtual EmailAccount EmailAccount { get; set; }
    }
}
