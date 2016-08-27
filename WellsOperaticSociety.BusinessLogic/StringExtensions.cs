using System;
using System.Web;

namespace WellsOperaticSociety.BusinessLogic
{
    public static class StringExtensions
    {
        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return $"{url.Scheme}://{url.Host}{port}{VirtualPathUtility.ToAbsolute(relativeUrl)}";
        }
    }
}
