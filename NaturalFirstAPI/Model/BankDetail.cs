using System;

namespace NaturalFirstAPI.Model
{
    public class BankDetail
    {
        public int IdBankDetails { get; set; }
        public string? BankName { get; set; }
        public string? RealName { get; set; }
        public string? AccountNo { get; set; }
        public string? IFSCCode { get; set; }
        public int UserId { get; set; }
        public string? TrnPassword { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string? email { get; set; }
    }
}
