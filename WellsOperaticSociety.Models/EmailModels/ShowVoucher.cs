using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Models.EmailModels
{
    public class ShowVoucher :EmailBase
    {
        public Member Member { get; set; }
        public string DateActive { get; set; }
        public string Key { get; set; }
        public Function Function { get; set; }
        public string Link { get; set; }
    }
}
