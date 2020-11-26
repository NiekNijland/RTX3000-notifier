using RTX3000_notifier.Model;
using System;
using System.Net.Mail;
using System.Threading;

namespace RTX3000_notifier.Helper
{
    static class Mailer
    {
        public static void SendNotificationsThreaded(Stock stock, Videocard videocard)
        {
            new Thread(() => SendNotifications(stock, videocard)).Start();
        }

        public static void SendNotifications(Stock stock, Videocard videocard)
        {
            var subscribers = Mongo.GetSubscribers();
            foreach (Subscriber subscriber in subscribers)
            {
                
                if (subscriber.Interests.Contains(videocard))
                {
                    SendNotificationThreaded(stock, videocard, subscriber);
                }
            }
        }

        public static void SendNotificationThreaded(Stock stock, Videocard videocard, Subscriber subscriber)
        {
            new Thread(() => SendNotification(stock, videocard, subscriber)).Start();
        }

        public static void SendNotification(Stock stock, Videocard videocard, Subscriber subscriber)
        {
            string subject = $"GeForceTracker: {Enum.GetName(typeof(Videocard), videocard)}";
            string body = "Beste Lezer,<br><br>" +
                        $"De voorraad van {Enum.GetName(typeof(Videocard), videocard)} is aangevuld bij <a href=\"{stock.Website.GetProductUrl(videocard)}\">{stock.Website.GetType().Name}</a><br><br>" +
                        "Wees er snel bij!<br><br>" +
                        $"<a href=\"https://geforce.nieknijland.com/voorkeuren/{subscriber.Id}\">Emailvoorkeur aanpassen</a>";
            SendMail(subscriber.Email, subject, body);
        }

        public static void SendLogThreaded(string log)
        {
            new Thread(() => SendLog(log)).Start();
        }

        public static void SendLog(string log)
        {
            string subject = $"GeForceTracker: new log";
            string body = "GeForceTracker meld het volgende:<br><br>" + log;

            SendMail(Constants.GetErrorLogAddress(), subject, body);
        }

        private static void SendMail(string email, string subject, string body)
        {
            try
            {
                var mail = new MailMessage();
                var smtp = new SmtpClient(Constants.GetEmailHost());

                mail.From = new MailAddress(Constants.GetEmailUsername());
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                smtp.Port = Constants.GetEmailPort();
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(Constants.GetEmailUsername(), Constants.GetEmailPassword());
                smtp.EnableSsl = true;

                smtp.Send(mail);
                Logger.EmailSend(email);
            }
            catch (Exception)
            {
                Logger.EmailError(email);
            }
        }
    }
}
