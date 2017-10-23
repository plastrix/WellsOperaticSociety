using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.AdminModels
{
    public class FunctionSettingsModel
    {
        [Required]
        public int FunctionId { get; set; }
        [Display(Name ="Are the tickets being sold online?")]
        public bool IsAvailableOnline { get; set; }
        [Display(Name = "When will the tickets be sold online?")]
        public DateTime? IsAvailableOnlineFrom { get; set; }
        [Display(Name = "Are the tickets being sold at the boxo ffice?")]
        public bool IsAvailableFromBoxOffice { get; set; }
        [Display(Name = "When will the tickets be sold from the box office?")]
        public DateTime? IsAvailableFromBoxOfficeFrom { get; set; }
    }
}
