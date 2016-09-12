using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellsOperaticSociety.Models.Enums;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class LongServiceAward
    {
        public int LongServiceAwardId { get; set; }
        public int Member { get; set; }
        public NodaLongServiceAward Award { get; set; }
        public bool Hide { get; set; }
    }
}
