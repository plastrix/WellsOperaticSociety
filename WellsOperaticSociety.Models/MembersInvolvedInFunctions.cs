using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models
{
    public class MemberInvolvementInFunction
    {
        public int MemberInvolvementInFunctionId { get; set; }
        public string Category { get; set; }
        public string Role { get; set; }
        public int FunctionId { get; set; }
        public int MemberId { get; set; }

        //public virtual Function Function { get; set; }
    }
}
