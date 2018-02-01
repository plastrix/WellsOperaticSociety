using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "You must supply a password")]
        [AllowHtml]
        public string Password { get; set; }
        [Required(ErrorMessage = "You must supply an email address")]
        [Display(Name = "Email")]
        public string Username { get; set; }
    }
}
