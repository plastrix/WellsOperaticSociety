using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WellsOperaticSociety.Models.StandardModels;

namespace WellsOperaticSociety.Web.Models
{
    public class ManageBoxOfficeTimesViewModel
    {
        public BoxOfficeTime BoxOfficeTime { get; set; }
        public List<BoxOfficeTime> BoxOfficeTimesList { get; set; }
    }
}