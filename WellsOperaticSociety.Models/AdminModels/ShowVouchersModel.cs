using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.AdminModels
{
    public class ShowVouchersModel
    {
        public int FunctionId { get; set; }
        public string ShowMemberVouchers { get; set; }
        public string MemberVouchers { get; set; }
        public string PatronVouchers { get; set; }
        [Required(ErrorMessage = "We need to know when the tickets will go on sale")]
        public DateTime BoxOfficeOpenDate { get; set; }
        [Required(ErrorMessage = "You must enter a booking url")]
        [Display(Name = "Booking Url")]
        public string BookingUrl { get; set; }
    }
}
