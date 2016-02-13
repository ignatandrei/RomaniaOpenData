using System.Web.Http;
using ROPInfrastructure;

namespace ROPWebMVC.Controllers
{
    public class RopDataSavedController : ApiController
    {
        [HttpGet]
        public RopDataSaved Get([FromUri] string id)
        {
            var rep = new Repository<RopDataSaved>();
            return rep.GetFromId(id);
        }
    }
}
