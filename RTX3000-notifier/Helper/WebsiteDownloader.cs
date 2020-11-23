using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Http;
using RTX3000_notifier.Model;
using RestSharp;

namespace RTX3000_notifier.Helper
{
    static class WebsiteDownloader
    {
        public static string GetHtml(string url)
        {
            try
            {
                WebClient client = new WebClient();

                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                Stream data = client.OpenRead(url);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();

                data.Close();
                reader.Close();

                return s;
            }
            catch (Exception)
            {
                Logger.HtmlDownloadGetError(url);
                return "";
            }
        }


    }
}
