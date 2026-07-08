using GymSystem.BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.MembersViewModels
{
    public class MemberIndexViewModel
    {
        public MemberFilterViewModel Filter { get; set; } = new();

        public PaginatedList<MemberViewModel> Members { get; set; } = null!;

        public IEnumerable<string> Cities { get; set; } = Enumerable.Empty<string>();

    }
}
