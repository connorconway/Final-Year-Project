using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Multiplayer_Software_Game_Engineering.Handlers
{
    class DataBaseHandler
    {
        private const string connectionString = "mongodb://ec2-54-154-20-112.eu-west-1.compute.amazonaws.com:27017";


        public static void CreateCollection(String database, String collection)
        {
            try
            {
                MongoClient client = new MongoClient(connectionString); 
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
            MongoClient client = new MongoClient(connectionString); 
            MongoServer server = client.GetServer();
            server.Connect();
            MongoDatabase test = server.GetDatabase(database);
            test.DropCollection(collection);
        }

        public static void InputData(String database, String collection, String username, String data)
        {
            MongoClient client = new MongoClient(connectionString);
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


            MongoClient client = new MongoClient(connectionString); 
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