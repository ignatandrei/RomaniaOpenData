using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROPCommon;
using ROPObjects;

namespace ROPSiruta
{
    public class SirutaLoader
    {
        public async Task<Judet[]> InitJudete()
        {

            var dd = new DownloadData();
            var dataBytes =await dd.Data(new Uri("http://data.gov.ro/storage/f/2013-11-01T11%3A53%3A13.359Z/siruta-judete.csv"));
            string data = Encoding.UTF8.GetString(dataBytes);
            var list = new List<Judet>();
            var lines = data.Split(new string[] {Environment.NewLine},StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var arr=line.Split(new string[] { ";"}, StringSplitOptions.RemoveEmptyEntries);

                if(arr[0]=="JUD")//header
                    continue;

                var j = new Judet();
                j.Cod = arr[3];
                j.Nume = arr[1];
                list.Add(j);
            }
            return list.ToArray();

        }
    }
}
