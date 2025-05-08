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
				Body = GeneratePasswordResetEmailBody(email.Body),
				IsBodyHtml = true,
			};
			mailMessage.To.Add(email.Recipents);
			await Client.SendMailAsync(mailMessage);
			
		}

		private static string GeneratePasswordResetEmailBody(string code)
		{
			return $@"
        <html>
            <head>
                <style>
                    .code-box {{
                        background:#eee;
                        padding:10px;
                        font-size:24px;
                        text-align:center;
                        border:1px dashed #3498db;
                        font-weight:bold;
                    }}
                </style>
            </head>
            <body>
                <h2>Password Reset</h2>
                <p>Use the code below to reset your password:</p>
                <div class='code-box'>{code}</div>
                <p>This code is valid for 10 minutes.</p>
            </body>
        </html>";
		}
	
}
}
