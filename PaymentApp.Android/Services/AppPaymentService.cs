using Android.Content;
using PaymentApp.Droid.Services;
using PaymentApp.Services;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(AppPaymentService))]
namespace PaymentApp.Droid.Services
{
    public class AppPaymentService : IAppPaymentService
    {
        public string BhimPay(string amount)
        {

            Intent intent = new Intent(nameof(BhimPayActivity));
            intent = new Intent(CrossCurrentActivity.Current.Activity, typeof(BhimPayActivity));
            intent.PutExtra("amount", amount);
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
            intent.PutExtra("amount", amount);
            intent.AddFlags(ActivityFlags.ClearTop);
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
            return "";
        }

        public string IciciPay(string amount)
        {
            return "";
        }

        public string PayTm(string amount)
        {

            //Intent intent = new Intent(nameof(PayTmActivity));
            //intent = new Intent(CrossCurrentActivity.Current.Activity, typeof(PayTmActivity));
            //intent.PutExtra("amount", amount);
            //intent.AddFlags(ActivityFlags.ClearTop);
            //CrossCurrentActivity.Current.Activity.StartActivity(intent);
            return "";
        }

        public string PhonePay(string pa, string pn, string amount)
        {
            //Intent intent = new Intent(nameof(PhonePeActivity));
            //intent = new Intent(CrossCurrentActivity.Current.Activity, typeof(PhonePeActivity));
            //intent.PutExtra("amount", amount);
            //intent.AddFlags(ActivityFlags.ClearTop);
            //CrossCurrentActivity.Current.Activity.StartActivity(intent);
            return "";
        }
    }
}