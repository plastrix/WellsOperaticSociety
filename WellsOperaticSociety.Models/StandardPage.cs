using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Web.Models;
using Zone.UmbracoMapper;

namespace WellsOperaticSociety.Models
{
    public class StandardPage : BaseNodeViewModel
    {
        public StandardPage(IPublishedContent content)
        {
            UmbracoMapper mapper = new UmbracoMapper();
            mapper.Map(content, this);
        }
    }
}
