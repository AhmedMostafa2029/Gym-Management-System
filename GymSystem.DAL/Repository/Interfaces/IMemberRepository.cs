using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        // New Faetures
        Task<Member?> GetMemberWithHealthRecordById(int id);  // include
    }
}
