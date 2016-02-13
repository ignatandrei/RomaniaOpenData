using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROPObjects
{
    public class RopDocument:IID
    {
        public RopDocument()
        {
            
        }
        public RopDocument(RopDocument rd)
        {
            this.Name = rd.Name;
            this.AvailableOn = rd.AvailableOn;
            this.Description = rd.Description;
            this.PathDocument = rd.PathDocument;
            this.WebPage = rd.WebPage;

        }
        public string ID { get; set; }
        public Uri PathDocument { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public TimePeriod AvailableOn { get; set; }

        public RopData[] Data { get; set; }
        public string WebPage { get; set; }
    }
}
