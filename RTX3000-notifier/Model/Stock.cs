using System;
using System.Collections.Generic;

namespace RTX3000_notifier.Model
{
    public class Stock
    {
        public IWebsite Website { get; private set; }
        public DateTime Timestamp { get; private set; }
        public Dictionary<Videocard, int> Values { get; private set; }

        public Stock(IWebsite website, Dictionary<Videocard, int> values)
        {
            this.Website = website;
            this.Timestamp = DateTime.Now;
            this.Values = values;
        }
    }
}
