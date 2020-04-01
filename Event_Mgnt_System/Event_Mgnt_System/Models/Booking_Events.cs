//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Event_Mgnt_System.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking_Events
    {
        public int Book_ID { get; set; }
        public Nullable<int> User_ID { get; set; }
        public string User_Name { get; set; }
        public string Event_Type { get; set; }
        public Nullable<int> Event_ID { get; set; }
        public string Venue { get; set; }
        public Nullable<System.DateTime> Event_Date { get; set; }
        public Nullable<System.TimeSpan> Event_time { get; set; }
        public Nullable<int> Guest_Number { get; set; }
        public string Approval { get; set; }
        public Nullable<int> Package_ID { get; set; }
        public string email { get; set; }
        public string package_name { get; set; }
    
        public virtual Event Event { get; set; }
        public virtual Package Package { get; set; }
        public virtual Registration Registration { get; set; }
    }
}
