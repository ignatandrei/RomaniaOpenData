using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROPCommon;
using ROPInfrastructure;
using ROPObjects;

namespace ROPPermise
{
    public class DetinatoriPermise : RopLoader
    {
        private RopDocument rdBase;
        public DetinatoriPermise()
        {
            rdBase = new RopDocument();
            rdBase.ID = ("305A49E7-0971-445F-890E-293F502FCDB1");
            rdBase.PathDocument =
               new Uri("http://data.gov.ro/storage/f/2015-03-04T11%3A53%3A57.941Z/detinatori-pc-2014.csv");
            rdBase.WebPage =
                "http://data.gov.ro/dataset/detinatori-permise-de-conducere/resource/137f2972-5350-4dc8-b8f3-7f135fdd60fb";
            rdBase.Name = "detinatori pc 2014.csv";
            rdBase.Description =
                "Statistica detinatori permise de conducere si categorii";

            rdBase.AvailableOn = new SingleTimePeriod("2014-12-31");
        }

        public override async Task<RopDocument[]> FillDate()
        {
            var dd = new DownloadData();
            var dataBytes = await dd.Data(rdBase.PathDocument);
            string data = Encoding.UTF8.GetString(dataBytes);
            var list = new List<RopData>();
            var lines = data.Split((char) (10));
            var restFromPreviousLine = "";
            for (int lineIter = 0; lineIter < lines.Length; lineIter++)
            {
                
                var line = lines[lineIter];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var arr = (restFromPreviousLine + line).Split(new[] {";"}, StringSplitOptions.None);
                if (arr.Length < 10)
                {
                    restFromPreviousLine += line;
                    continue;
                }
                restFromPreviousLine = "";
                if(arr[0].StartsWith("Jude"))
                    continue;
                if (string.IsNullOrWhiteSpace(arr[0]))
                    continue;

                var numeJudet = arr[0].Trim().ToLower();  
                if(numeJudet.ToLower()== "dgeip")              
                    continue;
                
                var judet = judetFinder.Find(numeJudet);
                var val = int.Parse(arr[6].Replace(".", ""));
                
                var rd = new RopData();
                rd.Judet = judet;
                rd.Valoare = val;
                list.Add(rd);


            }
            list = list.GroupBy(it => it.Judet).Select(group =>

                  new RopData()
                  {
                      Judet = group.Key,
                      Valoare = group.Sum(it => it.Valoare)
                  }
            ).ToList();
            rdBase.Data = list.ToArray();
            return new[] { rdBase };
        }
    }
}
