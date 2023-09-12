namespace NaturalFirstWebApp.ViewModels
{
    public class MyTeamVM
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class MyTeamPostVM
    {
        public string Email { get; set; }
        public int Percent { get; set; }
    }

}
