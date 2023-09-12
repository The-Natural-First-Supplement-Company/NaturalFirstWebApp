namespace NaturalFirstAPI.ViewModels
{
    public class MyTeamVM
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class MyTeamPostVM
    {
        public int Percent { get; set; }
        public int UserId { get; set; }
    }
    public class UserProfile
    {
        public int Id { get; set; }
        public Decimal Recharge { get; set; }
        public Decimal Balance { get; set; }
        public Decimal TotalEarning { get; set; }
        public Decimal TeamIncome { get; set; }
        public Decimal IncomeToday { get; set; }
        public byte[] Image { get; set; }
        public string Email { get; set; }
    }
}
