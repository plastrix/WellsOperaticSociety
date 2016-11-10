using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.EmailModels
{
    public class MembershipRenewal : EmailBase
    {
        public DateTime DateDue { get; set; }
        public string MembershipType { get; set; }
    }
}
