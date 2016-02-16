using ROPInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROPObjects;
using ROPCommon;

namespace ROPMECS
{
    public class UnitatiInvatamint : RopLoader
    {
        private RopDocument rdBase;
        public UnitatiInvatamint()
        {
            rdBase = new RopDocument();
            rdBase.ID = ("B4069A84-5C75-4EFA-98BD-9594A50A997C");
            rdBase.PathDocument =
               new Uri("http://date.edu.ro/sites/default/files/Retea_2014_2015.csv");
            rdBase.WebPage =
                "http://data.gov.ro/dataset/reteaua-scolara-a-unitatilor-de-invatamant-2014-2015";
            rdBase.Name = "Rețeaua școlară a unităților de învățământ 2014-2015";
            rdBase.Description =
                "Rețeaua școlară a unităților de învățământ include datele tuturor unităților subordonate Ministerului Educației Naționale care funcționează în anul școlar 2014-2015.";

            rdBase.AvailableOn = new SingleTimePeriod("2015-12-31");
        }
        public override async Task<RopDocument[]> FillDate()
        {
            var dd = new DownloadData();
            var dataBytes = await dd.Data(rdBase.PathDocument);
            string data = Encoding.UTF8.GetString(dataBytes);
            var list = new List<RopData>();
            var lines = data.Split((char)(10));
            var restFromPreviousLine = "";
            for(int lineIter=0;lineIter<lines.Length; lineIter++)
            {
                if(lineIter==0)//header
                    continue;
                
                var line = lines[lineIter];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var arr = (restFromPreviousLine+line).Split(new [] { "," }, StringSplitOptions.None);
                
                if (arr.Length < 16)
                {
                    restFromPreviousLine += line;
                    continue;    
                }
                restFromPreviousLine = "";
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = arr[i].Replace("\"", "");
                }

                Judet judet = null;                
                for (int i = 16; i < arr.Length; i++)
                {

                    var numeJudet = arr[i].ToString().Trim().ToLower();

                    try
                    {
                        judet = judetFinder.Find(numeJudet);
                    }
                    catch (ArgumentException)
                    {
                        //do not log
                    }
                    if (judet != null)
                        break;
                }
                
                if(judet == null)
                {
                    throw new ArgumentException("not found judet:" + string.Join(",", arr));
                }

                var rd = new RopData();
                rd.Judet = judet;
                rd.Valoare = 1;                
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
