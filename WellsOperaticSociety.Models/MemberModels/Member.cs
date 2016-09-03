using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Zone.UmbracoMapper;
using System.ComponentModel.DataAnnotations;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class Member
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }
        public int Id { get; set; }
        [DisplayName("Telephone number")]
        public string TelephoneNumber { get; set; }
        [DisplayName("Mobile number")]
        public string MobileNumber { get; set; }
        [DisplayName("Date of birth")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Date applied for membership")]
        public DateTime? DateAppliedForMembership { get; set; }
        [DisplayName("Date approved for memberhsip")]
        public DateTime? DateApprovedForMembership { get; set; }
        [DisplayName("Date declined for membership")]
        public DateTime? DateDeclinedForMembership { get; set; }
        [DisplayName("Date life membership granted")]
        public DateTime? DateLifeMembershipGranted { get; set; }
        [DisplayName("Stripe user id")]
        public string StripeUserId { get; set; }
        [Required(ErrorMessage = "Please enter a valid email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        [DisplayName("Deactivate the user")]
        public bool Deactivated { get; set; }
        [DisplayName("vehicle reg 1")]
        public string VehicleRegistration1 { get; set; }
        [DisplayName("Vehicle reg 2")]
        public string VehicleRegistration2 { get; set; }

        public Member(IPublishedContent content)
        {
            UmbracoMapper mapper = new UmbracoMapper();
            mapper.Map(content, this);
        }

        public Member() { }

    }
}
