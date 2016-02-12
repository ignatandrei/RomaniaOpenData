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
                j.ID = arr[2];//FSJ
                list.Add(j);
            }
            return list.ToArray();

        }


        public async Task<UAT[]> InitUat(Judet[] judete)
        {
            var dd = new DownloadData();
            var dataBytes = await dd.Data(new Uri("http://data.gov.ro/storage/f/2013-11-01T11%3A49%3A59.808Z/siruta.csv"));
            string data = Encoding.UTF8.GetString(dataBytes);
            var list = new List<UAT>();
            var lines = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            Judet lastJudet = null;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var arr = line.Split(new string[] { ";" }, StringSplitOptions.None);

                if (arr[0] == "\"SIRUTA\"")//header
                    continue;

                if (arr[12].Contains("00000000000"))//rang => judet
                {
                    var idJudet = int.Parse(arr[12].Replace("00000000000", "").Replace("\"",""));
                    lastJudet = judete.First(it => it.ID == idJudet.ToString());
                    continue;
                }
                if (lastJudet == null)
                {
                    throw new ArgumentException("not found judet for " + line);
                }
                var uat = new UAT();
                uat.UatTip= (arr[5]);
                uat.Nume = arr[1].Replace("\"", "");
                uat.ID = arr[9];
                
                uat.Judet = lastJudet;
                list.Add(uat);
            }
            return list.ToArray();
            
        }
    }
}
