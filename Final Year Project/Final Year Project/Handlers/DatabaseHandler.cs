using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Wrappers;

namespace Multiplayer_Software_Game_Engineering.Handlers
{
    class DataBaseHandler
    {
        public static void CreateCollection(String database, String collection)
        {
            try
            {
                MongoClient client = new MongoClient(); // connect to localhost
                MongoServer server = client.GetServer();
                server.Connect();
                MongoDatabase test = server.GetDatabase(database);
                test.CreateCollection(collection);
            }
            catch (Exception)
            {
                throw;
            }        
        }

        public void DropCollection(String database, String collection)
        {
            MongoClient client = new MongoClient(); // connect to localhost
            MongoServer server = client.GetServer();
            server.Connect();
            MongoDatabase test = server.GetDatabase(database);
            test.DropCollection(collection);
        }

        public static void InputData(String database, String collection, String username, String data)
        {
            MongoClient client = new MongoClient(); // connect to localhost
            MongoServer server = client.GetServer();
            MongoDatabase test = server.GetDatabase(database);

            var GetFromCollection = test.GetCollection<BsonDocument>(collection);

            BsonDocument book = new BsonDocument
            {
                {"Username", username }, 
                {"High Score", data},
                {"Time", DateTime.Now.ToString("MM/dd/yyyy h:mm tt") }
            };

            GetFromCollection.Insert(book);
        }

        public static List<Tuple<string, int>> ReadData(String database, String collection)
        {
            List<Tuple<string, int>> data = new List<Tuple<string, int>>();


            MongoClient client = new MongoClient(); // connect to localhost
            MongoServer server = client.GetServer();
            MongoDatabase test = server.GetDatabase(database);

            var GetFromCollection = test.GetCollection(collection);

            var query = from e in GetFromCollection.AsQueryable()
                            select e;

            foreach (var entry in query)
            {
                data.Add(Tuple.Create(entry[1].ToString(), entry[2].ToInt32()));
            }

            return data;
        }
    }
}
