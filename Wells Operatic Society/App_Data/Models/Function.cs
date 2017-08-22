using System.Collections.Generic;
using System.Linq;
using Umbraco.ModelsBuilder;
using Umbraco.Web;
using WellsOperaticSociety.Models.MemberModels;
using Umbraco.Core.Models.PublishedContent;

namespace Umbraco.Web.PublishedContentModels
{
    public partial class Function
    {
        //public string Description { get; set; }
        //public string ShortDescription { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        //public int Image { get; set; }
        //public string ButtonText { get; set; }
        //public string ButtonLink { get; set; }
        //public bool OpenLinkInNewWindow { get; set; }
        //public string ShowName { get; set; }

        [ImplementPropertyType("showName")]
        public string DisplayName { get { return this.GetPropertyValue<string>("showName") ?? this.Name; } }

        [ImplementPropertyType("galleryImages")]
        public List<int> GalleryImages { get { return this.GetPropertyValue<string>("galleryImages").Split(',').Select(int.Parse).ToList(); } }
        //public string IconForButton { get; set; }
        //public bool DoNotShowInPastProductions { get; set; }

        public List<MemberRolesInShow> MemberRoles { get; set; }

        //public Function(IPublishedContent content) : base(content)
        //{
        //    if (content.HasValue("galleryImages"))
        //    {
        //        GalleryImages = ;
        //    }
        //}
    }
}
