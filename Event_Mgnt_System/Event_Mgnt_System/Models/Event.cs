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
    
    public partial class Event
    {
        public Event()
        {
            this.Booking_Events = new HashSet<Booking_Events>();
        }
    
        public int Event_ID { get; set; }
        public string Event_Type { get; set; }
        public string event_image { get; set; }
        public int Status { get; set; }
    
        public virtual ICollection<Booking_Events> Booking_Events { get; set; }
    }
}
