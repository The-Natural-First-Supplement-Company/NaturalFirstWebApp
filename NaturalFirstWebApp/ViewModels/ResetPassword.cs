using System.ComponentModel.DataAnnotations;

namespace NaturalFirstWebApp.ViewModels
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "Mandatory Field!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mandatory Field!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Mandatory Field!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string VerificationCode { get; set; }
    }
}
