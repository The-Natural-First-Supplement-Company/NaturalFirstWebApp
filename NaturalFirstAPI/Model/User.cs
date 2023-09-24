namespace NaturalFirstAPI.Model
{
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? NickName { get; set; }
        public string? Password { get; set; }
        public string? ReferralCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public int isActive{ get; set; }
        public string? ReferredBy { get; set; }
        public string? Role { get; set; }
        public byte[]? ProfilePic { get; set; }
    }
}
