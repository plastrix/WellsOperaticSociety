using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using AutoMapper;
using Stripe;
using log4net;
using WellsOperaticSociety.BusinessLogic;
using WellsOperaticSociety.EmailService;
using WellsOperaticSociety.Models.Enums;
using WellsOperaticSociety.Models.MemberModels;
using WellsOperaticSociety.Web.Controllers;

namespace WellsOperaticSociety.Web.StripeEvents
{
    public class StripeHandler : IHttpHandler
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var json = new StreamReader(context.Request.InputStream).ReadToEnd();
            var html = string.Empty;
            var emailService = new EmailService.EmailHelpers();
            var c = new HttpContextWrapper(context);
            var routeData = new RouteData();
            routeData.Values.Add("controller", typeof(MembershipSurfaceController).Name
                                                    .ToLower()
                                                    .Replace("controller", ""));
            var controllerContext = new ControllerContext(new RequestContext(c, routeData), new MembershipSurfaceController());
            ViewDataDictionary viewData = new ViewDataDictionary();
            TempDataDictionary tempData = new TempDataDictionary();

            var stripeEvent = StripeEventUtility.ParseEvent(json);
            StripeSubscription stripeSubscription;
            StripeCustomer customer;
            DataManager dataManager = new DataManager();
            switch (stripeEvent.Type)
            {
                case Stripe.StripeEvents.AccountApplicationDeauthorized: _log.Info("Stripe Event: AccountApplicationDeauthorized"); break;
                case Stripe.StripeEvents.AccountUpdated: _log.Info("Stripe Event: AccountUpdated"); break;
                case Stripe.StripeEvents.ApplicationFeeCreated: _log.Info("Stripe Event: ApplicationFeeCreated"); break;
                case Stripe.StripeEvents.ApplicationFeeRefunded: _log.Info("Stripe Event: ApplicationFeeRefunded"); break;
                case Stripe.StripeEvents.BalanceAvailable: _log.Info("Stripe Event: BalanceAvailable"); break;
                case Stripe.StripeEvents.ChargeCaptured: _log.Info("Stripe Event: ChargeCaptured"); break;
                case Stripe.StripeEvents.ChargeDisputeClosed: _log.Info("Stripe Event: ChargeDisputeClosed"); break;
                case Stripe.StripeEvents.ChargeDisputeCreated: _log.Info("Stripe Event: ChargeDisputeCreated"); break;
                case Stripe.StripeEvents.ChargeDisputeFundsReinstated: _log.Info("Stripe Event: ChargeDisputeFundsReinstated"); break;
                case Stripe.StripeEvents.ChargeDisputeFundsWithdrawn: _log.Info("Stripe Event: ChargeDisputeFundsWithdrawn"); break;
                case Stripe.StripeEvents.ChargeDisputeUpdated: _log.Info("Stripe Event: ChargeDisputeUpdated"); break;
                case Stripe.StripeEvents.ChargeFailed: _log.Info("Stripe Event: ChargeFailed"); break;
                case Stripe.StripeEvents.ChargeRefunded: _log.Info("Stripe Event: ChargeRefunded"); break;
                case Stripe.StripeEvents.ChargeSucceeded: _log.Info("Stripe Event: ChargeSucceeded"); break;
                case Stripe.StripeEvents.ChargeUpdated: _log.Info("Stripe Event: ChargeUpdated"); break;
                case Stripe.StripeEvents.CouponCreated: _log.Info("Stripe Event: CouponCreated"); break;
                case Stripe.StripeEvents.CouponDeleted: _log.Info("Stripe Event: CouponDeleted"); break;
                case Stripe.StripeEvents.CustomerCreated: _log.Info("Stripe Event: CustomerCreated"); break;
                case Stripe.StripeEvents.CustomerDeleted: _log.Info("Stripe Event: CustomerDeleted"); break;
                case Stripe.StripeEvents.CustomerDiscountCreated: _log.Info("Stripe Event: CustomerDiscountCreated"); break;
                case Stripe.StripeEvents.CustomerDiscountDeleted: _log.Info("Stripe Event: CustomerDiscountDeleted"); break;
                case Stripe.StripeEvents.CustomerDiscountUpdated: _log.Info("Stripe Event: CustomerDiscountUpdated"); break;
                case Stripe.StripeEvents.CustomerSourceCreated: _log.Info("Stripe Event: CustomerSourceCreated"); break;
                case Stripe.StripeEvents.CustomerSourcedDeleted: _log.Info("Stripe Event: CustomerSourcedDeleted"); break;
                case Stripe.StripeEvents.CustomerSourceUpdated: _log.Info("Stripe Event: CustomerSourceUpdated"); break;
                case Stripe.StripeEvents.CustomerSubscriptionCreated:
                    _log.Info("Stripe Event: CustomerSubscriptionCreated");
                    stripeSubscription = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString());
                    if (stripeSubscription.CustomerId.IsNullOrEmpty())
                    {
                        return;
                    }
                    customer = GetCustomer(stripeSubscription.CustomerId);
                    if (customer == null)
                        return;
                    //Thank you for signing up
                    var subCreatedModel = new WellsOperaticSociety.Models.EmailModels.SubscriptionCreated();
                    subCreatedModel.BaseUri = UrlHelpers.GetBaseUrl();
                    subCreatedModel.PlanName = stripeSubscription.StripePlan.Name;
                    viewData.Model = subCreatedModel;
                    html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/SubscriptionCreated.cshtml", controllerContext, viewData, tempData);

