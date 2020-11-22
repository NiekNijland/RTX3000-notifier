using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RTX3000_notifier.Helper
{
    static class Constants
    {
        private static Dictionary<string, string> values;

        private static void ReadJson()
        {
            if(values == null)
            {
                try
                {
                    using StreamReader file = new StreamReader("constants.json");
                    values = JsonConvert.DeserializeObject<Dictionary<string, string>>(file.ReadToEnd());
                }
                catch (Exception)
                {
                    throw new Exception("Error reading constants.json");
                }
            }
        }

        public static string GetMongoDatabaseName()
        {
            ReadJson();

            if (values.ContainsKey("mongodbdatabasename"))
            {
                return values["mongodbdatabasename"];
            }
            else
            {
                return "";
            }
        }

        public static string GetMongoConnectionString()
        {
            ReadJson();

            if (values.ContainsKey("mongdbconnectionstring"))
            {
                return values["mongdbconnectionstring"];
            }
            else
            {
                return "";
            }
        }

        public static int GetReloadInterval()
        {
            ReadJson();

            int retVal = 60000;

            if (values.ContainsKey("reloadinterval"))
            {
                int.TryParse(values["reloadinterval"], out retVal);
            }

            return retVal;
        }
    }
}
