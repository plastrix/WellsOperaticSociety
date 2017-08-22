using WellsOperaticSociety.Models.UmbracoModels;

namespace WellsOperaticSociety.Models.EmailModels
{
    public class ShowVoucher :EmailBase
    {
        public Models.MemberModels.Member Member { get; set; }
        public string DateActive { get; set; }
        public string Key { get; set; }
        public Function Function { get; set; }
        public string Link { get; set; }
    }
}
