using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using RTX3000_notifier.Model;

namespace RTX3000_notifier.Helper
{
    static public class Mongo
    {
        private static readonly IMongoDatabase database = new MongoClient(Constants.GetMongoConnectionString()).GetDatabase(Constants.GetMongoDatabaseName());

        public async static void InsertStock(Stock stock)
        {
            var collection = database.GetCollection<BsonDocument>("stocks");

            BsonDocument document = new BsonDocument();

            BsonElement timestamp = new BsonElement("created_at", new BsonDateTime(stock.Timestamp));
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

        public static List<Subscriber> GetSubscribers()
        {
            List<Subscriber> ret = new List<Subscriber>();
            var collection = database.GetCollection<BsonDocument>("subscribers");
            List<BsonDocument> documents = collection.Find(new BsonDocument()).ToList();

            foreach (BsonDocument document in documents)
            {
                List<Videocard> interests = new List<Videocard>();

                foreach(KeyValuePair<string, string> pair in JsonConvert.DeserializeObject<Dictionary<string, string>>(document.GetValue("cards").ToString()))
                {
                    if(pair.Value == "true")
                    {
                        Videocard card;
                        if (Enum.TryParse(pair.Key, out card))
                        {
                            if (Enum.IsDefined(typeof(Videocard), card))
                            {
                                interests.Add(card);
                            }
                        }
                    }
                }

                Subscriber newSub = new Subscriber(document.GetValue("_id").ToString(), document.GetValue("email").ToString(), interests);
                ret.Add(newSub);
            }

            return ret;
        }
    }
}
