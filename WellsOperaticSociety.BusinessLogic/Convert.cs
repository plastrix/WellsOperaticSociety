using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellsOperaticSociety.Models.Enums;

namespace WellsOperaticSociety.BusinessLogic
{
    public static class Convert
    {
        public static MembershipType? StripePlanToMembershipType(string planName)
        {
            switch (planName)
            {
                case StaticIdentifiers.OrdinaryMemberPlanId:
                    return MembershipType.Ordinary;
                case StaticIdentifiers.PatronPlanId:
                    return MembershipType.Patron;
                case StaticIdentifiers.SocialMemberPlanId:
                    return MembershipType.Social;
            }
            return null;
        }
    }
}
