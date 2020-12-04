using RTX3000.Notifier.Library.Shop;
using RTX3000.Notifier.Library.Model;
using System;
using RTX3000.Notifier.Library.Helper;

namespace RTX3000.Notifier
{
    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to GeForce Tracker");
            Console.WriteLine("Press any key to exit\n\n");

            var notifier = new RTX3000.Notifier.Library.Model.Notifier();
            notifier.TrackWebsite(new Megekko());
            notifier.TrackWebsite(new Azerty());
            notifier.TrackWebsite(new Cdromland());
            notifier.TrackWebsite(new Informatique());
            notifier.TrackWebsite(new Coolblue());
            notifier.TrackWebsite(new Cyberport());
            notifier.TrackWebsite(new Amazon());
            notifier.TrackWebsite(new Centralpoint());
            notifier.TrackWebsite(new PCKing());
            notifier.TrackWebsite(new CaseKing());
            //notifier.TrackWebsite(new MaxICT());
            
            notifier.Start();
            if(Constants.GetUseToasts())
                RTX3000.Notifier.Library.Helper.Mailer.SendToast("RTX 3000 notifier started", "");
            Console.ReadLine();
        }
    }
}
