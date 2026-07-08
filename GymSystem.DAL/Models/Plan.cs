using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Models
{
    public class Plan : BaseEntity
    {

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Description { get; set; } = null!;

        [Range(1, 365)]
        public int Duration { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }   // soft-delete flag


        // Navigation: M-M with Member via Membership junction
        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    }
}
