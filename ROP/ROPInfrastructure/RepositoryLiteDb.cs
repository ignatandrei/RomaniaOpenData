using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiteDB;
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
    public class Repository
    {
        public static async Task<RopDocument[]> GetOrLoad(string type)
        {
            var t = Type.GetType(type);
            return await GetOrLoad(t);
        }
        public static async Task<RopDocument[]> GetOrLoad(RopDataSaved data)
        {
            var type = Type.GetType(data.Name);
            return await GetOrLoad(type);
        }
        public static async Task<RopDocument[]> GetOrLoad(IRopLoader loaderData)
        {
            loaderData.Init(JudeteLoader.judFinder);
            var d = await loaderData.GetData();
            return d;
        }
        public static async Task<RopDocument[]> GetOrLoad(Type typeIRopLoader)
        {
            
            var loaderData = Activator.CreateInstance(typeIRopLoader) as IRopLoader;
            return await GetOrLoad(loaderData);
        }

    }
    public class RepositoryLiteDb<T> : IDisposable, IRepository<T> where T:IID,new()
    {
        public string Type { get; set; }
        private string FullNameType;
        private static string pathData;
        static RepositoryLiteDb()
        {
            pathData=AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            if (!Directory.Exists(pathData))
                Directory.CreateDirectory(pathData);

        }

        async Task<string> CreateDatabase()
        {
            string dbName = DatabaseName();
            if (ExistsData())
                return dbName;

            using (var db = new LiteDatabase(Path.Combine(pathData, dbName + ".db")))
            {
                db.Commit();
            }

            return dbName;

        }
        public RepositoryLiteDb(string type = null)
        {
            Type = type;
            FullNameType = typeof(T).FullName;
                     
        }
        

        string DatabaseName()
        {
            return  (FullNameType + (Type ?? "")).Replace(".", "_");
        }

       
        public bool ExistsData()
        {
            string dbName = DatabaseName();
            using (var db = new LiteDatabase(Path.Combine(pathData, dbName + ".db")))
            {

                return db.CollectionExists(nameCollection());
            }

        }

        public T GetFromId(string id)
        {
            if (!ExistsData())
                return default(T);

            string dbName = DatabaseName();
            using (var db = new LiteDatabase(Path.Combine(pathData, dbName + ".db")))
            {
                var col = db.GetCollection<T>(nameCollection());
                return col.FindOne(it => it.ID == id);
            }
        }

        public IEnumerable<T> RetrieveData()
        {
            
            if(!ExistsData())
                yield break;
            var dbName = DatabaseName();
            using (var db = new LiteDatabase(Path.Combine(pathData, dbName + ".db")))
            {
                var col = db.GetCollection<T>(nameCollection());
                foreach (var item in col.FindAll())
                {
                    yield return item;
                }

            }


        }

        public void DeleteData()
        {
            if (ExistsData())
            {
                var dbName = DatabaseName();
                using (var db = new LiteDatabase(Path.Combine(pathData, dbName + ".db")))
                {
                    db.DropCollection(nameCollection());
                    db.Commit();
                }
                
            }
        }

        public async Task<long> StoreDataAsNew(T[] data)
        {
            var sw = new Stopwatch();
            sw.Start();

            DeleteData();
            await CreateDatabase();
            var dbName = DatabaseName();
            using (var db = new LiteDatabase(Path.Combine(pathData, dbName + ".db")))
            {
                var col = db.GetCollection<T>(nameCollection());
                col.InsertBulk(data);
                col.EnsureIndex(x => x.ID);
                db.Commit();
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        string nameCollection()
        {
            return typeof (T).Name;
        }

        public void Dispose()
        {
            
        }
    }
}
