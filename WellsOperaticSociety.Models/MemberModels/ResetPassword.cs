using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class ResetPassword
    {
        [Required]
        public string Token { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage = "Password must be at least 8 charachters long")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please re-enter your password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Passwords do not match")]
        public string RepeatePassword { get; set; }
    }
}
