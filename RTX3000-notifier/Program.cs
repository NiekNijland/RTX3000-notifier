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

            _ = new Notifier();

            Console.ReadLine();
        }
    }
}
