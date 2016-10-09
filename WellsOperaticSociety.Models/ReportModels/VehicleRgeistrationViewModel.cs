using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.ReportModels
{
    public class VehicleRgeistrationViewModel : ReportBase
    {
        public List<VehicleRegistrationModel> RegistrationList { get; set; }
    }
}
