using System.ComponentModel.DataAnnotations;

namespace NaturalFirstWebApp.ViewModels
{
    public class BankVM
    {
        [Required(ErrorMessage ="Mandatory")]
        public string? RealName { get; set; }
        [Required(ErrorMessage = "Mandatory")]
        public string? BankName { get; set; }
        [Required(ErrorMessage = "Mandatory")]
        public string? AccountNo { get; set; }
        [Required(ErrorMessage = "Mandatory")]
        public string? IFSCCode { get; set; }
        [Required(ErrorMessage = "Mandatory")]
        public string? TrnPassword { get; set; }
        public string? email { get; set; }
    }
}
