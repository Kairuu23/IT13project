using System;

namespace project.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int MemberID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; } = string.Empty;
        public string? WalkinID { get; set; }

        public bool IsArchived { get; set; }

        public Member? Member { get; set; }
    }
}
