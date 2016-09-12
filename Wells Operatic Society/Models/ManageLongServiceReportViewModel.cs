using System.Collections.Generic;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Web.Models
{
    public class ManageLongServiceReportViewModel
    {
        public List<LongServiceAward> DueAwards { get; set; }
        public List<LongServiceAward> AlreadyPresentedAwards { get; set; }
    }
}