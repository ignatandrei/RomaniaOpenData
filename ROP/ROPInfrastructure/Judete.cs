using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROPObjects;
using ROPSiruta;

namespace ROPInfrastructure
{
    public class JudeteLoader
    {
        private static JudetFinder judFinderCache;
        public static async Task<Judet[]> Judete()
        {
            if (judFinderCache != null)
            {
                return judFinderCache.judete;
            }
            using (var rep = new RepositoryLiteDb<Judet>())
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
        public static JudetFinder judFinder
        {
            get
            {
                if (judFinderCache != null)
                {
                    return judFinderCache;
                }
                 var jud = Judete().Result;
                judFinderCache = new JudetFinder();
                judFinderCache.judete = jud;
                judFinderCache.altNumeJudet = GetAlternate(jud);
                return judFinderCache;
                
            }
        }
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
            alternateNames.Add(a("bistriţa-năsăud", "BISTRITA-NASAUD"));
            alternateNames.Add(a("bistrita nasaud", "BISTRITA-NASAUD"));
            alternateNames.Add(a("Botoşani", "Botosani"));
            alternateNames.Add(a("botoșani", "Botosani"));
            alternateNames.Add(a("braşov", "Brasov"));
            alternateNames.Add(a("brașov", "Brasov"));
            alternateNames.Add(a("brăila", "braila"));
            alternateNames.Add(a("buzău", "buzau"));
            alternateNames.Add(a("caraş-s.", "CARAS-SEVERIN"));
            alternateNames.Add(a("caras severin", "CARAS-SEVERIN"));
            alternateNames.Add(a("caraş-severin", "CARAS-SEVERIN"));
            alternateNames.Add(a("călăraşi", "calarasi"));
            alternateNames.Add(a("călărași", "calarasi"));
            alternateNames.Add(a("constanţa", "Constanta"));
            alternateNames.Add(a("constanța", "Constanta"));
            alternateNames.Add(a("dîmboviţa", "dimbovita"));
            alternateNames.Add(a("dâmboviţa", "dimbovita"));
            alternateNames.Add(a("dâmbovita", "dimbovita"));
            alternateNames.Add(a("dambovita", "dimbovita"));
            alternateNames.Add(a("galaţi", "galati"));
            alternateNames.Add(a("ialomiţa", "ialomita"));
            alternateNames.Add(a("ialomița", "ialomita"));
            alternateNames.Add(a("iaşi", "iasi"));
            
            alternateNames.Add(a("maramureş", "maramures"));
            alternateNames.Add(a("mehedinţi", "mehedinti"));
            alternateNames.Add(a("mureş", "mures"));
            alternateNames.Add(a("mureș", "mures"));
            alternateNames.Add(a("neamţ", "neamt"));
            alternateNames.Add(a("satu-mare", "satu_mare"));
            alternateNames.Add(a("satu mare", "satu_mare"));
            alternateNames.Add(a("sălaj", "salaj"));
            alternateNames.Add(a("timiş", "timis"));
            alternateNames.Add(a("vâlcea", "vilcea"));
            alternateNames.Add(a("valcea", "vilcea"));
            alternateNames.Add(a("m.bucureşti", "bucuresti"));
            alternateNames.Add(a("MUNICIPIUL BUCURESTI", "bucuresti"));
            alternateNames.Add(a("bucureşti", "bucuresti"));
            alternateNames.Add(a("sector 1", "bucuresti"));
            alternateNames.Add(a("sector 2", "bucuresti"));
            alternateNames.Add(a("sector 3", "bucuresti"));
            alternateNames.Add(a("sector 4", "bucuresti"));
            alternateNames.Add(a("sector 5", "bucuresti"));
            alternateNames.Add(a("sector 6", "bucuresti"));






            //using (var rep = new Repository<AlternateNamesJudet>())
            //{
            //    await rep.StoreDataAsNew(alternateNames);
            //}
            return alternateNames.ToArray();
        }

    }
}
