using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Models
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }

        // 1-25 (DB CHECK constraint)
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        // FKs + navigations
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Members attend many sessions via Booking
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
