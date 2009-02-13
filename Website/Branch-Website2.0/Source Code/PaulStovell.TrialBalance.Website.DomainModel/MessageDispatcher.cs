using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Web;
using System.Threading;
using System.Configuration;
using System.Net.Mail;

namespace PaulStovell.TrialBalance.Website.DomainModel {
    public class MessageDispatcher {
        public static void Dispatch(Message message) {
            message.DispatchAttemptCount += 1;

            ThreadPool.QueueUserWorkItem(DispatchOnBackgroundThreadCallback, message);
        }

        private static void DispatchOnBackgroundThreadCallback(object userState) {
            Message message = userState as Message;
            try {
                if (message != null) {
                    if (message.DispatchAttemptCount < 5) {
                        MailAddressCollection recipients = new MailAddressCollection();

                        MailMessage mailMessage = new MailMessage();
                        mailMessage.Body = message.MessageBody;
                        mailMessage.Subject = message.MessageTitle;
                        mailMessage.IsBodyHtml = true;
                        mailMessage.From = new MailAddress("notifications@trialbalance.net.au", "Notifications");
                        mailMessage.To.Add(new MailAddress(ConfigurationManager.AppSettings["AdministratorContact.EmailAddress"], "Paul Stovell"));

                        SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["AdministratorContact.SmtpServer"]);
                        client.SendAsync(mailMessage, null);
                    }
                }
            } catch {
                Dispatch(message);
            }
        }
    }
}
