using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.EmailModels
{
    public class SubscriptionCreated:EmailBase
    {
        public string PlanName { get; set; }
    }
}
