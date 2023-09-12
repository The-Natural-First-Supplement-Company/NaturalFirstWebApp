namespace NaturalFirstWebApp.ViewModels
{
    public class RechargeVM
    {
        public string? PayCode { get; set; }
        public string? PayOption { get; set; }
        public string? Email { get; set; }
        public Decimal Amount { get; set; }
        public int? IdHistory { get; set; }
        public int? Status { get; set; }
    }
    public class RechargeListVM
    {
        public int IdHistory { get; set; }
        public int UserId { get; set; }
        public byte[]? Image { get; set; }
        public string Email { get; set; }
        public Decimal Amount { get; set; }
        public string TrnCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}
