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
            Get3070Stock(values);
            Get3080Stock(values);
            Get3090Stock(values);
            return new Stock(this, values);
        }

        private void Get3070Stock(Dictionary<Videocard, int> values)
        {
            string html = WebsiteDownloader.GetHtml(GetProductUrl(Videocard.RTX3070));

            try
            {
                html = html.Replace(@"\", string.Empty);
                var splittedHtml = html.Split("<img class=\"picture__image");
                int counter = splittedHtml.Where(o => o.Contains("RTX 3070") && !o.Contains("Binnenkort leverbaar") && !o.Contains("Tijdelijk uitverkocht") && !o.Contains("DOCTYPE")).Count();
                values.Add(Videocard.RTX3070, counter);
            }
            catch (Exception)
            { }
        }

        private void Get3080Stock(Dictionary<Videocard, int> values)
        {
            string html = WebsiteDownloader.GetHtml(GetProductUrl(Videocard.RTX3080));

            try
            {
                html = html.Replace(@"\", string.Empty);
                var splittedHtml = html.Split("<img class=\"picture__image");
                int counter = splittedHtml.Where(o => o.Contains("RTX 3080") && !o.Contains("Binnenkort leverbaar") && !o.Contains("Tijdelijk uitverkocht") && !o.Contains("DOCTYPE")).Count();
                values.Add(Videocard.RTX3080, counter);
            }
            catch (Exception)
            { }
        }

        private void Get3090Stock(Dictionary<Videocard, int> values)
        {
            string html = WebsiteDownloader.GetHtml(GetProductUrl(Videocard.RTX3090));

            try
            {
                html = html.Replace(@"\", string.Empty);
                var splittedHtml = html.Split("<img class=\"picture__image");
                int counter = splittedHtml.Where(o => o.Contains("RTX 3090") && !o.Contains("Binnenkort leverbaar") && !o.Contains("Tijdelijk uitverkocht") && !o.Contains("DOCTYPE")).Count();
                values.Add(Videocard.RTX3090, counter);
            }
            catch (Exception)
            { }
        }
    }
}
