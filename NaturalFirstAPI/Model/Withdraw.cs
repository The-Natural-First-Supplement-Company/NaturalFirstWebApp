namespace NaturalFirstAPI.Models
{
    public class Withdraw
    {
        public int IdWithdraw { get; set; }
        public Decimal? Amount { get; set; }
        public int UserId { get; set; }
        public int BankId { get; set; }
        //0-Pending 1-Success 2-Failed
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
    }
}
