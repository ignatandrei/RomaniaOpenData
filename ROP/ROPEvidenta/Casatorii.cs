using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using ROPCommon;
using ROPInfrastructure;
using ROPObjects;

namespace ROPEvidenta
{
    public class Casatorii : RopLoader
    {
        private RopDocument rdBase;

        public Casatorii()
        {
            rdBase = new RopDocument();

            rdBase.PathDocument =
                new Uri(
                    "http://data.gov.ro/dataset/55a39469-fb6a-4dae-90ee-827d075eab9e/resource/7f941b8d-4f7e-477d-92b8-a7c9d4be6511/download/casatorii2015.xlsx");
            rdBase.WebPage =
                "http://data.gov.ro/dataset/numarul-casatoriilor-civile-in-anul-2015-pe-luni-respectiv-orase/resource/7f941b8d-4f7e-477d-92b8-a7c9d4be6511";
            rdBase.Name = "căsătorii 2015 ";
            rdBase.Description =
                "Numărul căsătoriilor civile, în anul 2015";
            rdBase.ID = "4E29E0A3-D7A6-4F00-B141-DF6253158223";
            rdBase.AvailableOn = new SingleTimePeriod("2015-01-01");
        }

        public override async Task<RopDocument[]> FillDate()
        {

            var dataCasatoriti = new List<RopData>();

            var dd = new DownloadData();
            var dataBytes = await dd.Data(rdBase.PathDocument);

            var path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".xls");
            File.WriteAllBytes(path, dataBytes);
            //var path = "D:\\a.xls";
            var dt = new DataTable();
            using (var m = new OleDbConnection())
            {
                m.ConnectionString = ExcelHelpers.BuildExcel2007ConnectionString(path, true);
                m.Open();
                var query = @"Select * From [Anul2015$]";
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
                var judet = judetFinder.Find(numeJudet);
                Func<object, int> retVal = (obj) =>
                {
                    int i;
                    if (obj == null)
                        return 0;

                    if (int.TryParse(obj.ToString(), out i))
                        return i;

                    return 0;
                };
                int valoare = retVal(arr[15]);
                var rd = new RopData();
                rd.Judet = judet;
                rd.Valoare = valoare;
                rd.Oras = null;
                dataCasatoriti.Add(rd);


            }
            rdBase.Data = dataCasatoriti
                .GroupBy(it => it.Judet).Select(group =>

                    new RopData()
                    {
                        Judet = group.Key,
                        Valoare = group.Sum(it => it.Valoare)
                    }
                ).ToArray();

            return new[] {rdBase};
        }
    }
}