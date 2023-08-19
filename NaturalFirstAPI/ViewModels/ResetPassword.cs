using System.ComponentModel.DataAnnotations;

namespace NaturalFirstWebApp.ViewModels
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
