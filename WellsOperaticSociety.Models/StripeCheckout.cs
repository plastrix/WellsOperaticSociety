using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models
{
    public class StripeCheckout
    {
        public string PlanId { get; set; }
        public string StripeToken { get; set; }
    }
}
