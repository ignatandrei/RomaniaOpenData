using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROPObjects
{
    public class RopDocument
    {
        public string UniqueId { get; set; }
        public Uri PathDocument { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }        
        public TimePeriod AvailableOn { get; set; }

        public RopData[] Data { get; set; } 

    }
}
