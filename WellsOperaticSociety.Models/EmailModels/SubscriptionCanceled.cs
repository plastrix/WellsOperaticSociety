﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.EmailModels
{
    public class SubscriptionCanceled : EmailBase
    {
        public string PlanName { get; set; }
    }
}
