using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.AdminModels
{
    public class Seat
    {
        public int SeatId { get; set; }
        [Required(ErrorMessage = "You must supply a seat number")]
        public string SeatNumber { get; set; }
        [Required(ErrorMessage = "You must enter a description")]
        public string Description { get; set; }
    }
}
