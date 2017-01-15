using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellsOperaticSociety.Models.Enums;

namespace WellsOperaticSociety.Models.ReportModels
{
    public class MembershipReportModel
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public MembershipType? LatestMembershipType { get; set; }

    }
}
