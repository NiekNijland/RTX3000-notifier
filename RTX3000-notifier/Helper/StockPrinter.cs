using System;
using System.Collections.Generic;
using System.Text;
using RTX3000_notifier.Model;

namespace RTX3000_notifier.Helper
{
    static class StockPrinter
    {
        public static void PrintStock(Stock stock)
        {
            string toPrint = stock.Website.GetType().Name + " -- " + stock.Timestamp.ToString();

            foreach (KeyValuePair<Videocard, int> entry in stock.Values)
            {
                toPrint += $"\n{Enum.GetName(typeof(Videocard), entry.Key)} : {entry.Value}";
            }

            Console.WriteLine(toPrint + "\n------------------------");
        }
    }
}
