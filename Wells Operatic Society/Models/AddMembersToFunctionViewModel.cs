using System.Collections.Generic;
using WellsOperaticSociety.Models.MemberModels;
using WellsOperaticSociety.Models.UmbracoModels;

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