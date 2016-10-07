using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellsOperaticSociety.Models.ServiceModels
{
    public class AuthorisationToken
    {
        public int AuthorisationTokenId { get; set; }
        public string Token { get; set; }
        public int Member { get; set; }
        public bool Used { get; set; }
        public DateTime Created { get; set; }
    }
}
