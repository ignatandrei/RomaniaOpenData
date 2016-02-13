using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raven.Abstractions.Data;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database;
using Raven.Database.Server;
using Raven.Database.Server.Connections;
using Raven.Database.Storage;
using ROPObjects;
using ROPSiruta;

namespace ROPInfrastructure
{
    public class instanceRavenStore
    {
        public static void BackupAllDatabase(string path)
        {
            return;
            instanceDefault.DocumentDatabase.Maintenance.StartBackup(Path.Combine(path, "SYSTEM"), false,
                new DatabaseDocument());
            var def = instanceDefault.DatabaseCommands.ForSystemDatabase().GlobalAdmin.GetDatabaseNames(10000);
            foreach (var s in def)
            {
                Console.WriteLine(s);
                instanceDefault.DatabaseCommands.GlobalAdmin.StartBackup(Path.Combine(path, s), new DatabaseDocument(),
                    false, s);
            }

        }

        private static JudetFinder judFinderCache;
        internal static EmbeddableDocumentStore instanceDefault;

        static instanceRavenStore()
        {

            instanceDefault = new EmbeddableDocumentStore();
            //instanceDefault.Configuration.Settings.Add("Raven/MaxSecondsForTaskToWaitForDatabaseToLoad", "60");
            instanceDefault.ConnectionStringName = "RavenDB";
            instanceDefault.Conventions.FindIdentityPropertyNameFromEntityName = (entity) => "ID";

            try
            {
                //To try in debug mode
                //NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);
                //instanceDefault.UseEmbeddedHttpServer = true;
                instanceDefault.Initialize();

            }
            catch (ReflectionTypeLoadException ex)
            {
                string message = "LoaderExceptions:";
                ex.LoaderExceptions.ForEach(it => message += it.Message + ";");
                throw new Exception(message, ex);
            }


            //instanceDefault.DatabaseCommands.GlobalAdmin.StartBackup(@"D:\a\", new DatabaseDocument(), false, );
        }

