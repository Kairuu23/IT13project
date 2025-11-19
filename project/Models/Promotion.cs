using System;
using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Promotion
    {
        [Key]
        public int PromoID { get; set; } 
        public string PromoName { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal DiscountRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
