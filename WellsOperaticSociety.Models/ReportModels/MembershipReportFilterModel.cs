using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellsOperaticSociety.Models.Enums;

namespace WellsOperaticSociety.Models.ReportModels
{
    public class MembershipReportFilterModel
    {
        public IList<string> MembershipStatus { get; set; }
        public IList<MembershipType> MembershipType { get; set; }
    }
}
