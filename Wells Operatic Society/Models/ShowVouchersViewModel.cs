using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WellsOperaticSociety.Models.AdminModels;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Web.Models
{
    public class ShowVouchersViewModel
    {
        public int ShowMembersCount { get; set; }
        public int MembersCount { get; set; }
        public int PatronsCount { get; set; }
        public ShowVouchersModel ShowVouchers { get; set; }
        public string ShowMemberVouchers { get; set; }
        public string MemberVouchers { get; set; }
        public string PatronVouchers { get; set; }

    }
}