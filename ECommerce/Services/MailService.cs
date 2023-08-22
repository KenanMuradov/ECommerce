using ECommerce.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Mail;

namespace ECommerce.Services
{
    public class MailService : IMailService
    {
        private readonly string _email;
        private readonly string _password;

        public MailService(string email, string password)
        {
            _email = email;
            _password = password;
        }

        public void SendConfirmationMessage(string destinationAddress, string url,string receiverName)
        {
            using var smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port= 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(_email, _password)
            };


            using var message = new MailMessage()
            {
                
                IsBodyHtml = true,
                Subject = "Ecommerce Email confirmation",
                Body = @$"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email Confirmation</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #1a1a1a; color: #ffffff;"">

    <table style=""max-width: 600px; margin: 0 auto; background-color: #1a1a1a; border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px; text-align: center; background-color: #b90000;"">
                <h1 style=""color: #ffffff;"">Email Confirmation</h1>
            </td>
        </tr>
        <tr>
            <td style=""padding: 20px;"">
                <p>Hello {receiverName},</p>
                <p style=""color: #cccccc;"">Thank you for registering on our website. Please click the button below to confirm your email address:</p>
                <p style=""text-align: center;"">
                    <a href=""{url}"" style=""display: inline-block; padding: 10px 20px; background-color: #b90000; color: #ffffff; text-decoration: none; border-radius: 5px;"">Confirm Email</a>
                </p>
                <p style=""color: #cccccc;"">If you did not request this confirmation, you can ignore this email.</p>
                <p style=""color: #cccccc;"">Best regards,<br> The Ecommerce Team</p>
            </td>
        </tr>
        <tr>
            <td style=""padding: 20px; text-align: center; background-color: #1a1a1a;"">
                <p style=""margin: 0; color: #cccccc;"">© {DateTime.Now.Year} Ecommerce. All rights reserved.</p>
            </td>
        </tr>
    </table>

</body>
</html>"
            };

            message.From = new MailAddress(_email);
            message.To.Add(new MailAddress(destinationAddress));

            smtp.Send(message);
        }

        public void SendResetPasswordMessage(string destinationAddress, string url)
        {
            using var smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(_email, _password)
            };

            using var message = new MailMessage()
            {

                IsBodyHtml = true,
                Subject = "Ecommerce password reset",
                Body = @$"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email Confirmation</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #1a1a1a; color: #ffffff;"">

    <table style=""max-width: 600px; margin: 0 auto; background-color: #1a1a1a; border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px; text-align: center; background-color: #b90000;"">
                <h1 style=""color: #ffffff;"">Reset Password</h1>
            </td>
        </tr>
        <tr>
            <td style=""padding: 20px;"">
                <p style=""color: #cccccc;"">This message has been sent you yo reset your password in our website:</p>
                <p style=""text-align: center;"">
                    <a href=""{url}"" style=""display: inline-block; padding: 10px 20px; background-color: #b90000; color: #ffffff; text-decoration: none; border-radius: 5px;"">Reset Your Password</a>
                </p>
                <p style=""color: #cccccc;"">If you did not try to change the password please ignore this message</p>
                <p style=""color: #cccccc;"">Best regards,<br> The Ecommerce Team</p>
            </td>
        </tr>
        <tr>
            <td style=""padding: 20px; text-align: center; background-color: #1a1a1a;"">
                <p style=""margin: 0; color: #cccccc;"">© {DateTime.Now.Year} Ecommerce. All rights reserved.</p>
            </td>
        </tr>
    </table>

</body>
</html>"
            };

            message.From = new MailAddress(_email);
            message.To.Add(new MailAddress(destinationAddress));

            smtp.Send(message);
        }
    }
}
