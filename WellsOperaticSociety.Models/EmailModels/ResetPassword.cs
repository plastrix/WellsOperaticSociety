using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Models.EmailModels
{
    public class ResetPassword
    {
        public Member Member { get; set; }
        public string Link { get; set; }
    }
}
