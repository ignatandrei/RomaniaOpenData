using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROPInfrastructure
{
    public class DataSavedLoader
    {
        public static async Task<RopDataSaved[]> DataSaved()
        {
            var listTasks = new List<Task<IRopLoader>>();
            using (var rep = new RepositoryLiteDb<RopDataSaved>())
            {

                var data = rep.RetrieveData();
                if (data != null)
                {
                    var arrData = data.ToArray();

                    return arrData;

                }

            }
            return null;

        }
    }
}