                    //Generate email
                    emailService.SendEmail(customer.Email, "Thank you for setting up a subscription", html);
                    break;
                case Stripe.StripeEvents.CustomerSubscriptionDeleted:
                    _log.Info("Stripe Event: CustomerSubscriptionDeleted");
                    //Email notification of cancelation
                    stripeSubscription = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString());
                    CancelMembership(stripeSubscription);
                    if (stripeSubscription.CustomerId.IsNullOrEmpty())
                    {
                        return;
                    }
                    customer = GetCustomer(stripeSubscription.CustomerId);
                    if (customer == null)
                        return;
                    //Cancelation
                    var subCanceledModel = new WellsOperaticSociety.Models.EmailModels.SubscriptionCanceled();
                    subCanceledModel.BaseUri = UrlHelpers.GetBaseUrl();
                    subCanceledModel.PlanName = stripeSubscription.StripePlan.Name;
                    viewData.Model = subCanceledModel;
                    html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/SubscriptionCanceled.cshtml", controllerContext, viewData, tempData);

                    //Generate email
                    emailService.SendEmail(customer.Email, "Your subscription has been cancelled", html);
                    break;
                case Stripe.StripeEvents.CustomerSubscriptionTrialWillEnd: var z = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString()); _log.Info("Stripe Event: CustomerSubscriptionTrialWillEnd"); break;
                case Stripe.StripeEvents.CustomerSubscriptionUpdated:
                    _log.Info("Stripe Event: CustomerSubscriptionUpdated");

                    stripeSubscription = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString());
                    //canacel if cancel at end of period set
                    if (stripeSubscription.CancelAtPeriodEnd)
                    {
                        CancelMembership(stripeSubscription);
                        if (stripeSubscription.CustomerId.IsNullOrEmpty())
                        {
                            return;
                        }
                        customer = GetCustomer(stripeSubscription.CustomerId);
                        if (customer == null)
                            return;
                        //Cancelation
                        var sCanModel = new WellsOperaticSociety.Models.EmailModels.SubscriptionCanceled();
                        sCanModel.BaseUri = UrlHelpers.GetBaseUrl();
                        sCanModel.PlanName = stripeSubscription.StripePlan.Name;
                        viewData.Model = sCanModel;
                        html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/SubscriptionCanceled.cshtml", controllerContext, viewData, tempData);

                        //Generate email
                        emailService.SendEmail(customer.Email, "Your subscription has been cancelled", html);
                    }
                    else
                    {
                        
                        //update subscription
                        AddOrUpdateMembership(stripeSubscription);
                        customer = GetCustomer(stripeSubscription.CustomerId);
                        if (customer == null)
                            return;
                        var sCanModel = new WellsOperaticSociety.Models.EmailModels.SubscriptionCanceled();
                        sCanModel.BaseUri = UrlHelpers.GetBaseUrl();
                        sCanModel.PlanName = stripeSubscription.StripePlan.Name;
                        viewData.Model = sCanModel;
                        html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/SubscriptionUpdated.cshtml", controllerContext, viewData, tempData);
                        emailService.SendEmail(customer.Email, "Your subscription has been updated", html);
                    }

                    break;
                case Stripe.StripeEvents.CustomerUpdated: _log.Info("Stripe Event: CustomerUpdated"); break;
                case Stripe.StripeEvents.InvoiceCreated:
                    _log.Info("Stripe Event: InvoiceCreated");
                    //TODO: This is where to hold membership if no activity on account
                    break;
                case Stripe.StripeEvents.InvoiceItemCreated: _log.Info("Stripe Event: InvoiceItemCreated"); break;
                case Stripe.StripeEvents.InvoiceItemDeleted: _log.Info("Stripe Event: InvoiceItemDeleted"); break;
                case Stripe.StripeEvents.InvoiceItemUpdated: _log.Info("Stripe Event: InvoiceItemUpdated"); break;
                case Stripe.StripeEvents.InvoicePaymentFailed:
                    _log.Info("Stripe Event: InvoicePaymentFailed");
                    StripeInvoice i = Mapper<StripeInvoice>.MapFromJson(stripeEvent.Data.Object.ToString());
                    if (i.CustomerId.IsNullOrEmpty())
                    {
                        return;
                    }
                    customer = GetCustomer(i.CustomerId);
                    if(customer == null)
                        return;
                    //Email customer about issue and next steps
                    var paymentFailedModel = new WellsOperaticSociety.Models.EmailModels.PaymentFailed();
                    paymentFailedModel.BaseUri = UrlHelpers.GetBaseUrl();
                    paymentFailedModel.ReceiptId = i.ReceiptNumber;
                    paymentFailedModel.StartDate = i.PeriodStart.ToShortDateString();
                    paymentFailedModel.EndDate = i.PeriodEnd.ToShortDateString();
                    paymentFailedModel.Amount = i.AmountDue.ToString("C");

                    viewData.Model = paymentFailedModel;
                    html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/PaymentFailed.cshtml", controllerContext, viewData, tempData);

                    //Generate email
                    emailService.SendEmail(customer.Email, "Oops! Your membership payment failed", html);
                    break;
                case Stripe.StripeEvents.InvoicePaymentSucceeded:
                    _log.Info("Stripe Event: InvoicePaymentSucceeded");
                    StripeInvoice invoice = Mapper<StripeInvoice>.MapFromJson(stripeEvent.Data.Object.ToString());

                    //if not for a subscription ignore
                    if (invoice.SubscriptionId.IsNullOrEmpty())
                        return;
                    if (invoice.CustomerId.IsNullOrEmpty())
                    {
                        return;
                    }
                    customer = GetCustomer(invoice.CustomerId);
                    if (customer == null)
                        return;
                    //Email customer about issue and next steps
                    var paymentModel = new WellsOperaticSociety.Models.EmailModels.Payment();
                    paymentModel.BaseUri = UrlHelpers.GetBaseUrl();
                    paymentModel.RecieptId = invoice.ReceiptNumber;
                    paymentModel.StartDate = invoice.PeriodStart.ToShortDateString();
                    paymentModel.EndDate = invoice.PeriodEnd.ToShortDateString();
                    paymentModel.Amount = (invoice.AmountDue / 100m).ToString("N");

                    viewData.Model = paymentModel;
                    html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/Payment.cshtml", controllerContext, viewData, tempData);

                    //Generate email
                    emailService.SendEmail(customer.Email, "Payment to Wells Operatic Society", html);
                    stripeSubscription = new StripeSubscriptionService(SensativeInformation.StripeKeys.SecretKey).Get(invoice.CustomerId, invoice.SubscriptionId);
                    AddOrUpdateMembership(stripeSubscription);

                    break;
                case Stripe.StripeEvents.InvoiceUpdated: _log.Info("Stripe Event: InvoiceUpdated"); break;
                case Stripe.StripeEvents.Ping: _log.Info("Stripe Event: Ping"); break;
                case Stripe.StripeEvents.PlanCreated: _log.Info("Stripe Event: PlanCreated"); break;
                case Stripe.StripeEvents.PlanDeleted: _log.Info("Stripe Event: PlanDeleted"); break;
                case Stripe.StripeEvents.PlanUpdated: _log.Info("Stripe Event: PlanUpdated"); break;
                case Stripe.StripeEvents.RecipientCreated: _log.Info("Stripe Event: RecipientCreated"); break;
                case Stripe.StripeEvents.RecipientDeleted: _log.Info("Stripe Event: RecipientDeleted"); break;
                case Stripe.StripeEvents.RecipientUpdated: _log.Info("Stripe Event: RecipientUpdated"); break;
                case Stripe.StripeEvents.TransferCreated: _log.Info("Stripe Event: TransferCreated"); break;
                case Stripe.StripeEvents.TransferFailed: _log.Info("Stripe Event: TransferFailed"); break;
                case Stripe.StripeEvents.TransferPaid: _log.Info("Stripe Event: TransferPaid"); break;
                case Stripe.StripeEvents.TransferReversed: _log.Info("Stripe Event: TransferReversed"); break;
                case Stripe.StripeEvents.TransferUpdated: _log.Info("Stripe Event: TransferUpdated"); break;
            }
        }

        private void CancelMembership(StripeSubscription subscription)
        {
            if (subscription == null)
            {
                _log.Error($"Tried to cancel a membership but the subscription passed was null so could not work out which membership to cancel");
                EmailHelpers emailService = new EmailHelpers();
                emailService.SendEmail("info@wellslittletheatre.com", "Error canceling membership from subscription", $"Tried to cancel a membership but the subscription passed was empty so could not work out which membership to cancel. You will need to investigate.");
                return;
            }
            DataManager dataManager = new DataManager();
            var membership = dataManager.GetLatestMembership(subscription.Id);
            if (membership == null)
            {
                _log.Error($"Tried to update a membership to cancel it but could not find the membership with the subscription id {subscription.Id}");
                EmailHelpers emailService = new EmailHelpers();
                emailService.SendEmail("info@wellslittletheatre.com", "Error canceling membership from subscription", $"Tried to update a membership to cancel it but could not find the membership with the subscription id {subscription.Id}");
                return;
            }
            membership.CancelAtEnd = true;
            dataManager.AddOrUpdateMembership(membership);
        }

        private StripeCustomer GetCustomer(string stripeUserId)
        {
            try
            {
                StripeCustomerService custServce = new StripeCustomerService(SensativeInformation.StripeKeys.SecretKey);
                return custServce.Get(stripeUserId);
            }
            catch (StripeException e)
            {
                _log.Error("There was an error retrieving the user from stripe. " + e.Message);
                return null;
            }
        }

        private void AddOrUpdateMembership(StripeSubscription subscription)
        {
            if (subscription == null)
            {
                _log.Error($"Tried to add or update a membership but the subscription passed was null");
                EmailHelpers emailService = new EmailHelpers();
                emailService.SendEmail("info@wellslittletheatre.com", "Error creating or updating membership for user from subscription", $"Tried to add or update a membership but the subscription passed was empty. So we will need to explore to see if anything needs to be done.");
                return;
            }
            DataManager dataManager = new DataManager();
            var member = dataManager.GetMember(subscription.CustomerId);
            if (member == null)
            {
                _log.Error($"Tried to create/update a membership but no member could be found with the stripe id of {subscription.CustomerId}");
                EmailHelpers emailService = new EmailHelpers();
                emailService.SendEmail("info@wellslittletheatre.com", "Error creating or updating membership for user from subscription", $"Tried to create/update a membership but no member could be found with the stripe id of {subscription.CustomerId}");
                return;
            }

            //convert the plan to internal plan
            if (subscription.StripePlan == null)
            {
                _log.Error($"Could not locate plan linked to subscription {subscription.Id}");
                EmailHelpers emailService = new EmailHelpers();
                emailService.SendEmail("info@wellslittletheatre.com", "Error creating membership for user from subscription", $"Tried to create a membership but no stripeplan was associated with the subscription?!For {member.Name}");
                return;
            }
            var membershipType = BusinessLogic.Convert.StripePlanToMembershipType(subscription.StripePlan.Id);
            if (membershipType == null)
            {
                _log.Error($"Tried to create a membership but could not convert the plan to the membershiptype for {member.Name} and stripe plan {subscription.StripePlan.Name}");
                EmailHelpers emailService = new EmailHelpers();
                emailService.SendEmail("info@wellslittletheatre.com","Error creating membership for user from subscription", $"Tried to create a membership but could not convert the plan to the membershiptype for {member.Name} and stripe plan {subscription.StripePlan.Name}");
                return;
            }
            //if we already have a membership covering this subscription period update the plan
            var membership =
                    dataManager
                        .GetMembershipsForUser(member.Id)
                        .FirstOrDefault(m => m.StripeSubscriptionId == subscription.Id && m.StartDate <= (subscription.CurrentPeriodStart ?? DateTime.Now.Date) && m.EndDate >= (subscription.CurrentPeriodEnd ?? DateTime.Now.Date));

            if (membership != null)
            {
                membership.MembershipType = (MembershipType)membershipType;
                dataManager.AddOrUpdateMembership(membership);
            }
            else
            {
                Membership m = new Membership
                {
                    IsSubscription = true,
                    MembershipType = (MembershipType)membershipType,
                    StartDate = subscription.CurrentPeriodStart ?? DateTime.Now,
                    EndDate = subscription.CurrentPeriodEnd?.AddDays(-1).Date ?? DateTime.Now.AddYears(1).AddDays(-1).Date,
                    Member = member.Id,
                    StripeSubscriptionId = subscription.Id
                };
                dataManager.AddOrUpdateMembership(m);
            }
        }
    }
}