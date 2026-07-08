using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymSystem.DAL.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = null!;
        // varchar(20)

        // Navigation: a category groups many sessions
        public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
    }
}
