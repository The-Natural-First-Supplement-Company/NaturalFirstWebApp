using System.Net.Mail;
using System.Net;
//Exclude when posting to production
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace NaturalFirstWebApp.Models
{
    public class EmailSender
    {
        public void SendEmailAsync(string email, string subject, string message)
        {
            IConfiguration _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            string _smtpHost = _configuration["AppSettings:SmtpHost"];
            int _smtpPort = int.Parse(_configuration["AppSettings:SmtpPort"]);
            string _smtpUsername = _configuration["AppSettings:SmtpUsername"];
            string _smtpPassword = _configuration["AppSettings:SmtpPassword"];
            bool _smtpSSL = bool.Parse(_configuration["AppSettings:SmtpSSL"]);
            try
            {
                using (var client = new SmtpClient(_smtpHost, _smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    client.EnableSsl = true;
                    //if(_smtpPort == 25)
                    //{
                    //    client.EnableSsl = false; 
                    //}
                    //else
                    //{
                    //    client.EnableSsl = _smtpSSL;
                    //}

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpUsername),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(email);

                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        //public void SendEmailAsync(string email, string subject, string message)
        //{
        //    UserCredential credential;
        //    string relativePath = "wwwroot/extras/client_secret_934996869659.json";
        //    string jsonFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), relativePath);

        //    using (var stream = new FileStream("wwwroot/extras/client_secret_934996869659.json", FileMode.Open, FileAccess.Read))
        //    {
        //        string credPath = "token.json";
        //        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //            GoogleClientSecrets.FromStream(stream).Secrets,
        //            new[] { GmailService.Scope.GmailSend },
        //            "user",
        //            CancellationToken.None,
        //            new FileDataStore(credPath, true)).Result;
        //    }

        //    var service = new GmailService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = "My Email App",
        //    });

        //    var _email = CreateMessage("kumesh1615@gmail.com", email, subject, message);
        //    SendMessage(service, "me", _email);
        //}

        //private static MimeMessage CreateMessage(string from, string to, string subject, string body)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("", from));
        //    message.To.Add(new MailboxAddress("", to));
        //    message.Subject = subject;

        //    var builder = new BodyBuilder();
        //    builder.TextBody = body;

        //    message.Body = builder.ToMessageBody();

        //    return message;
        //}

        //private static void SendMessage(GmailService service, string userId, MimeMessage email)
        //{
        //    var rawMessage = Base64Url.Encode(email.ToString());

        //    var message = new Message
        //    {
        //        Raw = rawMessage
        //    };

        //    try
        //    {
        //        service.Users.Messages.Send(message, userId).Execute();
        //        Console.WriteLine("Message sent successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //    }
        //}

        //// Helper method for encoding to Base64Url
        //public static class Base64Url
        //{
        //    public static string Encode(string input)
        //    {
        //        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        //        return Convert.ToBase64String(bytes)
        //            .Replace('+', '-')
        //            .Replace('/', '_')
        //            .Replace("=", "");
        //    }
        //}

    }
}
