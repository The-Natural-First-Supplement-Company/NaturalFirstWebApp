namespace NaturalFirstWebApp.ViewModels
{
    public class UserProfile
    {
        public int Id { get; set; }
        public Decimal Recharge{ get; set; }
        public Decimal Balance{ get; set; }
        public Decimal TotalEarning{ get; set; }
        public Decimal TeamIncome{ get; set; }
        public Decimal IncomeToday { get; set; }
        public byte[] Image { get; set; }
        public string Email { get; set; }
    }
}
