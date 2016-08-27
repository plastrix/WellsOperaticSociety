using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace WellsOperaticSociety.Models
{
    public class Function : StandardPage

    {
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Image { get; set; }

        public Function(IPublishedContent content) : base(content)
        {
        }
    }
}
