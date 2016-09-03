using System.Globalization;
using Microsoft.Build.Framework;
using Umbraco.Core.Models;
using Umbraco.Web.Models;


namespace WellsOperaticSociety.Models.MemberModels
{
    public class LoginModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
