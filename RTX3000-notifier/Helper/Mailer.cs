using RTX3000_notifier.Model;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTX3000_notifier.Helper
{
    static class Mailer
    {
        public static async void SendSubscribersNotificationAsync(Stock stock, Videocard videocard)
        {
            await SendSubscribersNotificationTask(stock, videocard);
        }

        private static Task SendSubscribersNotificationTask(Stock stock, Videocard videocard)
        {
            return Task.Run(() => SendSubscribersNotification(stock, videocard));
        }

        public static void SendSubscribersNotification(Stock stock, Videocard videocard)
        {
            List<Subscriber> subscribers = Mongo.GetSubscribers();

            foreach(Subscriber subscriber in subscribers)
            {
                SendSubscriberNotificationAsync(stock, videocard, subscriber);
            }
        }
        
        public static async void SendSubscriberNotificationAsync(Stock stock, Videocard videocard, Subscriber subscriber)
        {
            await SendSubscriberNotificationTask(stock, videocard, subscriber);
        }

        private static Task SendSubscriberNotificationTask(Stock stock, Videocard videocard, Subscriber subscriber)
        {
            return Task.Run(() => SendSubscriberNotification(stock, videocard, subscriber));
        }

        public static void SendSubscriberNotification(Stock stock, Videocard videocard, Subscriber subscriber)
        {
            string subject = $"GeForceTracker: {Enum.GetName(typeof(Videocard), videocard)}";
            string body = "Beste Lezer,<br><br>" +
                        $"De voorraad van {Enum.GetName(typeof(Videocard), videocard)} is aangevuld bij <a href=\"{stock.Website.Url}\">{stock.Website.GetType().Name}</a><br><br>" +
                        "Wees er snel bij!<br><br>" +
                        $"<a href=\"https://geforce.nieknijland.com/voorkeuren/{subscriber.Id}\">Emailvoorkeur aanpassen</a>";

            SendMail(subscriber.Email, subject, body);
        }

        private static void SendMail(string email, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient(Constants.GetEmailHost());


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
            }
            catch (Exception)
            {
                Logger.EmailError(email);
            }
        }

        public static async void SendErrorLogAsync(string log)
        {
            await SendErrorLogTask(log);
        }

        private static Task SendErrorLogTask(string log)
        {
            return Task.Run(() => SendErrorLog(log));
        }

        public static void SendErrorLog(string log)
        {
            string subject = $"GeForceTracker: Error Encountered";
            string body = "GeForceTracker is de volgende error tegengekomen:<br><br>" + log;

            SendMail(Constants.GetErrorLogAddress(), subject, body);
        }
    }
}
