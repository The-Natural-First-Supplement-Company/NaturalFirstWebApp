namespace NaturalFirstWebApp.Models
{
    public class BalanceWallet
    {
        public int IdWallet { get; set; }
        public Decimal? Amount { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }

    }
}
