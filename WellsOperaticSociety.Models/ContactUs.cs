using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WellsOperaticSociety.Models
{
    public class ContactUs
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your email")]
        [DataType(DataType.EmailAddress,ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a message")]
        public string Message { get; set; }
    }
}
