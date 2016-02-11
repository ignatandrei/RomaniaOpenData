using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ROPObjects;

namespace ROPInfrastructure
{
    public interface IRopLoader
    {
        void Init(Judet[] judete,UAT[] uate);

        Task<RopDocument[]> Date();

    }
}
