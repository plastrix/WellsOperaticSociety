﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using WellsOperaticSociety.PreCompiledViews;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Emails/SubscriptionCanceled.cshtml")]
    public partial class _Views_Emails_SubscriptionCanceled_cshtml : System.Web.Mvc.WebViewPage<WellsOperaticSociety.Models.EmailModels.SubscriptionCanceled>
    {
        public _Views_Emails_SubscriptionCanceled_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<html>\r\n<head>\r\n");

WriteLiteral("    ");

            
            #line 5 "..\..\Views\Emails\SubscriptionCanceled.cshtml"
Write(Html.Partial("~/Views/Emails/EmailStyling.cshtml"));

            
            #line default
            #line hidden
WriteLiteral("\r\n</head>\r\n<body>\r\n");

WriteLiteral("    ");

            
            #line 8 "..\..\Views\Emails\SubscriptionCanceled.cshtml"
Write(Html.Partial("~/Views/Emails/EmailHeader.cshtml", new ViewDataDictionary { { "baseUri", Model.BaseUri } }));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <p>\r\n        Your subscription for ");

            
            #line 10 "..\..\Views\Emails\SubscriptionCanceled.cshtml"
                         Write(Model.PlanName);

            
            #line default
            #line hidden
WriteLiteral(" was recently cancelled.\r\n    </p>\r\n    <p>\r\n        If this was a mistake, or yo" +
"u just need to update your card info please login or get in contact with us.\r\n  " +
"  </p>\r\n    <p>\r\n        Wells Operatic Society\r\n    </p>\r\n");

WriteLiteral("    ");

            
            #line 18 "..\..\Views\Emails\SubscriptionCanceled.cshtml"
Write(Html.Partial("~/Views/Emails/EmailFooter.cshtml"));

            
            #line default
            #line hidden
WriteLiteral("\r\n</body>\r\n</html>");

        }
    }
}
#pragma warning restore 1591
