using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Raven.Database.Indexing.Collation.Cultures;
using ROPObjects;

namespace ROPInfrastructure
{
    public interface IRopLoader
        
    {
        
        void Init(JudetFinder judete);

        Task<RopDocument[]> FillDate();

        Task<RopDocument[]> GetData();

    }

    public abstract class RopLoader : IRopLoader
    {

        public void Init(JudetFinder judete)
        {
            this.judetFinder = judete;
        }

        protected JudetFinder judetFinder { get; set; }

        public abstract  Task<RopDocument[]> FillDate();
        
        public async Task<RopDocument[]> GetData()
        {
            var type = this.GetType().FullName;
            using (var rep = new Repository<RopDocument>(type))
            {
                var exists = rep.ExistsData();
                if (!exists)
                {
                    var rd = await FillDate();
                    var notId = rd.FirstOrDefault(it => string.IsNullOrWhiteSpace(it.ID));
                    if (notId != null)
                    {
                        throw new ArgumentException("not id for" + notId.Name + "-- " + notId.PathDocument);
                    }
                    await rep.StoreDataAsNew(rd);
                }

                var data = rep.RetrieveData();
                return data.ToArray();
            }
        }
    }
}
