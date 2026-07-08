using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }   // in cm
        public decimal Weight { get; set; }   // in kg


        [Required, MaxLength(5)]
        public string BloodType { get; set; } = null!;   // e.g. "A+", "O-"

        [MaxLength(500)]
        public string? Note { get; set; }


        // Foreign key + navigation back to Member
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
    }
}
