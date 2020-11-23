using RTX3000_notifier.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTX3000_notifier.Model
{
    class Alternate : IWebsite
    {
        public string Url { get; set; } = "https://www.alternate.nl/Grafische-kaarten/GeForce-RTX-Gaming/html/listings/1534500258044?lk=21501&showFilter=false&hideFilter=false&disableFilter=false&filter_-1=31900&filter_-1=209900&filter_-2=true";

        public Stock GetStock()
        {
            string html = WebsiteDownloader.GetHtml(this.Url);
            Console.WriteLine(html);
            Dictionary<Videocard, int> values = new Dictionary<Videocard, int>();

            foreach (Videocard card in Enum.GetValues(typeof(Videocard)))
            {
                values[card] = this.CheckHtmlForStock(html, card);
            }

            return new Stock(this, values);
        }

        private int CheckHtmlForStock(string html, Videocard card)
        {
            string str = "NVIDIA GeForce RTX ";

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
                str = html.Split(new string[] { str + "&nbsp;(" }, StringSplitOptions.None)[1];
                str = str.Split(new string[] { ")" }, StringSplitOptions.None)[0];
                return int.Parse(str);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