        static JudetFinder judFinder
        {
            get
            {
                if (judFinderCache != null)
                {
                    return judFinderCache;
                }
                lock (instanceDefault)
                {
                    var jud = Judete().Result;
                    judFinderCache = new JudetFinder();
                    judFinderCache.judete = jud;
                    judFinderCache.altNumeJudet = GetAlternate(jud);
                    return judFinderCache;
                }
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
            alternateNames.Add(a("Botoşani", "Botosani"));
            alternateNames.Add(a("braşov", "Brasov"));
            alternateNames.Add(a("brăila", "braila"));
            alternateNames.Add(a("buzău", "buzau"));
            alternateNames.Add(a("caraş-s.", "CARAS-SEVERIN"));
            alternateNames.Add(a("călăraşi", "calarasi"));
            alternateNames.Add(a("constanţa", "Constanta"));
            alternateNames.Add(a("dâmboviţa", "dimbovita"));
            alternateNames.Add(a("dambovita", "dimbovita"));
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
            alternateNames.Add(a("valcea", "vilcea"));
            alternateNames.Add(a("m.bucureşti", "bucuresti"));





            //using (var rep = new Repository<AlternateNamesJudet>())
            //{
            //    await rep.StoreDataAsNew(alternateNames);
            //}
            return alternateNames.ToArray();
        }

        public static async Task<Judet[]> Judete()
        {
            if (judFinderCache != null)
            {
                return judFinderCache.judete;
            }
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

        public static async Task<RopDataSaved[]> DataSaved()
        {
            var listTasks = new List<Task<IRopLoader>>();
            using (var rep = new Repository<RopDataSaved>())
            {

                var data = rep.RetrieveData();
                if (data != null)
                {
                    var arrData = data.ToArray();

                    return arrData;

                }

            }
            return null;

        }

        public static async Task<RopDocument[]> GetOrLoad(RopDataSaved data)
        {
            var type = Type.GetType(data.Name);
            return await GetOrLoad(type);
        }

        public static async Task<RopDocument[]> GetOrLoad(Type typeIRopLoader)
        {
            //await Task.Delay(1000);
            var loaderData = Activator.CreateInstance(typeIRopLoader) as IRopLoader;
            loaderData.Init(judFinder);
            var d = await loaderData.GetData();
            return d;
        }

        public static async Task<RopDocument[]> GetOrLoad(string typeName)
        {
            var type = Type.GetType(typeName);
            return await GetOrLoad(type);
        }
    }

    public class Repository<T>:IDisposable
        where T:IID
    {
        public string Type { get; set; }
        private string FullNameType;

        static EmbeddableDocumentStore instanceDefault;

        static Repository()
        {
            instanceDefault = instanceRavenStore.instanceDefault;

        } 

        

        async Task<string> CreateDatabase()
        {
            string dbName = DatabaseName();
            if (ExistsData())
                return dbName; 
            
            var settingsRep = new Dictionary<string, string>();
            settingsRep.Add("Raven/DataDir",  dbName);
            settingsRep.Add("Raven/Voron/TempPath", dbName + "/Temp");
            

            instanceDefault.DatabaseCommands.GlobalAdmin.CreateDatabase(new DatabaseDocument()
            {
                Id = DatabaseName(),
                Settings = settingsRep

            });
            instanceDefault.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(dbName);
            
            return dbName;

        }
        public Repository(string type = null)
        {
            Type = type;
            FullNameType = typeof(T).FullName;
            //ConnectInstance(null);
            //var settings2 = new Dictionary<string, string>();
            //settings2.Add("Raven/DataDir", "~/Databases/andrei2");
            //instance.DatabaseCommands.GlobalAdmin.CreateDatabase(new DatabaseDocument()
            //{
            //    Id="andrei2",
            //    Settings =settings2

            //});
        }
        

        string DatabaseName()
        {
            return  (FullNameType + (Type ?? "")).Replace(".", "_");
        }
        public bool ExistsData()
        {
            //string indexName = IndexName();
            //var index = instance.DatabaseCommands.GetIndex(indexName);
            //return (index != null);
            string databaseName = DatabaseName();
            var dbNames = instanceDefault.DatabaseCommands.GlobalAdmin.GetDatabaseNames(10000);
            return (dbNames.Contains(DatabaseName()));

        }

        public T GetFromId(string id)
        {
            if (!ExistsData())
                return default(T);


            using (var session = instanceDefault.OpenSession(DatabaseName()))
            {
                //var indexName = new RavenDocumentsByEntityName().IndexName;

                //var query = session.Query<T>(indexName);
                return session.Load<T>(id);
            }
        }

        public IEnumerable<T> RetrieveData()
        {
            
            if(!ExistsData())
                yield break;


            //using (var instance = ConnectInstance(DatabaseName()))
            {

                var config = instanceDefault.DatabaseCommands.ForDatabase(DatabaseName()).Admin.GetDatabaseConfiguration();                                
                using (var session = instanceDefault.OpenSession(DatabaseName()))
                {
                    session.Advanced.UseOptimisticConcurrency = true;
                    var indexName = new RavenDocumentsByEntityName().IndexName;

                    var query = session.Query<T>(indexName);
                    using (var res = session.Advanced.Stream(query))
                    {
                        while (res.MoveNext())
                        {
                            yield return res.Current.Document;
                        }
                    }


                }
            }
        }

        public void DeleteData()
        {
            if (ExistsData())
            {
                instanceDefault.DatabaseCommands.GlobalAdmin.DeleteDatabase(DatabaseName(),true);
                
            }
        }
        public async Task<long> StoreDataAsNew(T[] data)            
        {
            var sw=new Stopwatch();
            sw.Start();
            
            DeleteData();
            await CreateDatabase();
            //using (var instance = ConnectInstance(DatabaseName()))
            {
                using (var bulkInsert = instanceDefault.BulkInsert(DatabaseName()))
                {

                    foreach (var id in data)
                    {
                        bulkInsert.Store(id, id.ID);

                    }

                }
                
                var dbc = instanceDefault.DatabaseCommands.ForDatabase(DatabaseName());
                var defIndex = new RavenDocumentsByEntityName();
                
                defIndex.Execute(dbc,new DocumentConvention()
                {
                    DefaultUseOptimisticConcurrency = true                    
                    
                } );
                instanceDefault.ExecuteIndex(defIndex);
            }
            sw.Stop();

            await Task.Delay(2000);//to can retrieve after
            return sw.ElapsedMilliseconds;
        }

        public void Dispose()
        {
            
        }
    }
}
