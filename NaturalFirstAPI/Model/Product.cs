namespace NaturalFirstAPI.Models
{
    public class Product
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
    }
}
