using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROPObjects;

namespace ROPInfrastructure
{
    public class RopDataSaved:IID
    {
        public RopDocument Document { get; set; }

        
        public string ID { get; set; }

        public string Name { get; set; }
    }
}
