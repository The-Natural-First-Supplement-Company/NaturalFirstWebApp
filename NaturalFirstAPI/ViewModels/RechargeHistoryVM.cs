using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace NaturalFirstAPI.ViewModels
{
    public class RechargeHistoryVM
    {
        public int IdHistory { get; set; }
        public Decimal? Amount { get; set; }
        public int Status { get; set; }
        public string? PaymentType { get; set; }
        public string? TrnCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Email { get; set; }
    }
}
