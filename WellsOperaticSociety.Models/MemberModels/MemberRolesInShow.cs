using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class MemberRolesInShow
    {
        public int MemberRolesInShowId { get; set; }
        [Display(Name = "Member")]
        [Required(ErrorMessage = "You must select a valid member")]
        [Microsoft.Build.Framework.Required]
        public int? MemberId { get; set; }
        [Required(ErrorMessage = "You must supply a group")]
        [Microsoft.Build.Framework.Required]
        public string Group { get; set; }
        [Microsoft.Build.Framework.Required]
        public string Role { get; set; }
        [Required(ErrorMessage = "There is no function assigned. So we cannot add the user to the function")]
        [Microsoft.Build.Framework.Required]
        public int FunctionId { get; set; }
        [NotMapped]
        public Member Member { get; set; }
    }
}
