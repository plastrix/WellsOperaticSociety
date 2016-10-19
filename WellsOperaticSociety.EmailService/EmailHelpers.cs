using System;
using System.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace WellsOperaticSociety.EmailService
{
    public class EmailHelpers
    {
        public void SendEmail(string toEmail, string subject ,string body, string fromEmail=null, string fromName = null)
        {
            var apiKey = SensativeInformation.Keys.SendGridApiKey;
            var sg = new SendGridAPIClient(apiKey);

            var defaultEmail = ConfigurationManager.AppSettings["DefaultEmailAddress"];
            var defaultEmailName = ConfigurationManager.AppSettings["DefaultEmailAddressName"];

            Email from = new Email();
            from.Address = fromEmail.IsNotNullOrEmpty() ? fromEmail : defaultEmail;
            from.Name = fromName.IsNotNullOrEmpty() ? fromName : defaultEmailName;

            Email to = new Email(toEmail);
            Content content = new Content("text/html", body);
            Mail mail = new Mail(from, subject, to, content);

            var response = sg.client.mail.send.post(requestBody: mail.Get());
        }
    }
}
