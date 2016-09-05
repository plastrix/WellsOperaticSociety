using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WellsOperaticSociety.Models.AdminModels;

namespace WellsOperaticSociety.Web.Models
{
    public class ManageSeatsViewModel
    {
        public List<Seat> Seats { get; set; }
        public Seat NewSeat { get; set; }
    }
}