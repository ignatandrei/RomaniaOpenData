using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROPCommon;
using ROPInfrastructure;
using ROPObjects;

namespace ROPPI
{
    public class ParcAuto:RopLoader
    {
        private RopDocument rdBase;
        public ParcAuto()
        {
            rdBase = new RopDocument();

            rdBase.PathDocument =
                new Uri(
                    "http://data.gov.ro/dataset/b93e0946-2592-4ed7-a520-e07cba6acd07/resource/98895aa9-5cdf-4572-86d8-d6ec6c28b0da/download/parcauto2015.csv");
            rdBase.WebPage =
                "http://data.gov.ro/dataset/parc-auto-romania/resource/98895aa9-5cdf-4572-86d8-d6ec6c28b0da";
            rdBase.Name = "Parcul auto la data de 31.12.2015";
            rdBase.Description =
                "Situatia cuprinde parcul auto la 31.12.2015";
            rdBase.ID = "1AEA07DB-4F14-42DC-989A-91241A020182";
            rdBase.AvailableOn = new SingleTimePeriod("2015-12-31");
        }

        public override async Task<RopDocument[]> FillDate()
        {

            var data= new List<RopData>();

            var dd = new DownloadData();
            var dataBytes = await dd.Data(rdBase.PathDocument);

            var path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".csv");
            var csv = Encoding.UTF8.GetString(dataBytes);
            //var path = "D:\\a.xls";
            var lines = csv.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var arr = line.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = arr[i].Replace("\"", "");
                }
                if (arr[0].Trim().ToUpper() == "﻿JUDET") //header
                    continue;


                var numeJudet = arr[0].Trim().ToLower();
                var judet = judetFinder.Find(numeJudet);
                int valoare;
                try
                {
                    valoare = int.Parse(arr[arr.Length-1]);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("judet:" + numeJudet + "=> valoare"+ arr[arr.Length - 1],ex);
                }
                var rd = new RopData();
                rd.Judet = judet;
                rd.Valoare = valoare;
                rd.Oras = null;
                data.Add(rd);
            }
            rdBase.Data = data
                .GroupBy(it => it.Judet).Select(group =>

                    new RopData()
                    {
                        Judet = group.Key,
                        Valoare = group.Sum(it => it.Valoare)
                    }
                ).ToArray();

            return new[] { rdBase };



        }

        }
    }
