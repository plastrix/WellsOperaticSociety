using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class MemberRolesInShow
    {
        public int MemberRolesInShowId { get; set; }
        public int MemberId { get; set; }
        public string Group { get; set; }
        public string Role { get; set; }
    }
}
