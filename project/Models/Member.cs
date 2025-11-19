using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models
{
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberID { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string MiddleInitial { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; }
        public string Status { get; set; } = "Active";

        public int? MembershipTypeID { get; set; }

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        

        private List<MembershipType> membershipTypes = new();
        private int selectedMembershipTypeId;
    }
}
