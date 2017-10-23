using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellsOperaticSociety.Models.UmbracoModels;

namespace WellsOperaticSociety.Models.StandardModels
{
    public class BoxOfficeModel
    {
        public IEnumerable<BoxOfficeTime> OpeningTimes { get; set; }
        public IEnumerable<Function> FunctionsAvailable { get; set; }
    }
}
