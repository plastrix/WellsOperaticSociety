using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.EmailModels
{
    public class Payment :EmailBase
    {
        public string RecieptId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Amount { get; set; }
    }
}
