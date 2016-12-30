using System.Reflection;
using System.Web.Mvc;
using log4net;
using Umbraco.Web.Mvc;
using WellsOperaticSociety.BusinessLogic;
using WellsOperaticSociety.Models;
using WellsOperaticSociety.Models.EmailModels;
using WellsOperaticSociety.Web.Models;

namespace WellsOperaticSociety.Web.Controllers
{
    public class MainSurfaceController : SurfaceController
    {

        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult FunctionCards(int quantity = 3)
        {
            DataManager manager = new DataManager();

            var model = manager.GetUpcomingFunctions(quantity, 0);

            return PartialView("FunctionCards", model);

        }

        public ActionResult WhatsOn(int quantity = 10)
        {
            DataManager manager = new DataManager();

            var model = manager.GetUpcomingFunctions(quantity, 0);

            return PartialView("WhatsOn", model);
        }

        public ActionResult PreviousProductions(int? pageSize, int? row)
        {
            pageSize = pageSize ?? 10;
            row = row ?? 0;

            DataManager manager = new DataManager();

            var model = new PreviousProductionsViewModel()
            {
                Functions = manager.GetExpiredFunctions((int) pageSize, (int) row),
                PageSize = (int) pageSize,
                Row = (int) row,
                TotalItemCount = manager.GetCountOfExpiredFunctions()
            };

            return PartialView("PreviousProductions", model);
        }

        public ActionResult ContactUsForm()
        {
            var model = new ContactUs();

            return PartialView("ContactUsForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitContactUsForm(ContactUs model)
        {
            if (ModelState.IsValid)
            {
                string encodedResponse = Request.Form["g-Recaptcha-Response"];
                var response = ReCaptcha.Validate(encodedResponse, SensativeInformation.ReCaptchaKeys.ReCaptchaSecretKey);
                bool isCaptchaValid = response.ToLower() == "true"
                        ? true
                        : false;
                if (!isCaptchaValid)
                {
                    ModelState.AddModelError("", "Please verify you are not a robot");
                    return CurrentUmbracoPage();
                }

                ContactUsEmailModel emailModel = new ContactUsEmailModel();
                emailModel.Info = model;
                emailModel.BaseUri = UrlHelpers.GetBaseUrl();
                ViewData.Model = emailModel;
                var html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/ContactUsEmail.cshtml", ControllerContext, ViewData, TempData);
                var emailService = new EmailService.EmailHelpers();
                emailService.SendEmail("info@wellslittletheatre.com","Query from contact form", html);
                TempData["Success"] = "That is winging its way to us now. We will be in contact as soon as we can.";
                return RedirectToCurrentUmbracoPage();
            }
            return CurrentUmbracoPage();
        }
    }
}