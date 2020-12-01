﻿using RTX3000_notifier.Model;
using System;
using System.Net.Mail;
using System.Threading;

namespace RTX3000_notifier.Helper
{
    /// <summary>
    /// Defines the <see cref="Mailer" />.
    /// </summary>
    static class Mailer
    {
        #region Public

        /// <summary>
        /// Send notifications threaded.
        /// </summary>
        /// <param name="stock">The stock<see cref="Stock"/>.</param>
        /// <param name="videocard">The videocard<see cref="Videocard"/>.</param>
        public static void SendNotificationsThreaded(Stock stock, Videocard videocard)
        {
            new Thread(() => SendNotifications(stock, videocard)).Start();
        }

        /// <summary>
        /// Send notifications.
        /// </summary>
        /// <param name="stock">The stock<see cref="Stock"/>.</param>
        /// <param name="videocard">The videocard<see cref="Videocard"/>.</param>
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

        /// <summary>
        /// Send notifications threaded.
        /// </summary>
        /// <param name="stock">The stock<see cref="Stock"/>.</param>
        /// <param name="videocard">The videocard<see cref="Videocard"/>.</param>
        /// <param name="subscriber">The subscriber<see cref="Subscriber"/>.</param>
        public static void SendNotificationThreaded(Stock stock, Videocard videocard, Subscriber subscriber)
        {
            new Thread(() => SendNotification(stock, videocard, subscriber)).Start();
        }

        /// <summary>
        /// Send a single notification.
        /// </summary>
        /// <param name="stock">The stock<see cref="Stock"/>.</param>
        /// <param name="videocard">The videocard<see cref="Videocard"/>.</param>
        /// <param name="subscriber">The subscriber<see cref="Subscriber"/>.</param>
        public static void SendNotification(Stock stock, Videocard videocard, Subscriber subscriber)
        {
            string subject = $"GeForceTracker: {Enum.GetName(typeof(Videocard), videocard)}";
            string body = "Beste Lezer,<br><br>" +
                        $"De voorraad van {Enum.GetName(typeof(Videocard), videocard)} is aangevuld bij <a href=\"{stock.Website.GetProductUrl(videocard)}\">{stock.Website.GetType().Name}</a><br><br>" +
                        "Wees er snel bij!<br><br>" +
                        $"<a href=\"https://geforce.nieknijland.com/voorkeuren/{subscriber.Id}\">Emailvoorkeur aanpassen</a>";
            SendMail(subscriber.Email, subject, body);
        }

        /// <summary>
        /// Send logs threaded.
        /// </summary>
        /// <param name="log">The log<see cref="string"/>.</param>
        public static void SendLogThreaded(string log)
        {
            new Thread(() => SendLog(log)).Start();
        }

        /// <summary>
        /// Send logs.
        /// </summary>
        /// <param name="log">The log<see cref="string"/>.</param>
        public static void SendLog(string log)
        {
            string subject = $"GeForceTracker: new log";
            string body = "GeForceTracker meld het volgende:<br><br>" + log;

            SendMail(Constants.GetErrorLogAddress(), subject, body);
        }

        #endregion

        #region Private

        /// <summary>
        /// Send the actual email to the smtp server.
        /// </summary>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <param name="subject">The subject<see cref="string"/>.</param>
        /// <param name="body">The body<see cref="string"/>.</param>
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

        #endregion
    }
}
