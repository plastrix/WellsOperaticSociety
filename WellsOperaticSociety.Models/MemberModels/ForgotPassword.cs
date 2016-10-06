using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Please enter the email address associated with your account")]
        [DataType(DataType.EmailAddress,ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }
}
