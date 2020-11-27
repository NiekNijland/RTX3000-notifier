using System;
using RTX3000_notifier.Model;

namespace RTX3000_notifier
{
    class Program
    {
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
            
            //notifier.TrackWebsite(new MaxICT());

            notifier.Start();
            Console.ReadLine();
        }
    }
}
