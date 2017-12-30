using System;
using System.Configuration;
using System.Net.Mail;
using System.Reflection;
using log4net;

namespace WellsOperaticSociety.EmailService
{
	public class EmailHelpers
	{
		public void SendEmail(string toEmail, string subject, string body, string fromEmail = null, string fromName = null)
		{
			try
			{
				var defaultEmail = ConfigurationManager.AppSettings["DefaultEmailAddress"];
				var defaultEmailName = ConfigurationManager.AppSettings["DefaultEmailAddressName"];
				var fromEmailAddress = fromEmail.IsNotNullOrEmpty() ? fromEmail : defaultEmail;
				var fromEmailAddressDisplayName = fromName.IsNotNullOrEmpty() ? fromName : defaultEmailName;
				MailMessage mailMsg = new MailMessage();
				mailMsg.To.Add(toEmail);
                mailMsg.Bcc.Add("info@wellslittletheatre.com");
				mailMsg.From = new MailAddress(fromEmailAddress, fromEmailAddressDisplayName);
				mailMsg.Subject = subject;

				mailMsg.IsBodyHtml = true;
				mailMsg.Body = body;

				SmtpClient smtpClient = new SmtpClient();
				smtpClient.Send(mailMsg);
			}
			catch (Exception e)
			{

				ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
				_log.Error("There was an error whilst trying to send an email", e);
			}
		}
	}
}
