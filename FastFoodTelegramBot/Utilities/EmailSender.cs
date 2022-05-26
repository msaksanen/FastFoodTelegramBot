using FastFoodTelegramBot.Models;
using FastFoodTelegramBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Utilities
{
    class EmailSender
    {
        internal static async Task SendEmailAsync(Account foundAccount, string subj, string mailbody)
        {
            CommandUserStatus st = CmdUserStatusRepos<CommandUserStatus>.GetStatusById(foundAccount.ChatId);
            st.isMailSent = false;
            string str = default;
            MailAddress from = new MailAddress(CommandNames.ServiceMailField, CommandNames.ServiceMailFrom);
            MailAddress to = new MailAddress(foundAccount.Email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = subj;
            message.Body = mailbody;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential(CommandNames.ServiceGmail, CommandNames.ServicePwd);
            client.EnableSsl = true;


            try
            {
                await client.SendMailAsync(message);
                str=string.Format("Email has been sent");
                Console.WriteLine(str);
                LogHelper.Debug(str, $"From:{from}. To:{to}. Subject: {subj}");
                st.isMailSent = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception caught during email sending:\n{e}");
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Exception: {e.StackTrace}");
                LogHelper.Error(e);
            }
        }
    }
}
