using System;
using System.Collections.Generic;
using System.Text;
using RTX3000_notifier.Helper;
using System.Net.Http;

namespace RTX3000_notifier.Model
{
    class Azerty : Website
    {
        public string Url { get; set; } = "https://azerty.nl/category/componenten/videokaarten/nvidia_geforce#!sorting=15&limit=96&view=rows&Videochip_generatie=GeForce_3000&levertijd=green";


        private string GetHtml()
        {
            var values = new Dictionary<string, string>
            {
                {"data", "{\"service\":\"getFilterOptions\",\"route\":[\"lister\",\"componenten\",\"videokaarten\",\"nvidia_geforce \"],\"params\":{ \"navigation\":\"190\",\"keywords\":\"\",\"keyData\":\"U1RLb01jeFdrRUhZeUFieis4RE5zY0o1VHJGVTZPd0hyVWlreTlON1NJRVVGRlZDWjlZbDZ0RGdoRWVia0xMSTJMdURma1N0ZFVWUFRlcEZYV0lGN0pRdkwvSE9wMWduYWhZbCthcFo0SjRwMVJuUFZ1dUVUVWh5NncyWG5VeUJxeDc4a2lCbFBkei8zTkV6YUFZM0sxcXNlRTE4SFhoTWREbS9QcFBsVmp3PQ==\"},\"state\":{ \"sorting\":\"15\",\"limit\":\"96\",\"view\":\"rows\",\"levertijd\":\"green\",\"Videochip_generatie\":\"GeForce_3000\",\"filters\":\"levertijd:green+Videochip_generatie:GeForce_3000\"},\"callID\":1}" }
            };

            var content = new FormUrlEncodedContent(values);

            return WebsiteDownloader.PostHtml("https://azerty.nl/system/modules/ajax/lib/webservice/load.php", content);
        }

        public Stock GetStock()
        {
            string html = GetHtml();

            try
            {
                html = html.Replace(@"\", string.Empty);
                html = html.Split(new string[] { "filter_Videochip_videokaarten" }, StringSplitOptions.None)[1];
                html = html.Split(new string[] { "</ul>" }, StringSplitOptions.None)[0];
            }
            catch (Exception)
            {

            }

            Dictionary<Videocard, int> values2 = new Dictionary<Videocard, int>();

            foreach (Videocard card in Enum.GetValues(typeof(Videocard)))
            {
                values2[card] = this.CheckHtmlForStock(html, card);
            }

            return new Stock(this, values2);
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

            if(html != "")
            {
                try
                {
                    str = html.Split(new string[] { str + "                    <div class=\"count\">(" }, StringSplitOptions.None)[1].ToString();
                    str = str.Split(new string[] { ")</div>" }, StringSplitOptions.None)[0].ToString();
                    return int.Parse(str);
                }
                catch
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
