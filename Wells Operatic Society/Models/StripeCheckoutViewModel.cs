using System.Collections.Generic;
using Stripe;
using WellsOperaticSociety.Models;

namespace WellsOperaticSociety.Web.Models
{
    public class StripeCheckoutViewModel
    {
        public StripeCheckout Checkout { get; set; }
        public IEnumerable<StripePlan> Plans { get; set; }
        public IEnumerable<StripeSubscription> Subscriptions { get; set; }
    }
}