using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.ModelsBuilder;
using Umbraco.Web;

namespace WellsOperaticSociety.Models.UmbracoModels
{
    public partial class Member
    {
        ///<summary>
        /// Date Applied For Membership: The date the member applied for membership
        ///</summary>
        [ImplementPropertyType("dateAppliedForMembership")]
        public DateTime? DateAppliedForMembership
        {
            get { return this.GetPropertyValue<DateTime?>("dateAppliedForMembership") == DateTime.MinValue ? null : this.GetPropertyValue<DateTime?>("dateAppliedForMembership"); }
        }

        ///<summary>
        /// Date Approved For Membership: The date the member was approved by the committee for membership
        ///</summary>
        [ImplementPropertyType("dateApprovedForMembership")]
        public DateTime? DateApprovedForMembership
        {
            get { return this.GetPropertyValue<DateTime?>("dateApprovedForMembership") == DateTime.MinValue ? null : this.GetPropertyValue<DateTime?>("dateApprovedForMembership"); }
        }

        ///<summary>
        /// Date Declined For Membership: The date the committee declined the user for membership
        ///</summary>
        [ImplementPropertyType("dateDeclinedForMembership")]
        public DateTime? DateDeclinedForMembership
        {
            get { return this.GetPropertyValue<DateTime?>("dateDeclinedForMembership") == DateTime.MinValue ? null : this.GetPropertyValue<DateTime?>("dateDeclinedForMembership"); }
        }

        ///<summary>
        /// Date Life Membership Granted: The date the committee awarded this user with life membership
        ///</summary>
        [ImplementPropertyType("dateLifeMembershipGranted")]
        public DateTime? DateLifeMembershipGranted
        {
            get { return this.GetPropertyValue<DateTime?>("dateLifeMembershipGranted") == DateTime.MinValue ? null : this.GetPropertyValue<DateTime?>("dateLifeMembershipGranted"); }
        }

        ///<summary>
        /// Date Of Birth: The date the user was member was born. Used for tracking possible subscriptions
        ///</summary>
        [ImplementPropertyType("dateOfBirth")]
        public DateTime? DateOfBirth
        {
            get { return this.GetPropertyValue<DateTime?>("dateOfBirth") == DateTime.MinValue ? null : this.GetPropertyValue<DateTime?>("dateOfBirth"); }
        }
    }
}
