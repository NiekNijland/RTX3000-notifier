using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using RTX3000_notifier.Model;

namespace RTX3000_notifier.Helper
{
    static public class Mongo
    {
        private static readonly IMongoDatabase database = new MongoClient(Constants.GetMongoConnectionString()).GetDatabase(Constants.GetMongoDatabaseName());

        public async static void InsertStock(Stock stock)
        {
            var collection = database.GetCollection<BsonDocument>("stock");

            BsonDocument document = new BsonDocument();

            BsonElement timestamp = new BsonElement("timestamp", new BsonDateTime(stock.Timestamp));
            document.Add(timestamp);

            BsonDocument website = new BsonDocument();
            website.Add(new BsonElement("name", stock.Website.GetType().Name));
            website.Add(new BsonElement("url", stock.Website.Url));
            document.Add(new BsonElement("website", website));

            var jsonDoc = Newtonsoft.Json.JsonConvert.SerializeObject(stock.Values);
            BsonElement values = new BsonElement("values", BsonDocument.Parse(jsonDoc));
            document.Add(values);

            try
            {
                await collection.InsertOneAsync(document);
            }
            catch (Exception)
            {
                Console.WriteLine("Error interacting with MongoDB");
                return;
            }
        }
    }
}
