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
            return await DataSavedLoader.DataSaved();
        }
        [HttpGet]
        public RopDataSaved Get([FromUri] string id)
        {
            using (var rep = new RepositoryLiteDb<RopDataSaved>())
            {
                return rep.GetFromId(id);
            }
        }
    }
}
