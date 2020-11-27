using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using RTX3000_notifier.Helper;

namespace RTX3000_notifier.Model
{
    class Coolblue : IWebsite
    {
        public string Url { get; set; } = "https://www.coolblue.nl/videokaarten/nvidia-chipset/nvidia-geforce-rtx-3000-serie";

        public string GetProductUrl(Videocard card)
        {
            return card switch
            {
                Videocard.RTX3070 => "https://www.coolblue.nl/videokaarten/nvidia-chipset/nvidia-rtx-3070",
                Videocard.RTX3080 => "https://www.coolblue.nl/videokaarten/nvidia-chipset/nvidia-geforce-rtx-3000-serie/nvidia-geforce-rtx-3080",
                Videocard.RTX3090 => "https://www.coolblue.nl/videokaarten/nvidia-chipset/nvidia-geforce-rtx-3000-serie/nvidia-geforce-rtx-3090",
                _ => Url,
            };
        }

        public Stock GetStock()
        {
            Dictionary<Videocard, int> values = new Dictionary<Videocard, int>();
            GetStock(Videocard.RTX3070, "RTX 3070", values);
            GetStock(Videocard.RTX3080, "RTX 3080", values);
            GetStock(Videocard.RTX3090, "RTX 3080", values);
            return new Stock(this, values);
        }

        private void GetStock(Videocard card, string name, Dictionary<Videocard, int> values)
        {
            string html = WebsiteDownloader.GetHtml(GetProductUrl(card));

            try
            {
                html = html.Replace(@"\", string.Empty);
                var splittedHtml = html.Split("<div class=\"product-card\n");
                var filteredByName = splittedHtml.Where(o => o.Contains(name) && !o.Contains("DOCTYPE")).ToList();
                var filtered = filteredByName.Where(o => !o.Contains("Binnenkort leverbaar") && !o.Contains("Tijdelijk uitverkocht")).ToList();
                values.Add(card, filtered.Count());
            }
            catch (Exception)
            { }
        }
    }
}
