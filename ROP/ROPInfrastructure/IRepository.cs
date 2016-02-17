using System.Collections.Generic;
using System.Threading.Tasks;
using ROPObjects;

namespace ROPInfrastructure
{
    public interface IRepository<T> where T : IID
    {
        string Type { get; set; }

        void DeleteData();
        void Dispose();
        bool ExistsData();
        T GetFromId(string id);
        IEnumerable<T> RetrieveData();
        Task<long> StoreDataAsNew(T[] data);
    }
}