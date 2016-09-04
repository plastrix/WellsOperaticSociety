using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace WellsOperaticSociety.Models.MemberModels
{
    public class MemberRolesInShow
    {
        public int MemberRolesInShowId { get; set; }
        [Required]
        public int? MemberId { get; set; }
        [Required]
        public string Group { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public int FunctionId { get; set; }
        [NotMapped]
        public Member Member { get; set; }
    }
}
