using System.Collections.Generic;
using WellsOperaticSociety.Models.UmbracoModels;

namespace WellsOperaticSociety.Web.Models
{
    public class PreviousProductionsViewModel
    {
        public List<Function> Functions { get; set; }
        public int PageSize { get; set; }
        public int Row { get; set; }
        public int TotalItemCount { get; set; }
    }
}