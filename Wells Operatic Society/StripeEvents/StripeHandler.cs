using System.IO;
using System.Reflection;
using System.Web;
using Stripe;
using log4net;

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
                case Stripe.StripeEvents.CustomerSubscriptionCreated: var x = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString()); break;
                case Stripe.StripeEvents.CustomerSubscriptionDeleted: var y = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString()); break;
                case Stripe.StripeEvents.CustomerSubscriptionTrialWillEnd: var z = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString()); break;
                case Stripe.StripeEvents.CustomerSubscriptionUpdated: var a = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString()); break;
                case Stripe.StripeEvents.CustomerUpdated: _log.Info("Stripe Event: CustomerUpdated"); break;
                case Stripe.StripeEvents.InvoiceCreated: _log.Info("Stripe Event: InvoiceCreated"); break;
                case Stripe.StripeEvents.InvoiceItemCreated: _log.Info("Stripe Event: InvoiceItemCreated"); break;
                case Stripe.StripeEvents.InvoiceItemDeleted: _log.Info("Stripe Event: InvoiceItemDeleted"); break;
                case Stripe.StripeEvents.InvoiceItemUpdated: _log.Info("Stripe Event: InvoiceItemUpdated"); break;
                case Stripe.StripeEvents.InvoicePaymentFailed: _log.Info("Stripe Event: InvoicePaymentFailed"); break;
                case Stripe.StripeEvents.InvoicePaymentSucceeded: _log.Info("Stripe Event: InvoicePaymentSucceeded"); break;
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
                case Stripe.StripeEvents.TransferUpdated:_log.Info("Stripe Event: TransferUpdated"); break;
            }
        }
    }
}