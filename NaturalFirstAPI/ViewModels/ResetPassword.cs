using System.ComponentModel.DataAnnotations;

namespace NaturalFirstAPI.ViewModels
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
