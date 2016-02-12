using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Raven.Database.Indexing.Collation.Cultures;
using ROPObjects;

namespace ROPInfrastructure
{
    public interface IRopLoader
        
    {
        
        void Init(JudetFinder judete,UAT[] uate);

        Task<RopDocument[]> FillDate();

    }
}
