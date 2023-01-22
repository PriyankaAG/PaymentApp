using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace PaymentApp.Droid.Services
{
    [Activity(Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class BhimPayActivity : Activity
    {
        string status = "";
        string TrnxacsnId = "";

        List<string> ResponseList = new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            try
            {
                string amount = Intent.GetStringExtra("amount");

                long tsLong = JavaSystem.CurrentTimeMillis() / 1000;
                string transaction_ref_id = Guid.NewGuid().ToString().Substring(0, 10) + "UPI";
                string transaction_ref = Guid.NewGuid().ToString().Substring(0, 10);
                //Log.WriteLine("TR Reference ID==>", "" + transaction_ref_id, "Test");

                using (var uri = new Android.Net.Uri.Builder()
                                               .Scheme("upi")
                                                .Authority("pay")
                                                 .AppendQueryParameter("pa", "PaymentUpiId")
                                                // .AppendQueryParameter("pa", "7017958029@upi")
                                                .AppendQueryParameter("pn", "Nonco")
                                                .AppendQueryParameter("mc", "0000")
                                                .AppendQueryParameter("tid", transaction_ref)
                                                .AppendQueryParameter("tr", transaction_ref_id)
                                                .AppendQueryParameter("tn", "Pay to nonco")
                                                .AppendQueryParameter("am", amount)
                                                .AppendQueryParameter("cu", "INR")
                                                .AppendQueryParameter("url", "https://www.sample.in")

                                                .Build())
                {
                    Intent = new Intent(Intent.ActionView);
                    Intent.SetData(uri);
                    if (IsAppInstalled("in.org.npci.upiapp"))
                    {
                        Intent.SetPackage("in.org.npci.upiapp");
                        StartActivityForResult(Intent, 9999);
                    }

                    else
                    {
                        var package = PackageName;
                        Toast.MakeText(Android.App.Application.Context, "Bhim is not available in this device", ToastLength.Long).Show();
                        this.Finish();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, "Payment through Bhim is failed", ToastLength.Long).Show();
                this.Finish();
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            try
            {
                if (requestCode == 9999)
                {
                    Console.WriteLine("Bhim pay result", data);
                    if (resultCode == Result.Ok)
                    {
                        GetResponseFromIntent(data?.Extras);
                    }
                    else if (resultCode == Result.Canceled)
                    {
                        Toast.MakeText(Android.App.Application.Context, "Payment through Bhim failed", ToastLength.Long).Show();
                    }

                }
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Exception while Bhim payment :" + ex.Message);
                ShowToast("Payment through Bhim failed");  //! Failed
            }
            if (status == "success")
            {
                var tranData = new Tuple<bool, string>(true, TrnxacsnId);
                Console.Write("Phonepe Messenging center [] status : success");
                MessagingCenter.Send("PayStatus", "PayStatus", tranData);
            }
            else
            {
                var tranData = new Tuple<bool, string>(false, null);
                Console.Write("Phonepe Messenging center [] status : failed");
                MessagingCenter.Send("PayStatus", "PayStatus", tranData);
            }

            Finish();
        }

        private void GetResponseFromIntent(Bundle extras)
        {
            Dictionary<string, string> dict;
            dict = new Dictionary<string, string>();
            if (extras != null)
            {
                foreach (var key in extras.KeySet())
                {
                    dict.Add(key, extras.Get(key).ToString());
                    ResponseList.Add(key + ":" + extras.Get(key).ToString());
                    if (key == "Status" && extras.Get(key).ToString().Contains("FAILURE"))
                    {
                        Toast.MakeText(Android.App.Application.Context, "Payment through Bhim fail", ToastLength.Long).Show();
                        status = "failed";
                    }
                    if (key == "Status" && extras.Get(key).ToString().Contains("SUCCESS"))
                    {
                        Toast.MakeText(Android.App.Application.Context, "Payment through Bhim success", ToastLength.Long).Show();
                        status = "success";
                    }
                    if (key == "responseCode")
                    {
                        Console.WriteLine("Response code [] " + extras.Get(key).ToString());
                    }
                    if (key == "txnid")
                    {
                        TrnxacsnId = extras.Get(key).ToString();
                    }
                }
            }
        }

        private bool IsAppInstalled(string packageName)
        {
            PackageManager pm = this.PackageManager;
            bool installed = false;
            try
            {
                pm.GetPackageInfo(packageName, PackageInfoFlags.Activities);
                installed = true;
            }
            catch (PackageManager.NameNotFoundException e)
            {
                installed = false;
            }

            return installed;
        }
        private void ShowToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }


    }
}