using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using log4net;
using Umbraco.Web.Mvc;
using WellsOperaticSociety.BusinessLogic;

namespace WellsOperaticSociety.Web.Controllers
{
    public class MainSurfaceController : SurfaceController
    {

        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult FunctionCards(int quantity =3)
        {
            DataManager manager = new DataManager();

            var model = manager.GetListOfUpcomingFunctions(quantity, 0);

            return PartialView("FunctionCards", model);

        }

        public ActionResult WhatsOn(int quantity = 10)
        {
            DataManager manager = new DataManager();

            var model = manager.GetListOfUpcomingFunctions(quantity, 0);

            return PartialView("WhatsOn", model);
        }

        public ActionResult PreviousProductions(int? pageSize, int? row)
        {
            pageSize = pageSize ?? 10;
            row = row ?? 0;

            DataManager manager = new DataManager();

            var model = manager.GetListOfExpiredFunctions((int)pageSize, (int)row);

            return PartialView("PreviousProductions", model);
        }

    }
}