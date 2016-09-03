using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Zone.UmbracoMapper;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class Manual
    {
        public int Document { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public Manual(IPublishedContent content)
        {
            var umbracoMapper = new UmbracoMapper();
            umbracoMapper.Map(content,this);
        }
    }
}
