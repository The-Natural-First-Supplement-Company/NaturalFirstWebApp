using System.Drawing;

namespace NaturalFirstWebApp.Models
{
    public static class Send_Email
    {
        public static string SendEmailVerification(string Email)
        {
            EmailSender emailSender = new EmailSender();
            Random random = new Random();
            int verificationCode = random.Next(100000, 999999);
            emailSender.SendEmailAsync(Email, "Account Verification", "Your verification code is : "+ verificationCode +" ");
            return verificationCode.ToString();
        }
    }
}
