using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.Enums
{
    public enum NodaLongServiceAward
    {
        [Display(Name = "Ten years")]
        TenYears,
        [Display(Name = "Fifteen years")]
        FifteenYears,
        [Display(Name = "Twenty years")]
        TwentyYears,
        [Display(Name = "Twenty five years")]
        TwentyFive,
        [Display(Name = "Thirty years")]
        ThirtyYears,
        [Display(Name = "Thirty five years")]
        ThirtyFiveYears,
        [Display(Name = "Fourty years")]
        FourtyYears,
        [Display(Name = "Fourty five years")]
        FourtyFiveYears,
        [Display(Name = "Fifty years")]
        FiftyYears,
        [Display(Name = "Fifty five years")]
        FiftyFiveYears,
        [Display(Name = "Sixty years")]
        SixtyYears,
        [Display(Name = "Sixty five years")]
        SixtyFiveYears,
        [Display(Name = "Seventy years")]
        SeventyYears
    }
}
