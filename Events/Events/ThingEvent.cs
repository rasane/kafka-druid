using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class ThingEvent
    {
        public ThingEvent()
        {
            EventDescription = Category = Priority = AcknowledgementStatus = ActiveStatus = string.Empty;

        }
        public long Id { get; set; }

        public long FacilityId { get; set; }
        public long ThingId { get; set; }

        public string EventDescription { get; set; } 

        public string Category { get; set; } 

        public string Priority { get; set; } 

        public string AcknowledgementStatus { get; set; } 

        public string ActiveStatus { get; set; } 

        public DateTime UpdatedDate { get; set; }
    }
}
