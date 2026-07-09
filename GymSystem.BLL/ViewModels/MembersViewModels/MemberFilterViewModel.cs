using GymSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.MembersViewModels
{
    public class MemberFilterViewModel
    {
        public string? Search { get; set; }

        public string? City { get; set; }

        public Gender? Gender { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;
    }
}
