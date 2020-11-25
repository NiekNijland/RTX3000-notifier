using System;
using System.Collections.Generic;
using RTX3000_notifier.Helper;

namespace RTX3000_notifier.Model
{
    class Megekko : IWebsite
    {
        public string Url { get; set; } = "https://www.megekko.nl/Computer/Componenten/Videokaarten/Nvidia-Videokaarten?f=f_vrrd-3_s-prijs09_pp-50_p-1_d-list_cf-";
    
        public string GetProductUrl(Videocard card)
        {
            return card switch
            {
                Videocard.RTX3070 => "https://www.megekko.nl/Computer/Componenten/Videokaarten/Nvidia-Videokaarten/Graphics-Engine/GeForce-RTX-3070?f=f_vrrd-0_s-populair_pp-50_p-1_d-list_cf-",
                Videocard.RTX3080 => "https://www.megekko.nl/Computer/Componenten/Videokaarten/Nvidia-Videokaarten/Graphics-Engine/GeForce-RTX-3080?f=f_vrrd-1_s-populair_pp-50_p-1_d-list_cf-",
                Videocard.RTX3090 => "https://www.megekko.nl/Computer/Componenten/Videokaarten/Nvidia-Videokaarten/Graphics-Engine/GeForce-RTX-3090?f=f_vrrd-1_s-populair_pp-50_p-1_d-list_cf-",
                _ => Url,
            };
        }

        public Stock GetStock()
        {
            string html = WebsiteDownloader.GetHtml(this.Url);
            Dictionary<Videocard, int> values = new Dictionary<Videocard, int>();

            foreach(Videocard card in Enum.GetValues(typeof(Videocard)))
            {
                values[card] = this.CheckHtmlForStock(html, card);
            }

            return new Stock(this, values);
        }

        private int CheckHtmlForStock(string html, Videocard card)
        {
            string str = "GeForce RTX ";

            switch (card)
            {
                case Videocard.RTX3070:
                    str += "3070";
                    break;
                case Videocard.RTX3080:
                    str += "3080";
                    break;
                case Videocard.RTX3090:
                    str += "3090";
                    break;
            }

            try
            {
                str = html.Split(new string[] { str + " <span" }, StringSplitOptions.None)[1];
                str = str.Split(new string[] { ">(" }, StringSplitOptions.None)[1];
                str = str.Split(new string[] { ")</span>" }, StringSplitOptions.None)[0];
                return int.Parse(str);
            }
            catch (Exception)
            {
                return -1;
            }
        }  
    }
}
