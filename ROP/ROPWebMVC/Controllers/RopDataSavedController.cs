using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ROPInfrastructure;

namespace ROPWebMVC.Controllers
{
    public class RopDataSavedController : ApiController
    {
        public async Task<IEnumerable<RopDataSaved>> Get()
        {
            return await instanceRavenStore.DataSaved();
        }
        [HttpGet]
        public RopDataSaved Get([FromUri] string id)
        {
            using (var rep = new Repository<RopDataSaved>())
            {
                return rep.GetFromId(id);
            }
        }
    }
}
