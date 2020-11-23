using System;
using RTX3000_notifier.Model;
using RTX3000_notifier.Helper;
using System.Collections.Generic;

namespace RTX3000_notifier
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to GeForce Tracker");
            Console.WriteLine("Press any key to exit\n\n");

            Notifier notifier = new Notifier();

            Console.ReadLine();
        }
    }
}
