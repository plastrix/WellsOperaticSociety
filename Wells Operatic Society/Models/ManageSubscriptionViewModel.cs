using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Web.Models
{
    public class ManageSubscriptionViewModel
    {
        public string PlanId { get; set; }
        public string SocialPrice { get; set; }
        public string OrdinaryPrice { get; set; }
        public string PatronPrice { get; set; }
        public bool IsSocialMember { get; set; }
        public bool IsOrdinaryMember { get; set; }
        public bool IsPatron { get; set; }
        public bool IsStripeUser { get; set; }
        public bool HasExistingSubscription { get; set; }
    }
}