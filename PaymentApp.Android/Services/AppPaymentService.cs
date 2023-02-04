using Android.Content;
using PaymentApp.Droid.Services;
using PaymentApp.Services;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(AppPaymentService))]
namespace PaymentApp.Droid.Services
{
    public class AppPaymentService : IAppPaymentService
    {
        public string BhimPay(string pa, string pn, string amount)
        {
            BhimPayActivity.pa = pa;
            BhimPayActivity.pn = pn;
            BhimPayActivity.amount = amount;
            Intent intent = new Intent(nameof(BhimPayActivity));
            intent = new Intent(CrossCurrentActivity.Current.Activity, typeof(BhimPayActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
            return "";
        }

        public string GooglePay(string pa, string pn, string amount)
        {
            GooglePayActivity.pa = pa;
            GooglePayActivity.pn = pn;
            GooglePayActivity.amount = amount;
            Intent intent = new Intent(nameof(GooglePayActivity));
            intent = new Intent(CrossCurrentActivity.Current.Activity, typeof(GooglePayActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
            return "";
        }

        public string IciciPay(string amount)
        {
            return "";
        }

        public string PayTm(string pa, string pn, string amount)
        {
            PaytmActivity.pa = pa;
            PaytmActivity.pn = pn;
            PaytmActivity.amount = amount;
            Intent intent = new Intent(nameof(PaytmActivity));
            intent = new Intent(CrossCurrentActivity.Current.Activity, typeof(PaytmActivity));
            intent.PutExtra("amount", amount);
            intent.AddFlags(ActivityFlags.ClearTop);
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
            return "";
        }

        public string PhonePay(string pa, string pn, string amount)
        {
            PhonePeActivity.pa = pa;
            PhonePeActivity.pn = pn;
            PhonePeActivity.amount = amount;
            Intent intent = new Intent(nameof(PhonePeActivity));
            intent = new Intent(CrossCurrentActivity.Current.Activity, typeof(PhonePeActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
            return "";
        }
    }
}