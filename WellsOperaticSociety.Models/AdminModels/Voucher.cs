using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using WellsOperaticSociety.Models.MemberModels;

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
        public Member Member { get; set; }
        [NotMapped]
        public Function Function { get; set; }
    }
}
