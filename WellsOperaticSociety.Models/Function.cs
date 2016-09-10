using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.Models
{
    public class Function : StandardPage

    {
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Image { get; set; }
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
        public bool OpenLinkInNewWindow { get; set; }
        public string ShowName { get; set; }
        public string DisplayName { get { return ShowName ?? Name; }}
        public List<int> GalleryImages { get; set; }

        public List<MemberRolesInShow> MemberRoles { get; set; }

        public Function(IPublishedContent content) : base(content)
        {
            if (content.HasValue("galleryImages"))
            {
                GalleryImages = content.GetPropertyValue<string>("galleryImages").Split(',').Select(int.Parse).ToList();
            }
        }
    }
}
