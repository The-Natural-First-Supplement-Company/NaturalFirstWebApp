namespace NaturalFirstWebApp.ViewModels
{
    public class IncomeVM
    {
        public int wbHistoryId { get; set; }
        public byte[]? ProductImage { get; set; }
        public string? ProductName { get; set; }
        public string? Remarks { get; set; }
        public Decimal Amount { get; set; }
        public int wdStatus { get; set; }
        public Decimal Total { get; set; }
        public int ProductCount { get; set; }
        public int user_id { get; set; }
    }
}
