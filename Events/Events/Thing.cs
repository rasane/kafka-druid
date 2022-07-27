using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class Thing
    {
        public long Id { get; set; }

        public Thing()
        {
            Name = TypeOfThing = LocationOfThing = string.Empty;
        }

        public string Name { get; set; } 

        public string TypeOfThing { get; set; } 
        public string LocationOfThing { get; set; } 

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
