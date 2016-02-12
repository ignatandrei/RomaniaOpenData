using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ROPObjects
{
    public interface IID
    {
        string ID { get; set; }
        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// DO NOT USE - for retrieving from database
        /// </value>
        string TypeName { get; set; }
    }
}
