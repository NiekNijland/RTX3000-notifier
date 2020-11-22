using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Net.Http;

namespace RTX3000_notifier.Helper
{
    static class WebsiteDownloader
    {
        public static string GetHtml(string url)
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead(url))
                using (var textReader = new StreamReader(stream, Encoding.UTF8, true))
                {
                    return textReader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error downloading html with GET");
                return "";
            }
        }

        public static string PostHtml(string url, HttpContent content)
        {
            try
            {
                var response = new HttpClient().PostAsync(url, content).Result;

                string html = response.Content.ReadAsStringAsync().Result;
                return (html);
            }
            catch (Exception)
            {
                Console.WriteLine("Error downloading html with POST");
                return ("");
            }
        }
    }
}
