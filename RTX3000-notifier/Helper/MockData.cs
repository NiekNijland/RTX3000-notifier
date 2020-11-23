using RTX3000_notifier.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTX3000_notifier.Helper
{
    static class MockData
    {
        public static Stock GetEmptyStock(IWebsite website)
        {
            Dictionary<Videocard, int> values = new Dictionary<Videocard, int>();

            foreach (Videocard card in Enum.GetValues(typeof(Videocard)))
            {
                values[card] = 0;
            }

            return new Stock(website, values);
        }
    }
}
