using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using RTX3000_notifier.Helper;

namespace RTX3000_notifier.Model
{
    public class Notifier
    {
        private List<IWebsite> websites;

        //stores the stock information of the previous fetch.
        private Dictionary<IWebsite, Stock> stockRecords;

        private readonly System.Timers.Timer timer;

        private bool verboseMode;

        public Notifier()
        {
            this.verboseMode = Constants.GetVerboseMode();
            this.websites = new List<IWebsite>();
            this.stockRecords = new Dictionary<IWebsite, Stock>();

            this.timer = new System.Timers.Timer(Constants.GetReloadInterval());
            this.timer.Elapsed += TimerElapsed;
        }

        public void TrackWebsite(IWebsite website)
        {
            this.websites.Add(website);
            this.stockRecords.Add(website, null);
        }

        public void Start()
        {
            this.GetStocksThreaded();
            this.timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            GetStocksThreaded();
        }

        private void GetStocksThreaded()
        {
            new Thread(GetStocks).Start();
        }

        private void GetStocks()
        {
            foreach(IWebsite website in this.websites)
            {
                new Thread(() => GetStock(website)).Start();
            }
        }

        private void GetStock(IWebsite website)
        {
            Stock stock = website.GetStock();

            if(CheckStockChange(website, stock))
            {
                Mongo.InsertStock(stock);
            }

            if (this.verboseMode)
            {
                Printer.PrintStock(stock);
            }

            this.stockRecords[website] = stock;
        }

        private bool CheckStockChange(IWebsite website, Stock stock)
        {
            bool ret = false;
            foreach (Videocard videocard in Enum.GetValues(typeof(Videocard)))
            {
                if (this.stockRecords[website] != null && this.stockRecords[website].Values[videocard] < stock.Values[videocard])
                {
                    Mailer.SendNotificationsThreaded(stock, videocard);
                    Logger.StockUpdate(stock, videocard);
                    ret = true;
                }
            }
            return ret;
        }
    }
}
