using RestSharp;
using RTX3000_notifier.Helper;
using System;
using System.Collections.Generic;

namespace RTX3000_notifier.Model
{
    class Cdromland : IWebsite
    {
        public string Url { get; set; } = "https://cdromland.nl/";

        private string url = "https://www.cdromland.nl/setfilter.php";

        public string GetProductUrl(Videocard card)
        {
            return Url;
        }

        public Stock GetStock()
        {
            string html = DownloadHtml();

            Dictionary<Videocard, int> values2 = new Dictionary<Videocard, int>();

            foreach (Videocard card in Enum.GetValues(typeof(Videocard)))
            {
                values2[card] = this.CheckHtmlForStock(html, card);
            }

            return new Stock(this, values2);
        }

        private string DownloadHtml()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>() { { "main", "20" }, { "sub", "3" }, { "levertijd", "3" } };
            string result = "";
            try
            {
                var client = new RestClient(this.url);
                client.AddDefaultHeader("Content-type", "application/x-www-form-urlencoded");
                var request = new RestRequest();
                request.AddHeader("Content-type", "application/x-www-form-urlencoded");

                foreach (KeyValuePair<string, string> pair in parameters)
                {
                    request.AddParameter(pair.Key, pair.Value);
                }

                IRestResponse response = client.Post(request);
                result = response.Content;
            }
            catch (Exception)
            {
                Logger.HtmlDownloadError(url);
            }
            return result;
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

            if (html != "")
            {
                try
                {
                    str = html.Split(new string[] { str + "&nbsp;&nbsp;(" }, StringSplitOptions.None)[1].ToString();
                    str = str.Split(new string[] { ")</label>" }, StringSplitOptions.None)[0].ToString();
                    return int.Parse(str);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                Logger.HtmlStockCheckError(this);
                return -1;
            }
        }
    }
}
