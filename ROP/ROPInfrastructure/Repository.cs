using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using ROPObjects;

namespace ROPInfrastructure
{
    public class Repository<T>:IDisposable
        where T:IID
    {
        private string nameType;
        private EmbeddableDocumentStore instance;
        public Repository()
        {
            nameType = typeof (T).FullName;
            instance = new EmbeddableDocumentStore();
            instance.ConnectionStringName = "RavenDB";
            instance.Conventions.FindIdentityPropertyNameFromEntityName = (entity) => "ID";
            instance.Initialize();

        }

        string IndexName(string type)
        {
            return nameType + (type ?? "") + "/ById";
        }
        public bool ExistsData(string type=null)
        {
            string indexName = IndexName(type);
            var index = instance.DatabaseCommands.GetIndex(indexName);
            return (index != null);
        }
        public IEnumerable<T> RetrieveData(string type=null)
        {
            string indexName = IndexName(type);
            if(!ExistsData(type))
                yield break;

            using (var session = instance.OpenSession())
            {

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

        public void DeleteData(string type = null)
        {
            string indexName = IndexName(type);

            if (ExistsData(type))
            {
                instance.DatabaseCommands.DeleteByIndex(indexName, new IndexQuery());
                instance.DatabaseCommands.DeleteIndex(indexName);
            }
        }
        public async Task<long> StoreDataAsNew(T[] data, string type=null)            
        {
            var sw=new Stopwatch();
            sw.Start();
            

            DeleteData(type);
            string indexName = IndexName(type);
            using (var bulkInsert = instance.BulkInsert())
            {

                foreach (var id in data)
                {
                    bulkInsert.Store(id);

                }

            }
            instance.DatabaseCommands.PutIndex(indexName,
                                        new IndexDefinitionBuilder<T>
                                        {
                                            Map = posts => from post in posts
                                                           select new { post.ID }

                                        });
            sw.Stop();
            await Task.Delay(2000);//to can retrieve after
            return sw.ElapsedMilliseconds;
        }

        public void Dispose()
        {
            instance.Dispose();
        }
    }
}
