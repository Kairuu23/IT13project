using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models
{
    public class MemberPromo
    {
        public int MemberPromoID { get; set; }
        public int MemberID { get; set; }

        [ForeignKey("Promotion")]  
        public int PromoID { get; set; }

        public DateTime DateJoined { get; set; }

       
        public Member? Member { get; set; }
        public Promotion? Promotion { get; set; }
    }
}
