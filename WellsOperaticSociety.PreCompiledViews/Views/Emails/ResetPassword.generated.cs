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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Emails/ResetPassword.cshtml")]
    public partial class _Views_Emails_ResetPassword_cshtml : System.Web.Mvc.WebViewPage<WellsOperaticSociety.Models.EmailModels.ResetPassword>
    {
        public _Views_Emails_ResetPassword_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<html>\r\n<head>\r\n<style");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(">\r\n    p{margin-bottom:1em;}\r\n</style>\r\n\r\n</head>\r\n<body>\r\n<p>\r\n    Hi ");

            
            #line 12 "..\..\Views\Emails\ResetPassword.cshtml"
  Write(Model.Member.Name);

            
            #line default
            #line hidden
WriteLiteral(",\r\n</p>\r\n<p>\r\n    You have requested to reset your password. Please use the link " +
"below to reset your password.\r\n</p>\r\n<p><a");

WriteAttribute("href", Tuple.Create(" href=\"", 314), Tuple.Create("\"", 332)
            
            #line 17 "..\..\Views\Emails\ResetPassword.cshtml"
, Tuple.Create(Tuple.Create("", 321), Tuple.Create<System.Object, System.Int32>(Model.Link
            
            #line default
            #line hidden
, 321), false)
);

WriteLiteral(">");

            
            #line 17 "..\..\Views\Emails\ResetPassword.cshtml"
                    Write(Model.Link);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n</p>\r\n<p>If you did not request to reset your password just ignore this ema" +
"il.</p>\r\n<p>\r\n    Wells Operatic Society\r\n</p>\r\n</body>\r\n</html>");

        }
    }
}
#pragma warning restore 1591
