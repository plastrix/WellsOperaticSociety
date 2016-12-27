using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WellsOperaticSociety.Models.AdminModels;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Web.Models
{
    public class ShowVouchersViewModel: ShowVouchersModel
    {
        public int ShowMembersCount { get; set; }
        public int MembersCount { get; set; }
        public int PatronsCount { get; set; }
        public List<Voucher> ShowMemberVoucherList { get; set; }
        public List<Voucher> MemberVoucherList { get; set; }
        public List<Voucher> PatronVoucherList { get; set; }

    }
}