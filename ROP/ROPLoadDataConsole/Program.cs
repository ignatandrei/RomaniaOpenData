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

        static AlternateNamesJudet[] GetAlternate(Judet[] jud)
        {
            var alternateNames = new List<AlternateNamesJudet>();
            Func<string, string, AlternateNamesJudet> a = (altNume, Nume) =>
            {
                altNume = altNume.ToLower();
                Nume = Nume.ToLower();
                var alt = new AlternateNamesJudet();
                alt.IDJudet = jud.First(it => it.Nume.ToLower() == Nume).ID;
                alt.AlternateName = altNume;
                return alt;
            };
            
            alternateNames.Add(a("argeş", "arges"));

            alternateNames.Add(a("bacău", "bacau"));
            alternateNames.Add(a("Bistriţa-N.", "BISTRITA-NASAUD"));
            alternateNames.Add(a("Botoşani", "Botosani"));
            alternateNames.Add(a("braşov", "Brasov"));
            alternateNames.Add(a("brăila", "braila"));
            alternateNames.Add(a("buzău", "buzau"));
            alternateNames.Add(a("caraş-s.", "CARAS-SEVERIN"));
            alternateNames.Add(a("călăraşi", "calarasi"));
            alternateNames.Add(a("constanţa", "Constanta"));
            alternateNames.Add(a("dâmboviţa", "dimbovita"));
            alternateNames.Add(a("galaţi", "galati"));
            alternateNames.Add(a("ialomiţa", "ialomita"));
            alternateNames.Add(a("iaşi", "iasi"));
            alternateNames.Add(a("maramureş", "maramures"));
            alternateNames.Add(a("mehedinţi", "mehedinti"));
            alternateNames.Add(a("mureş", "mures"));
            alternateNames.Add(a("neamţ", "neamt"));
            alternateNames.Add(a("satu-mare", "satu_mare"));
            alternateNames.Add(a("sălaj", "salaj"));
            alternateNames.Add(a("timiş", "timis"));
            alternateNames.Add(a("vâlcea", "vilcea"));
            alternateNames.Add(a("m.bucureşti", "bucuresti"));





            //using (var rep = new Repository<AlternateNamesJudet>())
            //{
            //    await rep.StoreDataAsNew(alternateNames);
            //}
            return alternateNames.ToArray();
        }

        static async Task<Judet[]> GetJudete()
        {

            

            using (var rep = new Repository<Judet>())
            {
                var exists = rep.ExistsData();
                if (!exists)
                {
                    Console.WriteLine("save judete to local");
                    var sl = new SirutaLoader();
                    var jud = (await sl.InitJudete()).ToArray();
                    var ms = await rep.StoreDataAsNew(jud);
                    
                }

                Console.WriteLine("get data from local");
                return rep.RetrieveData().ToArray();



            }
        }

        static async Task<UAT[]> GetUAT(Judet[] judete)
        {
            return new UAT[0];
            using (var repUAT = new Repository<UAT>())
            {
                var exists = repUAT.ExistsData();
                if (!exists)
                {
                    Console.WriteLine("save uat to local");
                    var sl = new SirutaLoader();
                    var uat = await sl.InitUat(judete);
                    var ms = await repUAT.StoreDataAsNew(uat.ToArray());

                }
                Console.WriteLine("get data from local");
                return repUAT.RetrieveData().ToArray();

            }
        }

        static void Main(string[] args)
        {
            //var dd = new DownloadData();
            //var dataBytes = dd.Data("http://www.date.gov.ro/dataset/3c128d2f-f4e2-47d5-ad11-a5602c1e4856/resource/61a73bc0-34c6-4067-b1c4-3ab659323c87/download/numrul-medicilor-pe-judee-i-ministere-din-sectorul-public-numrul-medicilor-pe-ministere-macroreg.xls").Result;

            //var path =  "D:\\a.xls";
            //File.WriteAllBytes(path, dataBytes);
            //Process.Start(path);
            //return;

            var judete = GetJudete().Result;
            foreach (var judet in judete)
            {
                Console.WriteLine(judet.Nume);
            }

            var UAT = GetUAT(judete).Result;
            foreach (var uat in UAT)
            {

                // Console.WriteLine(uat.UatTip +"---" +uat.Nume + "---" + uat.Judet.Nume);

            }
            var judFinder=new JudetFinder();
            judFinder.judete = judete;
            judFinder.altNumeJudet = GetAlternate(judete);



            IRopLoader loader = new Medici();
            loader.Init(judFinder,UAT);
            //var date = loader.FillDate().Result;
            //foreach (var data   in date)
            //{
            //    foreach (var ropData in data.Data)
            //    {
            //        Console.WriteLine(ropData.Judet.Nume + " " +ropData.Valoare);
            //    }
            //}


            loader = new Farmacii();
            loader.Init(judFinder, UAT);
            var date = loader.FillDate().Result;
            
        }
    }
}