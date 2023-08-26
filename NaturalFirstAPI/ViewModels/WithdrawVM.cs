namespace NaturalFirstWebApp.Models
{
    public class WithdrawVM
    {
        public int IdWithdraw { get; set; }
        public Decimal? Amount { get; set; }
        //0-Pending 1-Success 2-Failed
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string? Email { get; set; }
        public string? TrnPassword { get; set; }

    }
}
