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
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var arr = line.Split(new string[] { "," }, StringSplitOptions.None);
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = arr[i].Replace("\"", "");
                }
                if (arr[0].ToUpper() == "﻿COD")//header
                    continue;
                Judet judet = null;
                if (arr[0].ToUpper() == "PÂRÂUL PÂNTEI")
                    continue;
                if (arr[0].ToUpper() == "HENRI COANDĂ")
                    continue;

                if (arr[0].ToUpper() == "ŞTEI")
                    continue;
                if (arr[0].ToUpper() == "BULZ")
                    continue; 
                if (arr[0] == "0561208551")
                {
                    judet = judetFinder.Find("Bihor");
                    var rd1 = new RopData();
                    rd1.Judet = judet;
                    rd1.Valoare = 1;
                    list.Add(rd1);
                    continue;
                }
                if (arr[0] == "0561200059")
                {
                    judet = judetFinder.Find("Bihor");
                    var rd1 = new RopData();
                    rd1.Judet = judet;
                    rd1.Valoare = 1;
                    list.Add(rd1);
                    continue;
                }
                if (arr[0] == "2762103816")
                {
                    judet = judetFinder.Find("NEAMT");
                    var rd1 = new RopData();
                    rd1.Judet = judet;
                    rd1.Valoare = 1;
                    list.Add(rd1);                   
                    continue;

                }
                
                for (int i = 16; i < arr.Length; i++)
                {

                    var numeJudet = arr[i].ToString().Trim().ToLower();

                    try
                    {
                        judet = judetFinder.Find(numeJudet);
                    }
                    catch (Exception ex)
                    {
                        //do not log
                    }
                    if (judet != null)
                        break;
                }
                
                if(judet == null)
                {
                    throw new ArgumentException("not found judet:" + line);
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
