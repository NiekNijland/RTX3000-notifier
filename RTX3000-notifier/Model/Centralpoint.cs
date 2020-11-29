using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using RTX3000_notifier.Helper;

namespace RTX3000_notifier.Model
{
    class Centralpoint : IWebsite
    {
        public string Url { get; set; } = "https://www.centralpoint.nl/videokaarten/?Sorting=stockDESC&facet_716=GeForce+RTX+3070^GeForce+RTX+3080^GeForce+RTX+3090";

        public string GetProductUrl(Videocard card)
        {
            return card switch
            {
                Videocard.RTX3070 => "https://www.centralpoint.nl/videokaarten/?Sorting=stockDESC&facet_716=GeForce+RTX+3070",
                Videocard.RTX3080 => "https://www.centralpoint.nl/videokaarten/?Sorting=stockDESC&facet_716=GeForce+RTX+3080",
                Videocard.RTX3090 => "https://www.centralpoint.nl/videokaarten/?Sorting=stockDESC&facet_716=GeForce+RTX+3090",
                _ => Url,
            };
        }

        public Stock GetStock()
        {
            Dictionary<Videocard, int> values = new Dictionary<Videocard, int>();
            GetStock(Videocard.RTX3070, "RTX 3070", values);
            GetStock(Videocard.RTX3080, "RTX 3080", values);
            GetStock(Videocard.RTX3090, "RTX 3090", values);
            return new Stock(this, values);
        }

        private void GetStock(Videocard card, string name, Dictionary<Videocard, int> values)
        {
            string html = WebsiteDownloader.GetHtml(GetProductUrl(card));

            try
            {
                html = html.Replace(@"\", string.Empty);
                var splittedHtml = html.Split("<div class=\"card landscape wide\">");
                var filteredByName = splittedHtml.Where(o => o.Contains(name) && !o.Contains("DOCTYPE")).ToList();
                var filtered = filteredByName.Where(o => !o.Contains("Bericht mij bij voorraad")).ToList();
                values.Add(card, filtered.Count());
            }
            catch (Exception)
            { }
        }
    }
}
