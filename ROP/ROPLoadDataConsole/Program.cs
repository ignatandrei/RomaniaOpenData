using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raven.Abstractions.Data;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using ROPInfrastructure;
using ROPObjects;
using ROPSiruta;

namespace ROPLoadDataConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            
            

            var rep = new Repository<Judet>();
            var exists = rep.ExistsData("judete");
            if (!exists)
            {
                Console.WriteLine("save data to local");
                var sl = new SirutaLoader();
                var jud = sl.InitJudete().Result;
                var ms=rep.StoreDataAsNew(jud.ToArray(), "judete").Result;
                
            }

            Console.WriteLine("get data from local");
            foreach (var judet in rep.RetrieveData("judete"))
            {
                Console.WriteLine(judet.Nume);
            }
            

        }
    }
}
