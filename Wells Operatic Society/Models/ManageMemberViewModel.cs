using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Web.Models
{
    public class ManageMemberViewModel
    {
        public Member Member { get; set; }
        public Membership NewMembership { get; set; }
        public IEnumerable<Membership> Memberships { get; set; }
        public bool IsSubscribedToMailingList { get; set; }
        public bool IsSubscribedToMemberList { get; set; }
        public bool HasMemberRole { get; set; }
        public bool HasEditorRole { get; set; }
        public bool HasCommitteeRole { get; set; }
    }
}