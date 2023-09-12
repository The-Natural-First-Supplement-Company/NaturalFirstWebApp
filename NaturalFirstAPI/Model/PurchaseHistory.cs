namespace NaturalFirstAPI.Models
{
    public class PurchaseHistory
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int Quantity { get; set; }
        public Decimal? PurchaseAmt{ get; set; }
        public Decimal? IncomePerDay { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
