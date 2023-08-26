namespace NaturalFirstWebApp.Models
{
    public class RechargeHistory
    {
        public int IdHistory { get; set; }
        public Decimal? Amount { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string? PaymentType { get; set; }
        public string? TrnCode { get; set; }
    }
}
