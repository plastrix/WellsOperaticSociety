using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco;
using umbraco.presentation.umbraco.members;
using Umbraco.Core.Models;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Web.Models
{
    public class ManageMembershipViewModel
    {
        public MemberSearch MemberSearch { get; set; }
        public IEnumerable<IMember> Members { get; set; }
    }
}