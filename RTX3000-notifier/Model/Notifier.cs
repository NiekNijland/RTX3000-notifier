using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using RTX3000_notifier.Helper;

namespace RTX3000_notifier.Model
{
    public class Notifier
    {
        public List<Website> Websites { get; private set; }

        private readonly Timer timer;

        public Notifier()
        {
            this.Websites = new List<Website>() { new Megekko(), new Azerty() };

            this.TimerEvent(null, null);
            this.timer = new Timer(Constants.GetReloadInterval());
            this.timer.Elapsed += this.TimerEvent;
            this.timer.Start();
        }

        private async void TimerEvent(object sender, ElapsedEventArgs e)
        {
            await GetStockAsync();
        }

        private Task GetStockAsync()
        {
            return Task.Run(() => GetStock()); 
        }

        private void GetStock()
        {
            foreach(Website w in this.Websites)
            {
                Stock stock = w.GetStock();

                StockPrinter.PrintStock(stock);
                Mongo.InsertStock(stock);
            }
        }
    }
}
