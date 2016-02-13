using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raven.Abstractions.Data;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database.Storage.Voron.Impl;
using ROPCommon;
using ROPInfrastructure;
using ROPMinisterulSanatatii;
using ROPObjects;
using ROPSiruta;

namespace ROPLoadDataConsole
{
    class Program
    {

       

        //static async Task<UAT[]> GetUAT(Judet[] judete)
        //{
        //    return new UAT[0];
        //    using (var repUAT = new Repository<UAT>())
        //    {
        //        var exists = repUAT.ExistsData();
        //        if (!exists)
        //        {
        //            Console.WriteLine("save uat to local");
        //            var sl = new SirutaLoader();
        //            var uat = await sl.InitUat(judete);
        //            var ms = await repUAT.StoreDataAsNew(uat.ToArray());

        //        }
        //        Console.WriteLine("get data from local");
        //        return repUAT.RetrieveData().ToArray();

        //    }
        //}

        
        static void Main(string[] args)
        {
            //var dd = new DownloadData();
            //var dataBytes = dd.Data("http://www.date.gov.ro/dataset/3c128d2f-f4e2-47d5-ad11-a5602c1e4856/resource/61a73bc0-34c6-4067-b1c4-3ab659323c87/download/numrul-medicilor-pe-judee-i-ministere-din-sectorul-public-numrul-medicilor-pe-ministere-macroreg.xls").Result;

            
            //return;

            var judete = instanceRavenStore.Judete().Result;
            foreach (var judet in judete)
            {
                Console.WriteLine(judet.Nume);
            }


            var dataSv = instanceRavenStore.DataSaved().Result;
            foreach (var data in dataSv)
            {
                var docs = instanceRavenStore.GetOrLoad(data).Result;
                foreach (var ropDocument in docs)
                {
                    Console.WriteLine(ropDocument.Name);
                    foreach (var ropData in ropDocument.Data)
                    {
                        Console.WriteLine("              " + ropData.Judet.Nume + " "  + ropData.Valoare);
                    }
                }

            }

           
            
            

            var dataSaved=new List<RopDataSaved>();

            var type = typeof (Medici);
            var dataRetr =instanceRavenStore.GetOrLoad(type).Result;
            dataSaved.AddRange(
                dataRetr.Select(ropDocument => new RopDataSaved()
                {
                    ID = ropDocument.ID,
                    Name = type.AssemblyQualifiedName,
                    Document = ropDocument
                }));

            type = typeof(Farmacii);
            dataRetr = instanceRavenStore.GetOrLoad(typeof(Farmacii)).Result;

            dataSaved.AddRange(
                dataRetr.Select(ropDocument => new RopDataSaved()
                {
                    ID = ropDocument.ID,
                    Name = type.AssemblyQualifiedName,
                    Document = ropDocument
                }));

            using (var rep = new Repository<RopDataSaved>())
            {
                var q =rep.StoreDataAsNew(dataSaved.ToArray()).Result;
            }
            
        }
        
        private static void write(RopDocument[] d)
        {
            foreach (var doc in d)
            {
                Console.WriteLine("------------"+doc.Name);
                foreach (var item in doc.Data)
                {
                    Console.WriteLine(item.Judet.Nume + " -- " + item.Valoare);
                }
            }
        }
    }
}