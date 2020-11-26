using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RTX3000_notifier.Helper;

namespace RTX3000_notifier.Model
{
    class Cyberport : IWebsite
    {
        public string Url { get; set; } = "https://www.cyberport.de/pc-und-zubehoer/komponenten/grafikkarten/nvidia-fuer-gaming.html?productsPerPage=100&sort=popularity&2E_Grafikchip=GeForce%20RTX%203070,GeForce%20RTX%203080,GeForce%20RTX%203090&page=1&stockLevelStatus=IMMEDIATELY";

        public string GetProductUrl(Videocard card)
        {
            return card switch
            {
                Videocard.RTX3070 => "https://www.cyberport.de/pc-und-zubehoer/komponenten/grafikkarten/nvidia-fuer-gaming.html?productsPerPage=50&sort=popularity&2E_Grafikchip=GeForce%20RTX%203070&page=1&stockLevelStatus=IMMEDIATELY",
                Videocard.RTX3080 => "https://www.cyberport.de/pc-und-zubehoer/komponenten/grafikkarten/nvidia-fuer-gaming.html?productsPerPage=50&sort=popularity&2E_Grafikchip=GeForce%20RTX%203080&page=1&stockLevelStatus=IMMEDIATELY",
                Videocard.RTX3090 => "https://www.cyberport.de/pc-und-zubehoer/komponenten/grafikkarten/nvidia-fuer-gaming.html?productsPerPage=50&sort=popularity&2E_Grafikchip=GeForce%20RTX%203090&page=1&stockLevelStatus=IMMEDIATELY",
                _ => Url,
            };
        }

        public Stock GetStock()
        {
            Dictionary<Videocard, int> values = new Dictionary<Videocard, int>();
            values.Add(Videocard.RTX3070, GetStock(GetProductUrl(Videocard.RTX3070), "RTX 3070", values));
            values.Add(Videocard.RTX3080, GetStock(GetProductUrl(Videocard.RTX3080), "RTX 3080", values));
            values.Add(Videocard.RTX3090, GetStock(GetProductUrl(Videocard.RTX3090), "RTX 3090", values));
            return new Stock(this, values);
        }

        private int GetStock(string url, string name, Dictionary<Videocard, int> values)
        {
            string html = GetHTML(url).Result;

            try
            {
                html = html.Replace(@"\", string.Empty);
                var splittedHtml = html.Split("<article class=\" productArticle\"");
                var filteredPerName = splittedHtml.Where(o => o.Contains(name)).ToList();
                var filtered = filteredPerName.Where(o => !o.Contains("DOCTYPE") && o.Contains("Sofort verfügbar")).ToList();
                return filtered.Count();
            }
            catch (Exception)
            { }
            return -1;
        }

        private static async Task<string> GetHTML(string url)
        {
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                AutomaticDecompression = ~DecompressionMethods.None
            };
            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                {
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:83.0) Gecko/20100101 Firefox/83.0");
                    request.Headers.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.5");
                    request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                    var response = await httpClient.SendAsync(request);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
