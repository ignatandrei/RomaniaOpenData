using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ROPCommon;
using ROPInfrastructure;
using ROPObjects;

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
            var dataBytes = await dd.Data("http://data.gov.ro/dataset/situatia-farmaciilor-din-romania");
            var str = Encoding.ASCII.GetString(dataBytes);
            var hd = new HtmlDocument();
            hd.LoadHtml(str);
            var nodes= hd.DocumentNode.SelectNodes("//a[@class='resource-url-analytics']");
            foreach (var node in nodes)
            {
                Console.WriteLine(node.Attributes["href"].Value);
            }
            return null;
        }
    }
}
