using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class ResetPassword
    {
        [Required]
        public string Token { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage = "Password must be at least 8 charachters long")]
        [AllowHtml]
        public string Password { get; set; }
        [Display(Name = "Repeat Password")]
        [Required(ErrorMessage = "Please re-enter your password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage = "Passwords do not match")]
        [AllowHtml]
        public string RepeatePassword { get; set; }
    }
}
