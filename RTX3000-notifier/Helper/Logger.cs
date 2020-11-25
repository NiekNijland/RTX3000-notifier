using RTX3000_notifier.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTX3000_notifier.Helper
{
    public static class Logger
    {
        public static void JsonReadError(string field)
        {
            string log = $"Error reading {field} name from json";
            Console.WriteLine(log);
            Mailer.SendLogThreaded(log);
        }

        public static void EmailError(string email)
        {
            string log = $"Error sending email to: {email}";
            Console.WriteLine(log);
        }

        public static void EmailSend(string email)
        {
            string log = $"Email send to: {email}";
            Console.WriteLine(log);
        }

        public static void HtmlDownloadError(string url)
        {
            string log = $"Error downloading html with GET: {url}";
            Console.WriteLine(log);
            Mailer.SendLogThreaded(log);
        }

        public static void StockUpdate(Stock stock, Videocard videocard)
        {
            string log = $"De voorraad van {Enum.GetName(typeof(Videocard), videocard)} is aangevuld bij {stock.Website.GetType().Name}";
            Console.WriteLine(log);
        }

        public static void HtmlStockCheckError(IWebsite website)
        {
            string log = $"Error checking html for stock at: {website.GetType().Name}";
            Console.WriteLine(log);
            Mailer.SendLogThreaded(log);
        }
    }
}
