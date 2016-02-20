using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROPCommon;
using ROPInfrastructure;
using ROPObjects;

namespace ROPANSV
{
    public class NumarAnimale : RopLoader
    {
        private RopDocument rdBase;

        public NumarAnimale()
        {
            rdBase = new RopDocument();
            rdBase.ID = ("822FCCF9-F77F-4D70-A189-7A637CCC7BBC");
            rdBase.PathDocument =
                new Uri("http://data.gov.ro/storage/f/2013-11-19T09%3A18%3A21.274Z/ansv-statistica.csv");
            rdBase.WebPage =
                "http://data.gov.ro/dataset/numar-bovine-ovine-caprine-porci-pe-localitati-si-gospodarii";
            rdBase.Name = "Numar bovine, ovine, caprine, porci pe localitati si gospodarii";
            rdBase.Description =
                "Pentru fiecare localitate numarul total de bovine,ovine,caprine si porci precum si numarul de curti/gospodarii din localitate care au un anumit numar de animale. Informatie exportata din baza de date aferenta crotalierii in luna noiembrie 2013.";

            rdBase.AvailableOn = new SingleTimePeriod("2013-12-31");
        }

        public override async Task<RopDocument[]> FillDate()
        {
            var dd = new DownloadData();
            var dataBytes = await dd.Data(rdBase.PathDocument);
            var str = Encoding.UTF8.GetString(dataBytes);
            var list = new List<RopData>();

            var lines = str.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var arr = line.Split(new[] {";"}, StringSplitOptions.None);
                if(string.IsNullOrWhiteSpace(arr[0]))
                    continue;

                if (arr[0].Contains("Jude"))
                    continue;

                var rd = new RopData();
                rd.Judet = judetFinder.Find(arr[0]);
                rd.Valoare = int.Parse(arr[3]);
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
            return new[] {rdBase};

        }

    }
}