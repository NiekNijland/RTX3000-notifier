using System;
using System.Collections.Generic;
using System.Linq;
using RTX3000_notifier.Helper;

namespace RTX3000_notifier.Model
{
    class Amazon : IWebsite
    {
        public string Url { get; set; } = "https://www.amazon.nl/s?k=Grafische+kaart+RTX+3070&i=electronics&__mk_nl_NL=%C3%85M%C3%85%C5%BD%C3%95%C3%91&ref=nb_sb_noss";
    
        public string GetProductUrl(Videocard card)
        {
            return card switch
            {
                Videocard.RTX3070 => "https://www.amazon.nl/s?k=3070&i=electronics&bbn=16366443031&rh=n%3A16366443031&dc&__mk_nl_NL=%C3%85M%C3%85%C5%BD%C3%95%C3%91&qid=1606476830&rnid=16332311031&ref=sr_nr_p_n_availability_2",
                Videocard.RTX3080 => "https://www.amazon.nl/s?k=3080&i=electronics&bbn=16366443031&rh=n%3A16366443031&dc&__mk_nl_NL=%C3%85M%C3%85%C5%BD%C3%95%C3%91&qid=1606476830&rnid=16332311031&ref=sr_nr_p_n_availability_2",
                Videocard.RTX3090 => "https://www.amazon.nl/s?k=3090&i=electronics&bbn=16366443031&rh=n%3A16366443031&dc&__mk_nl_NL=%C3%85M%C3%85%C5%BD%C3%95%C3%91&qid=1606476830&rnid=16332311031&ref=sr_nr_p_n_availability_2",
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
                var splittedHtml = html.Split("class=\"sg-col-4-of-24 sg-col-4-of-12 sg-col-4-of-36 s-result-item s-asin sg-col-4-of-28 sg-col-4-of-16 sg-col sg-col-4-of-20 sg-col-4-of-32\"");
                var filteredByName = splittedHtml.Where(o => o.Contains(name) && !o.Contains("DOCTYPE")).ToList();
                var filtered = filteredByName.Where(o => !o.Contains("niet op voorraad") && (o.Contains("bezorging") || o.Contains("verzendkosten") || o.Contains("Nog slechts 1 op voorraad."))).ToList();
                values.Add(card, filtered.Count());
            }
            catch (Exception)
            { }
        }
    }
}
