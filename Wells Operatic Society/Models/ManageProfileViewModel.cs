using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Member = WellsOperaticSociety.Models.MemberModels.Member;

namespace WellsOperaticSociety.Web.Models
{
    public class ManageProfileViewModel
    {
        public Member Member { get; set; }
        public bool IsSubscribedToMemberList { get; set; }
        public bool IsSubscribedToMailingList { get; set; }
    }
}