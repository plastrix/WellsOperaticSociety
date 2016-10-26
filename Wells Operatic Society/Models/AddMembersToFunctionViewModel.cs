using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WellsOperaticSociety.Models;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Web.Models
{
    public class AddMembersToFunctionViewModel
    {
        public Function Function { get; set; }
        public List<MemberRolesInShow> MemberRolesInShows { get; set; }
        public MemberRolesInShow NewMemberRolesInShow { get; set; }
        public List<string> MostUsedGroups { get; set; }
    }
}