using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Zone.UmbracoMapper;
using System.ComponentModel.DataAnnotations;
using WellsOperaticSociety.Models.Enums;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class Membership
    {
        public int MembershipId { get; set; }
        [Required]
        public int Member { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public MembershipType MembershipType { get; set; }
        public bool IsSubscription { get; set; }
        public bool CancelAtEnd { get; set; }
        public string StripeSubscriptionId { get; set; }

        public string MembershipTypeName => Enum.GetName(typeof(MembershipType), MembershipType);
    }
}
