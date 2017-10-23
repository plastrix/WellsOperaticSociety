using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;
using WellsOperaticSociety.Models.UmbracoModels;

namespace WellsOperaticSociety.Models.AdminModels
{
    public class Voucher
    {
        public int VoucherId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Required]
        public int FunctionId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Required]
        public int MemberId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Required]
        public string Key { get; set; }
        [NotMapped]
        public MemberModels.Member Member { get; set; }
        [NotMapped]
        public Function Function { get; set; }
    }
}
