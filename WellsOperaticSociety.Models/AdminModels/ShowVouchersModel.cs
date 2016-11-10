using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.AdminModels
{
    public class ShowVouchersModel
    {
        public List<Voucher> ShowMemberVoucherList { get; set; }
        public List<Voucher> MemberVoucherList { get; set; }
        public List<Voucher> PatronVoucherList { get; set; }
    }
}
