using System;
using System.Collections.Generic;
using System.Text;
using RTX3000_notifier.Helper;

namespace RTX3000_notifier.Model
{
    public class Informatique : IWebsite
    {
        public string Url { get; set; } = "https://www.informatique.nl/?m=sts&g=166&p=&sort=&ss=2&pr_min=&pr_max=";

        public Stock GetStock()
        {
            string html = WebsiteDownloader.GetHtml(this.Url);
            Dictionary<Videocard, int> values = new Dictionary<Videocard, int>();

            try
            {
                html = html.Split(new string[] { "<h3>Chipset (GPU)</h3><ul><li>" }, StringSplitOptions.None)[1];
                html = html.Split(new string[] { "</ul>" }, StringSplitOptions.None)[0];
            }
            catch
            {

            }

            foreach (Videocard card in Enum.GetValues(typeof(Videocard)))
            {
                values[card] = this.CheckHtmlForStock(html, card);
            }

            return new Stock(this, values);
        }

        private int CheckHtmlForStock(string html, Videocard card)
        {
            string str = "RTX ";

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
            if (html != "")
            {
                try
                {
                    str = html.Split(new string[] { str + "  <span class=\"attr_count\">(" }, StringSplitOptions.None)[1];
                    str = str.Split(new string[] { ")" }, StringSplitOptions.None)[0];
                    return int.Parse(str);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }
    }
}
