using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Web.Models.ContentEditing;

namespace WellsOperaticSociety.BusinessLogic
{
    public static class UrlHelpers
    {
        public static string GetBaseUrl()
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                   HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
        }
    }
}
