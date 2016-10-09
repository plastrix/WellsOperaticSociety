using System.ComponentModel.DataAnnotations;


namespace WellsOperaticSociety.Models.MemberModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "You must supply a password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "You must supply an email address")]
        [Display(Name = "Email")]
        public string Username { get; set; }
    }
}
