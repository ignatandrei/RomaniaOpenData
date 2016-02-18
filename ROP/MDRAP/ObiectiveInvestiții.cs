using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROPCommon;
using ROPInfrastructure;
using ROPObjects;

namespace MDRAP
{
    public class ObiectiveInvestiții : RopLoader
    {
        private RopDocument rdBase;
        public ObiectiveInvestiții()
        {
            rdBase = new RopDocument();
            rdBase.ID = ("346CCE96-6970-4347-BC30-9AF806D00224");
            rdBase.PathDocument =
               new Uri("http://data.gov.ro/dataset/c0301bf1-88b6-408b-8740-88871133d8dd/resource/466e023d-2ecb-4c40-a4c5-a409d40661cb/download/listalegea114.xls");
            rdBase.WebPage =
                "http://data.gov.ro/dataset/programul-de-constructii-de-locuinte-sociale/resource/466e023d-2ecb-4c40-a4c5-a409d40661cb";
            rdBase.Name = "Obiective de investitii finantate - stadiu ";
            rdBase.Description =
                "Lista obiectivelor de investiții finanțate în anul 2015, cu Ordine MDRAP aprobate - situație la data de 20.09.2015";

            rdBase.AvailableOn = new SingleTimePeriod("2014-12-31");
        }

        public override async Task<RopDocument[]> FillDate()
        {
            var dd = new DownloadData();
            var dataBytes = await dd.Data(rdBase.PathDocument);
            var path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".xls");
            File.WriteAllBytes(path, dataBytes);


            var list = new List<RopData>();


            var str = 
@"Bacău 55
Bacău 40
Botoșani 72
Botoșani 24
Botoșani 7
Brașov 45
Călărași 29
Constanța 72
Constanța 96
Giurgiu 20
Gorj 16
Gorj 64
Gorj 40
Harghita 48
Harghita 18
Hunedoara 60
Hunedoara 30
Hunedoara 147
Ialomița 40
Ilfov 40
Maramureş 48
Mureș 36
Olt 60
Olt 38
Olt 40
Olt 32
Prahova 18
Prahova 30
Satu Mare 40
Timiş 15
Timiş 48
Vâlcea 40
Vrancea 8
Vrancea 60
Vrancea 61
Vrancea 16
";
            foreach (var s in str.Split(new[] {Environment.NewLine},StringSplitOptions.RemoveEmptyEntries))
            {
                var arr = s.Split(new[] {" "},StringSplitOptions.RemoveEmptyEntries);
                var nr =int.Parse(arr[arr.Length - 1]);
                string numeJudet = "";
                switch (arr.Length)
                {
                    case 2:
                        numeJudet = arr[0];
                        break;
                    case 3:
                        numeJudet = arr[0] + " " + arr[1];
                        break;
                    default:
                        throw new ArgumentException("not found judet " + s);

                }
                var judet = judetFinder.Find(numeJudet);
                var ropData=new RopData();
                ropData.Judet = judet;
                ropData.Valoare = nr;
                list.Add(ropData);
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
