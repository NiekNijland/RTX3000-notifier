using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using RTX3000_notifier.Helper;

namespace RTX3000_notifier.Model
{
    public class Notifier
    {
        public List<IWebsite> Websites { get; private set; }

        //stores the stock information of the previous fetch.
        public Dictionary<IWebsite, Stock> StockRecords { get; set; }

        private readonly System.Timers.Timer timer;

        public Notifier()
        {
            this.Websites = new List<IWebsite>() { new Megekko(), new Azerty(), new Informatique(), new Cdromland() };
            this.StockRecords = new Dictionary<IWebsite, Stock>();

            this.StockRecords[this.Websites[0]] = null;
            this.StockRecords[this.Websites[1]] = null;
            this.StockRecords[this.Websites[2]] = null;
            this.StockRecords[this.Websites[3]] = null;

            /*
            this.StockRecords[this.Websites[0]] = MockData.GetEmptyStock(this.Websites[0]);
            this.StockRecords[this.Websites[1]] = MockData.GetEmptyStock(this.Websites[1]);
            this.StockRecords[this.Websites[2]] = MockData.GetEmptyStock(this.Websites[2]);
            this.StockRecords[this.Websites[3]] = MockData.GetEmptyStock(this.Websites[3]);
            */

            this.TimerElapsed(null, null);
            this.timer = new System.Timers.Timer(Constants.GetReloadInterval());
            this.timer.Elapsed += TimerElapsed;
            this.timer.Start();
        }

        private void GetStock(IWebsite website)
        {
            IWebsite website2 = website;
            Stock stock = website2.GetStock();

            //Printer.PrintStock(stock);
            Mongo.InsertStock(stock);

            CheckIfStockIncreased(website2, stock);
            this.StockRecords[website2] = stock;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            GetStocksThreaded();
        }

        private void GetStocks()
        {
            foreach(IWebsite website in this.Websites)
            {
                new Thread(() => GetStock(website)).Start();
            }
        }

        private void GetStocksThreaded()
        {
            new Thread(GetStocks).Start();
        }

        private void CheckIfStockIncreased(IWebsite website, Stock stock)
        {
            if (this.StockRecords.ContainsKey(website) && this.StockRecords[website] != null)
            {
                foreach(KeyValuePair<Videocard, int> pair in stock.Values)
                {
                    if(this.StockRecords[website].Values[pair.Key] < pair.Value)
                    {
                        Mailer.SendNotificationsThreaded(stock, pair.Key);
                        Logger.StockUpdate(stock, pair.Key);
                    }
                }
            }
        }
    }
}
