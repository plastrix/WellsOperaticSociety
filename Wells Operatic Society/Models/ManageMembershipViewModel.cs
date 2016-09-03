using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco;
using Umbraco.Core.Models;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Web.Models
{
    public class ManageMembershipViewModel
    {
        public IEnumerable<IMember> Members { get; set; }
        public Membership Membership { get; set; }
    }
}