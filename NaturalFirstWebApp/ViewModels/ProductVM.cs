namespace NaturalFirstWebApp.ViewModels
{
    public class ProductVM
    {
        public int IdProducts { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public Decimal? InvestAmt { get; set; }
        public int Cycle { get; set; }
        public Decimal? IncomePerDay { get; set; }
        public Decimal? TotalAmt { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy{ get; set; }
        public byte[] ProductImage{ get; set; }
        public string email { get; set; }
    }

    //ViewModal for Wallet Balance in ProductDetails Page
    public class PDWallet
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        //Amount available in recharge wallet
        public Decimal Recharge { get; set; }
        //Amount available for withdraw
        public Decimal Balance { get; set; }
    }
}
