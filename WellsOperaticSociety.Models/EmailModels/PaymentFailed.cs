using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.EmailModels
{
    public class PaymentFailed : EmailBase
    {
        public string ReceiptId { get; set; }
        public string BillTo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Amount { get; set; }
    }
}
