using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace WellsOperaticSociety.EmailService
{
    public class EmailHelpers
    {
        public void SendEmail(string fromEmail, string toEmail, string subject ,string body)
        {
            var apiKey = SensativeInformation.Keys.SendGridApiKey;
            var sg = new SendGridAPIClient(apiKey);

            Email from = new Email(fromEmail);
            Email to = new Email(toEmail);
            Content content = new Content("text/html", body);
            Mail mail = new Mail(from, subject, to, content);

            var response = sg.client.mail.send.post(requestBody: mail.Get());
        }
    }
}
