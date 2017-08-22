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
        public string DisplayName { get { return this.GetPropertyValue<string>("showName") ?? this.Name; } }

        //[ImplementPropertyType("galleryImages")]
        //public List<int> GalleryImages { get { return this.GetPropertyValue<string>("galleryImages").Split(',').Select(int.Parse).ToList(); } }

        public List<MemberRolesInShow> MemberRoles { get; set; }

    }
}
