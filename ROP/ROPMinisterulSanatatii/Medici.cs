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

namespace ROPMinisterulSanatatii
{
    public class Medici: RopLoader
    {
        private RopDocument rdBase;
        public Medici()
        {
            rdBase = new RopDocument();
            rdBase.ID = ("931433F4-1595-4AF6-A746-8C5859B0823C");
            rdBase.PathDocument =
               new Uri( "http://www.date.gov.ro/dataset/3c128d2f-f4e2-47d5-ad11-a5602c1e4856/resource/61a73bc0-34c6-4067-b1c4-3ab659323c87/download/numrul-medicilor-pe-judee-i-ministere-din-sectorul-public-numrul-medicilor-pe-ministere-macroreg.xls");
            rdBase.WebPage =
                "http://data.gov.ro/dataset/personalul-medico-sanitar-din-romania/resource/61a73bc0-34c6-4067-b1c4-3ab659323c87";
            rdBase.Name = "personalul-medico-sanitar-din-romania";
            rdBase.Description =
                "NUMĂRUL MEDICILOR PE JUDEŢE ŞI MINISTERE DIN SECTORUL PUBLIC/NUMĂRUL MEDICILOR PE MINISTERE, MACROREGIUNI ŞI REGIUNI DIN SECTORUL PUBLIC";

            rdBase.AvailableOn = new SingleTimePeriod("2015-10-19");
        }

        public override async Task<RopDocument[]> FillDate()
        {
            
            List<RopData> data=new List<RopData>();
            var dd = new DownloadData();
            var dataBytes = await dd.Data(rdBase.PathDocument);

            var path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".xls");
            File.WriteAllBytes(path,dataBytes);
            //var path = "D:\\a.xls";
            var dt = new DataTable();
            using (var m = new OleDbConnection())
            {
                m.ConnectionString = ExcelHelpers.BuildExcelConnectionString(path, true);
                m.Open();
                var query = @"Select * From [Sheet1$]";
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
                int nr;
                var arr = row.ItemArray;
                //if (arr[0] == null || string.IsNullOrWhiteSpace(arr[0].ToString()))
                if (string.IsNullOrWhiteSpace(arr[0]?.ToString()))
                    continue;

                if(!int.TryParse(arr[0].ToString(),out nr))
                    continue;

                var numeJudet = arr[1].ToString().Trim().ToLower();
                var judet = judetFinder.Find(numeJudet);
                var rd=new RopData();
                rd.Judet = judet;
                rd.Valoare = int.Parse(arr[2].ToString());
                rd.Oras = null;
                data.Add(rd);



            }
            rdBase.Data = data.ToArray();
            return new [] { rdBase };
        }

        
        
        

        
    }
}
