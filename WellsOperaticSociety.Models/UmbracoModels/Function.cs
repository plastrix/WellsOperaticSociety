using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.ModelsBuilder;
using Umbraco.Web;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Models.UmbracoModels
{
    public partial class Function
    {
        [ImplementPropertyType("showName")]
        public string DisplayName { get { return this.GetPropertyValue<string>("showName") ?? Name; } }

        public List<MemberRolesInShow> MemberRoles { get ; set; }

    }
}
