using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace RTX3000_notifier.Helper
{
    static class Constants
    {
        public static Dictionary<string, string> values = ReadJson();

        private static Dictionary<string, string> ReadJson()
        {
            try
            {
                using StreamReader file = new StreamReader("constants.json");
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(file.ReadToEnd());
            }
            catch (Exception)
            {
                throw new Exception("Error reading constants.json");
            }
        }

        public static string GetMongoDatabaseName()
        {
            if (values.ContainsKey("mongodbdatabasename"))
            {
                return values["mongodbdatabasename"];
            }
            else
            {
                Logger.JsonReadError("mongodbdatabasename");
                return "";
            }
        }

        public static string GetMongoConnectionString()
        {
            if (values.ContainsKey("mongdbconnectionstring"))
            {
                return values["mongdbconnectionstring"];
            }
            else
            {
                Logger.JsonReadError("mongodbconnectionstring");
                return "";
            }
        }

        public static int GetReloadInterval()
        {
            int retVal = -1;

            if (values.ContainsKey("reloadinterval"))
            {
                int.TryParse(values["reloadinterval"], out retVal);
            }

            if(retVal == -1)
            {
                Logger.JsonReadError("reloadinterval");
                retVal = 600000;
            }

            return retVal;
        }

        public static string GetEmailHost()
        {
            if (values.ContainsKey("emailhost"))
            {
                return values["emailhost"];
            }
            else
            {
                Logger.JsonReadError("emailhost");
                return "";
            }
        }

        public static int GetEmailPort()
        {
            int port = -1;

            if (values.ContainsKey("emailport"))
            {
                int.TryParse(values["emailport"], out port);
            }

            if (port == -1)
            {
                Logger.JsonReadError("reloadinterval");
                port = 587;
            }

            return port;
        }

        public static string GetEmailUsername()
        {
            if (values.ContainsKey("emailusername"))
            {
                return values["emailusername"];
            }
            else
            {
                Logger.JsonReadError("emailusername");
                return "";
            }
        }

        public static string GetEmailPassword()
        {
            if (values.ContainsKey("emailpassword"))
            {
                return values["emailpassword"];
            }
            else
            {
                Logger.JsonReadError("emailpassword");
                return "";
            }
        }

        public static string GetErrorLogAddress()
        {
            if (values.ContainsKey("errorlogaddress"))
            {
                return values["errorlogaddress"];
            }
            else
            {
                Logger.JsonReadError("errorlogaddress");
                return "";
            }
        }

        public static bool GetVerboseMode()
        {
            bool ret = false;
            if (values.ContainsKey("verbosemode"))
            {
                Boolean.TryParse(values["verbosemode"], out ret);
            }
            return ret;
        }
    }
}
