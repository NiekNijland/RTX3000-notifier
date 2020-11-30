using RTX3000_notifier.Model;
using RTX3000_notifier.Shop;
using System;

namespace RTX3000_notifier
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

            Notifier notifier = new Notifier();
            notifier.TrackWebsite(new Megekko());
            notifier.TrackWebsite(new Azerty());
            notifier.TrackWebsite(new Cdromland());
            notifier.TrackWebsite(new Informatique());
            notifier.TrackWebsite(new Coolblue());
            notifier.TrackWebsite(new Cyberport());
            notifier.TrackWebsite(new Amazon());
            notifier.TrackWebsite(new Centralpoint());

            //notifier.TrackWebsite(new MaxICT());

            notifier.Start();
            Console.ReadLine();
        }
    }
}
