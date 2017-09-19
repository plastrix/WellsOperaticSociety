using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using System.ComponentModel.DataAnnotations;
using Umbraco.Core;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class Member
    {

        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your first name")]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [DisplayName("Last name")]
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }
        public int Id { get; set; }
        [DisplayName("Telephone number")]
        public string TelephoneNumber { get; set; }
        [DisplayName("Mobile number")]
        public string MobileNumber { get; set; }
        [DisplayName("Date of birth")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Date applied for membership")]
        public DateTime? DateAppliedForMembership { get; set; }
        [DisplayName("Date approved for membership")]
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
        [DisplayName("Vehicle reg 1")]
        public string VehicleRegistration1 { get; set; }
        [DisplayName("Vehicle reg 2")]
        public string VehicleRegistration2 { get; set; }
        [DisplayName("Previous years")]
        public int PreviousYears { get; set; }
        [DisplayName("Contact Email")]
        public string ContactEmail { get; set; }

        public string GetContactEmail
        {
            get
            {
                return ContactEmail.IsNullOrWhiteSpace() ? Email : ContactEmail;
            }
        }

        public Member(IPublishedContent memberNode)
        {
            var member = new UmbracoModels.Member(memberNode);
            Name = member.Name;
            Id = member.Id;
            FirstName = member.FirstName;
            LastName = member.LastName;
            TelephoneNumber = member.TelephoneNumber;
            MobileNumber = member.MobileNumber;
            DateOfBirth = member.DateOfBirth;
            DateAppliedForMembership = member.DateAppliedForMembership;
            DateApprovedForMembership = member.DateApprovedForMembership;
            DateDeclinedForMembership = member.DateDeclinedForMembership;
            DateLifeMembershipGranted = member.DateLifeMembershipGranted;
            StripeUserId = member.StripeUserId;
            Email = memberNode.GetProperty("Email").Value as string;
            Deactivated = member.Deactivated;
            VehicleRegistration1 = member.VehicleRegistration1;
            VehicleRegistration2 = member.VehicleRegistration2;
            PreviousYears = member.PreviousYears;
            ContactEmail = member.ContactEmail;
        }

        public Member() { }
    }
}
