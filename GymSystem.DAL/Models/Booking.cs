using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Models
{
    public class Booking :BaseEntity
    {
        public bool IsAttended { get; set; } = false;

        //public DateTime CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }

        // FKs to both sides
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
    }
}
