using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WellsOperaticSociety.Models
{
    public class ReCaptcha
    {
        private string _success;

        [JsonProperty("success")]
        public string Success
        {
            get { return _success; }
            set { _success = value; }
        }

        private List<string> _errorCodes;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return _errorCodes; }
            set { _errorCodes = value; }
        }

        public static string Validate(string encodedResponse,string privateKey)
        {
            var client = new System.Net.WebClient();
            var reply =client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={privateKey}&response={encodedResponse}");
            var captchaResponse = JsonConvert.DeserializeObject<ReCaptcha>(reply);
            return captchaResponse.Success;

        }
    }
}
