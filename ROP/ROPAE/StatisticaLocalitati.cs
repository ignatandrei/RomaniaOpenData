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

namespace ROPAE
{
    public class StatisticaLocalitati: RopLoader
    {
        private RopDocument rdBase;
        public StatisticaLocalitati()
        {
            rdBase = new RopDocument();
            
            rdBase.PathDocument =
               new Uri("http://data.gov.ro/storage/f/2014-06-11T07%3A56%3A32.555Z/siaep2014-stat-statistica-pe-localitati.xls");
            rdBase.WebPage =
                "http://data.gov.ro/dataset/alegerea-membrilor-din-romania-pentru-parlamentul-european-2014/resource/756ea5fc-d335-4ea3-adc4-8f6e62a21307";
            rdBase.Name = "statistica alegatori ";
            rdBase.Description =
                "SIAEP2014_STAT_Statistica-pe-localitati.xls";

            rdBase.AvailableOn = new SingleTimePeriod("2014-01-01");
        }

        public override async Task<RopDocument[]> FillDate()
        {

            var dataAlegatori = new List<RopData>();
            var dataAlegatoriPrezenti = new List<RopData>();
            var dd = new DownloadData();
            var dataBytes = await dd.Data(rdBase.PathDocument);

            var path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".xls");
            File.WriteAllBytes(path, dataBytes);
            //var path = "D:\\a.xls";
            var dt = new DataTable();
            using (var m = new OleDbConnection())
            {
                m.ConnectionString = ExcelHelpers.BuildExcelConnectionString(path, true);
                m.Open();
                var query = @"Select * From [SIAEP2014_STAT_Statistica pe lo$]";
                using (var cmd = new OleDbCommand(query, m))
                {
                    using (var dr = cmd.ExecuteReaderAsync().Result)
                    {

                        dt.Load(dr);

                    }
                }

            }
            foreach (DataRow row in dt.Rows)
            {
                
                var arr = row.ItemArray;
                //if (arr[0] == null || string.IsNullOrWhiteSpace(arr[0].ToString()))
                if (string.IsNullOrWhiteSpace(arr[0]?.ToString()))
                    continue;
                var numeJudet = arr[1].ToString().Trim().ToLower();
                if(numeJudet == "străinătate")
                    continue;
                
                var judet = judetFinder.Find(numeJudet);
                var valoare = int.Parse(arr[3].ToString());
                var rd = new RopData();
                rd.Judet = judet;
                rd.Valoare = valoare;
                rd.Oras = null;
                dataAlegatori.Add(rd);

                rd = new RopData();
                rd.Judet = judet;
                rd.Valoare = int.Parse(arr[6].ToString());
                rd.Oras = null;
                dataAlegatoriPrezenti.Add(rd);
            }
            var newRD = new RopDocument(rdBase);
            newRD.Name += "Nr Alegatori Liste";
            newRD.ID = "BA671A2B-27E8-408D-BABD-59B52661789D";
            newRD.Data = dataAlegatori
                    .GroupBy(it => it.Judet).Select(group =>

                  new RopData()
                  {
                      Judet = group.Key,
                      Valoare = group.Sum(it => it.Valoare)
                  }
            ).ToArray();

            var newRDUrne = new RopDocument(rdBase);
            newRDUrne.Name += "Nr Alegatori Prezentati La Urne";
            newRDUrne.ID = "BBB0ECA2-34BE-4177-B3A2-82BC6B41311E";
            newRDUrne.Data = dataAlegatoriPrezenti
                .GroupBy(it => it.Judet).Select(group =>
                  new RopData()
                  {
                      Judet = group.Key,
                      Valoare = group.Sum(it => it.Valoare)
                  }
            ).ToArray();

            return new[] { newRD,newRDUrne };

        }
        }
}
