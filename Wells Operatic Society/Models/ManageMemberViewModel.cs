using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Web.Models
{
    public class ManageMemberViewModel
    {
        public Member Member { get; set; }
        public List<Membership> Memberships { get; set; }
    }
}