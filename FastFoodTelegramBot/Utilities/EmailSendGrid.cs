using FastFoodTelegramBot.Models;
using FastFoodTelegramBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SendGrid; // SendGrid 
using SendGrid.Helpers.Mail; // used for MailHelper
using System.Text.RegularExpressions;

namespace FastFoodTelegramBot.Utilities
{
    class EmailSendGrid
    {
        internal static async Task SendEmailAsync(Account foundAccount, string subj, string mailbody)
        {
            string str = default;
            CommandUserStatus st = CmdUserStatusRepos<CommandUserStatus>.GetStatusById(foundAccount.ChatId);
            st.isMailSent = false;
           
          

            // Create an instance of the SendGrid Mail Client using the valid API Key
            var client = new SendGridClient(CommandNames.SendGridApiKey);

            // Use the From Email as the Email you verified above
            var senderEmail = new EmailAddress(CommandNames.ServiceMailField, CommandNames.ServiceMailFrom);

            // The recipient of the email
            var recieverEmail = new EmailAddress(foundAccount.Email, $"{ foundAccount.FirstName } { foundAccount.LastName}");

            // Define the Email Subject
            string emailSubject = subj;

            // the plain text content for Plain Text Email Clients
            string textContent = mailbody;

            // HTML content -> for clients supporting HTML, this is default
            //string htmlContent = $"<strong>FasFoodBot Service.</strong> <p>{subj}</p> <p>{mailbody}</p> ";
            string htmlContent = "<strong>FasFoodBot Service via SendGrid </strong> <br>" + StringToHtml(mailbody);

            // construct the email to send via the SendGrid email api
            var msg = MailHelper.CreateSingleEmail(senderEmail, recieverEmail, emailSubject, textContent, htmlContent);

            // send the email asyncronously
           // var resp = await client.SendEmailAsync(msg).ConfigureAwait(false);


            try
            {
                var resp = await client.SendEmailAsync(msg).ConfigureAwait(false);
                str = string.Format("Email has been sent by SendGrid.");
                Console.WriteLine(str);
                LogHelper.Debug(str, $"From:{CommandNames.ServiceMailField}. To:{foundAccount.Email}. Subject: {subj}");
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
        private static string StringToHtml(string text)
        {
            text = Regex.Replace(text, "^(.*?)$", "<p>$1</p>\r\n", RegexOptions.Multiline);
            text = Regex.Replace(text, @"http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?", "<a href=\"$0\">$0</a>");
            return text;
        }
    }
}
