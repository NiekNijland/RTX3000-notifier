using System;
using System.Collections.Generic;
using System.Text;

namespace RTX3000_notifier.Model
{
    public class Stock
    {
        public Website Website { get; private set; }
        public DateTime Timestamp { get; private set; }
        public Dictionary<Videocard, int> Values { get; private set; }

        public Stock(Website website, Dictionary<Videocard, int> values)
        {
            this.Website = website;
            this.Timestamp = DateTime.Now;
            this.Values = values;
        }
    }
}
