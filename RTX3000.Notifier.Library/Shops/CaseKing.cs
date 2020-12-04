using RTX3000.Notifier.Library.Helper;
using RTX3000.Notifier.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTX3000.Notifier.Library.Shop
{
    /// <summary>
    /// Defines the <see cref="CaseKing" />.
    /// </summary>
    public class CaseKing : IWebsite
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; } = "https://www.caseking.de/pc-komponenten/grafikkarten/nvidia?p=1&l=table4&ckFilters=10691&ckTab=0";

        #endregion

        #region Public

        /// <summary>
        /// The direct product url.
        /// </summary>
        /// <param name="card">The card<see cref="Videocard"/>.</param>
        /// /// The direct product url.
        public string GetProductUrl(Videocard card)
        {
            return card switch
            {
                Videocard.RTX3060TI => "https://www.caseking.de/pc-komponenten/grafikkarten/nvidia?ckFilters=10691-14202&l=table4&ckTab=0&sSort=103&sPerPage=48",
                Videocard.RTX3070 => "https://www.caseking.de/pc-komponenten/grafikkarten/nvidia?ckFilters=10691-13917&l=table4&ckTab=0&sSort=103&sPerPage=48",
                Videocard.RTX3080 => "https://www.caseking.de/pc-komponenten/grafikkarten/nvidia?ckFilters=10691-13915&l=table4&ckTab=0&sSort=103&sPerPage=48",
                Videocard.RTX3090 => "https://www.caseking.de/pc-komponenten/grafikkarten/nvidia?ckFilters=10691-13916&l=table4&ckTab=0&sSort=103&sPerPage=48",
                _ => Url,
            };
        }

        /// <summary>
        /// Get the stock.
        /// </summary>
        /// <returns>The <see cref="Stock"/>.</returns>
        public Stock GetStock()
        {
            Dictionary<Videocard, int> values = new Dictionary<Videocard, int>();
            GetStock(Videocard.RTX3060TI, "RTX 3060", values);
            GetStock(Videocard.RTX3070, "RTX 3070", values);
            GetStock(Videocard.RTX3080, "RTX 3080", values);
            GetStock(Videocard.RTX3090, "RTX 3090", values);
            return new Stock(this, values);
        }

        #endregion

        #region Private

        /// <summary>
        /// Get the stock.
        /// </summary>
        /// <param name="card">The card<see cref="Videocard"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="values">The values<see cref="Dictionary{Videocard, int}"/>.</param>
        private void GetStock(Videocard card, string name, Dictionary<Videocard, int> values)
        {
            string html = WebsiteDownloader.GetHtml(GetProductUrl(card));

            try
            {
                html = html.Replace(@"\", string.Empty);
                var splittedHtml = html.Split("<div class=\"artbox grid_");
                var filteredByName = splittedHtml.Where(o => o.Contains(name) && !o.Contains("DOCTYPE")).ToList();
                var filtered = filteredByName.Where(o => o.Contains("lagernd") && !o.Contains("unbekannt")).ToList();
                values.Add(card, filtered.Count());
            }
            catch (Exception)
            { }
        }

        #endregion
    }
}
