using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Zone.UmbracoMapper;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class Membership
    {
        public int Member { get; set; }
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MembershipType { get; set; }

        public Membership(IPublishedContent content)
        {
            UmbracoMapper mapper = new UmbracoMapper();
            mapper.Map(content, this);
        }
    }
}
