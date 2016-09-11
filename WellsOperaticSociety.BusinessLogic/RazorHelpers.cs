using System.IO;
using System.Web.Mvc;

namespace WellsOperaticSociety.BusinessLogic
{
    public class RazorHelpers
    {
        public static string RenderRazorViewToString(string viewName, ControllerContext controllerContext, ViewDataDictionary viewData, TempDataDictionary tempDataDictionary = null)
        {
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempDataDictionary, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
