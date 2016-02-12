using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ROPCommon;
using ROPInfrastructure;
using ROPObjects;
using System.IO;

namespace ROPMinisterulSanatatii
{
    public class Farmacii: IRopLoader
    {
        private JudetFinder judetFinder;
        private UAT[] uate;

        public void Init(JudetFinder judete, UAT[] uate)
        {
            this.judetFinder = judete;
            this.uate = uate;
        }

        public async Task<RopDocument[]> FillDate()
        {
            
            var dd = new DownloadData();
            var uri = new Uri("http://data.gov.ro/dataset/situatia-farmaciilor-din-romania");
            var dataBytes = await dd.Data(uri);
            var str = Encoding.ASCII.GetString(dataBytes);
            var hd = new HtmlDocument();
            hd.LoadHtml(str);
            var nodes= hd.DocumentNode.SelectNodes("//a[@class='resource-url-analytics']");
            var taskList = new List<Task<RopData>>();
            foreach (var node in nodes)
            {
                var link=node.Attributes["href"].Value;
                var task = CreateFarmacie(new Uri(link));
                taskList.Add(task);
            }

            await Task.WhenAll(taskList.ToArray());

            var rd = new RopDocument();
            rd.Name = "farmacii";
            rd.PathDocument = uri;
            rd.ID = "45A83E85-D049-4258-8575-9CE43C49273C";            
            rd.Description = "Situatia farmaciilor din Romania";
            var list = taskList.
                Select(it => (it.Exception == null) ? it.Result : null)
                .Where(it => it != null);
            //Bucuresti is duplicate - judet
            list=list.GroupBy(it => it.Judet).Select(group =>
            
                new RopData()
                {
                    Judet = group.Key,
                    Valoare = group.Sum(it => it.Valoare)
                }
            ).ToArray();
            rd.Data = list.ToArray();
            return new[] { rd };

        }

        private async Task<RopData> CreateFarmacie(Uri uri)
        {

            var rd = new RopData();
            var numeFarmacie = uri.Segments.Last().Replace(".xls","");
            switch (numeFarmacie)
            {
                case "bucuresti---sector-1":
                case "bucuresti---sector-2":
                case "bucuresti---sector-3":
                case "bucuresti---sector-4":
                case "bucuresti---sector-5":
                case "bucuresti---sector-6":
                    rd.Judet = judetFinder.Find("Bucuresti");
                    break;
                case "farmacii-circuit-inchis324112670-1":
                    //TODO: take this into consideration
                    return null;                    
                default:
                    rd.Judet = judetFinder.Find(numeFarmacie);
                    break;                    

            }
            
            
            
            var dd = new DownloadData();
            var dataBytes = await dd.Data(uri);
            var path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".xls");
            File.WriteAllBytes(path, dataBytes);
            var nr =await ExcelHelpers.NrRows(path);
            rd.Valoare = nr;
            return rd;
        }
    }
}
