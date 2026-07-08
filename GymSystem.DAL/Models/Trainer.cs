using GymSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymSystem.DAL.Models
{
    public class Trainer : GymUser
    {
        public Specialty Specialty { get; set; }

        public DateTime HireDate { get; set; }    // auto-set on insert


        // Navigation: 1-to-many — a trainer leads many sessions
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
