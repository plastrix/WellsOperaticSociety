using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using AutoMapper;
using Stripe;
using log4net;
using WellsOperaticSociety.BusinessLogic;
using WellsOperaticSociety.Models.Enums;
using WellsOperaticSociety.Models.MemberModels;

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

            var stripeEvent = StripeEventUtility.ParseEvent(json);
            StripeSubscription stripeSubscription;
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
                case Stripe.StripeEvents.CustomerSubscriptionCreated: _log.Info("Stripe Event: CustomerSubscriptionCreated"); break;
                case Stripe.StripeEvents.CustomerSubscriptionDeleted:
                    _log.Info("Stripe Event: CustomerSubscriptionDeleted");
                    stripeSubscription = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString());
                    CancelMembership(stripeSubscription);

                    break;
                case Stripe.StripeEvents.CustomerSubscriptionTrialWillEnd: var z = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString()); _log.Info("Stripe Event: CustomerSubscriptionTrialWillEnd"); break;
                case Stripe.StripeEvents.CustomerSubscriptionUpdated:
                    _log.Info("Stripe Event: CustomerSubscriptionUpdated");
                    stripeSubscription = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString());

                    if (stripeSubscription.CancelAtPeriodEnd)
                    {
                        CancelMembership(stripeSubscription);
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
                    //TODO: Email customer about issue and next steps
                    break;
                case Stripe.StripeEvents.InvoicePaymentSucceeded:
                    _log.Info("Stripe Event: InvoicePaymentSucceeded");
                    StripeInvoice invoice = Mapper<StripeInvoice>.MapFromJson(stripeEvent.Data.Object.ToString());

                    //if not for a subscription ignore
                    if (invoice.SubscriptionId.IsNullOrEmpty())
                        return;

                    var member = dataManager.GetActiveMember(invoice.CustomerId);
                    if (member == null)
                    {
                        _log.Error($"Tried to create/update a membership but no member could be found with the stripe id of {invoice.CustomerId}");
                        //TODO: send email to admins
                        return;
                    }

                    stripeSubscription = new StripeSubscriptionService(SensativeInformation.StripeKeys.SecretKey).Get(invoice.CustomerId, invoice.SubscriptionId);

                    //convert the plan to internal plan
                    if (stripeSubscription.StripePlan == null)
                    {
                        _log.Error($"Could not locate plan linked to subscription {invoice.SubscriptionId}");
                        //TODO: send email to admins
                        return;
                    }
                    var membershipType = BusinessLogic.Convert.StripePlanToMembershipType(stripeSubscription.StripePlan.Id);
                    if (membershipType == null)
                    {
                        _log.Error($"Tried to create a membership but could not convert the plan to the membershiptype for {member.Name} and stripe plan {stripeSubscription.StripePlan.Name}");
                        //TODO: send email to admins
                        return;
                    }
                    //if we already have a membership covering this subscription period update the plan
                    var membership =
                            dataManager
                                .GetMembershipsForUser(member.Id)
                                .FirstOrDefault(m => m.StripeSubscriptionId == stripeSubscription.Id && m.StartDate <= (stripeSubscription.CurrentPeriodStart ?? DateTime.Now.Date) && m.EndDate >= (stripeSubscription.CurrentPeriodEnd ?? DateTime.Now.Date));

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
                            StartDate = stripeSubscription.CurrentPeriodStart ?? DateTime.Now,
                            EndDate = stripeSubscription.CurrentPeriodEnd ?? DateTime.Now.AddYears(1),
                            Member = member.Id,
                            StripeSubscriptionId = stripeSubscription.Id
                        };
                        dataManager.AddOrUpdateMembership(m);
                    }
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
                //TODO: send email to admins
                return;
            }
            DataManager dataManager = new DataManager();
            var membership = dataManager.GetLatestMembership(subscription.Id);
            if (membership == null)
            {
                _log.Error($"Tried to update a membership but could not find the membership with the subscription id {subscription.Id}");
                //TODO: send email to admins
                return;
            }
            membership.CancelAtEnd = true;
            dataManager.AddOrUpdateMembership(membership);
        }
    }
}