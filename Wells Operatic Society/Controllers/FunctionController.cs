using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;
using WellsOperaticSociety.Models;

namespace WellsOperaticSociety.Web.Controllers
{
    public class FunctionController : Umbraco.Web.Mvc.RenderMvcController
    {
        // GET: Function
        public override ActionResult Index(RenderModel model)
        {
            var func = new Function(CurrentPage);
            return CurrentTemplate(func);
        }
    }
}