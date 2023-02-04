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
using Java.Interop;
using Java.Lang;

namespace PaymentApp.Droid.Services
{
    [Activity(Label = "PhonePeActivity", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class PhonePeActivity : Activity
    {
        string status = "";
        List<string> ResponseList = new List<string>();

        public string TrnxacsnId { get; private set; }

        public static string pn { get; set; }
        public static string pa { get; set; }
        public static string amount { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            try
            {
                string amount = "1"; // Intent.GetStringExtra("amount");
                long tsLong = JavaSystem.CurrentTimeMillis() / 1000;
                string transaction_ref_id = tsLong.ToString() + "UPI";
                using (var uri = new Android.Net.Uri.Builder()
                                                .Scheme("upi")
                                                .Authority("pay")
                                                .AppendQueryParameter("pa", pa)
                                                //  .AppendQueryParameter("pa", pa)
                                                .AppendQueryParameter("pn", pn)
                                                .AppendQueryParameter("tn", "Test integration note")
                                                .AppendQueryParameter("tr", transaction_ref_id)
                                                .AppendQueryParameter("am", amount)
                                                .AppendQueryParameter("cu", "INR")
                                                .Build())
                {
                    Intent = new Intent(Intent.ActionView);
                    Intent.SetData(uri);
                    if (IsAppInstalled("com.phonepe.app"))
                    {
                        Intent.SetPackage("com.phonepe.app");
                        StartActivityForResult(Intent, 9999);
                    }

                    else
                    {
                        var package = PackageName;
                        Toast.MakeText(Android.App.Application.Context, "Phonepe is not available in this device", ToastLength.Long).Show();
                        this.Finish();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, "Payment through PhonePe failed", ToastLength.Long).Show();
                Console.WriteLine("Error", ex.Message);
                this.Finish();
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
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Canceled)
            {

                Toast.MakeText(Android.App.Application.Context, "Payment through PhonePe Failed", ToastLength.Long).Show();
            }
            else if (resultCode == Result.Ok)
            {
                var b = data?.Extras;
                var d = new Dictionary<string, string>();
                if (b != null)
                {
                    foreach (var key in b.KeySet())
                    {
                        d.Add(key, b.Get(key).ToString());
                        ResponseList.Add(key + ":" + b.Get(key).ToString());
                        if (key == "Status" && b.Get(key).ToString().Contains("Failure"))
                        {
                            Toast.MakeText(Android.App.Application.Context, "Payment through Phonepe fail", ToastLength.Long).Show();
                            status = "Failed";
                        }
                        if (key == "Status" && b.Get(key).ToString().Contains("Success"))
                        {
                            Toast.MakeText(Android.App.Application.Context, "Payment through Phonepe success", ToastLength.Long).Show();
                            status = "Success";
                        }
                        if (key == "bleTxId")
                        {
                            Console.WriteLine("Phonepe[] transection id :" + b.Get(key).ToString());
                        }
                        if (key == "txnid")
                        {
                            TrnxacsnId = b.Get(key).ToString();
                        }
                    }
                }
            }
            Finish();
        }

    }
}