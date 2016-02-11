using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROPObjects
{
    public class Judet:IID
    {
        public string Nume { get; set; }
        public string Cod { get; set; }

        /// <summary>
        /// redirect to Cod
        /// </summary>
        public string ID { get { return Cod; } set { Cod = value; } }
    }
}
