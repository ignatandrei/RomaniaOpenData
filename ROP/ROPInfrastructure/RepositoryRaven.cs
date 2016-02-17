using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database;
using Raven.Database.Server;
using Raven.Database.Server.Connections;
using Raven.Database.Storage;
using ROPObjects;

namespace ROPInfrastructure
{
    public class RepositoryRaven<T> : IDisposable, IRepository<T> where T:IID
    {
        public string Type { get; set; }
        private string FullNameType;

        static EmbeddableDocumentStore instanceDefault;

        static RepositoryRaven()
        {
            instanceDefault = RepositoryRaven.instanceDefault;

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
        public RepositoryRaven(string type = null)
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
