using Rawy.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Rawy.APIs.Helper
{
	public class EmailService
	{
		public static async Task SendEmailAsync(Email email)
		{
			// mail service : google and SMTP
			var Client = new SmtpClient("smtp.gmail.com", 587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential("rawy.app2025@gmail.com", "wdiphylhyhstyzzj");
			
			var mailMessage = new MailMessage
			{
				From = new MailAddress("rawy.app2025@gmail.com"),
				Subject = email.Subject,
				Body = email.Body,
				IsBodyHtml = true,
			};
			mailMessage.To.Add(email.Recipents);
			await Client.SendMailAsync(mailMessage);
			
		}
	}
}
