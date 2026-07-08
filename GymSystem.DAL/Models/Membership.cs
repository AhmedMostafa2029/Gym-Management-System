using System.ComponentModel.DataAnnotations.Schema;

namespace GymSystem.DAL.Models
{
    public class Membership : BaseEntity
    {

        public DateTime EndDate { get; set; }   // when the membership expires

        [NotMapped]
        public string Status => IsActive ? "Active" : "Expired";   // e.g. "Active", "Expired"

        [NotMapped]
        public bool IsActive => EndDate > DateTime.Now;


        // FKs + navigations
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
    }
}