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
    [Activity(Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class GooglePayActivity : Activity
    {
        public static string pn { get; set; }
        public static string pa { get; set; }
        public static string amount { get; set; }
        public static string googlePayURI { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                long tsLong = JavaSystem.CurrentTimeMillis() / 1000;
                string transaction_ref_id ="999999999999";
                string transaction_ref = "999999999999";
                //string transaction_ref_id = tsLong.ToString() + "UPI";
                //googlePayURI = "upi://pay?pa=priya.gaikwad74@okhdfcbank&pn=Priya%20Gaikwad&aid=uGICAgIDVjo_qPQ";
                //var uri = Android.Net.Uri.Parse(googlePayURI);
                using (var uri = new Android.Net.Uri.Builder()
                                                .Scheme("upi")
                                                .Authority("pay")
                                                .AppendQueryParameter("pa", pa)
                                                .AppendQueryParameter("pn", pn)
                                                //.AppendQueryParameter("pn", pn)
                                                .AppendQueryParameter("mc", "0000")
                                                .AppendQueryParameter("tid", transaction_ref)
                                                .AppendQueryParameter("tr", transaction_ref_id)
                                                .AppendQueryParameter("tn", "Test integration note")
                                                .AppendQueryParameter("am", amount)
                                                .AppendQueryParameter("cu", "INR")
                                                .Build())
                {
                    Intent = new Intent(Intent.ActionView);
                    Intent.SetData(uri);
                    if (IsAppInstalled("com.google.android.apps.nbu.paisa.user"))
                    {
                        Intent.SetPackage("com.google.android.apps.nbu.paisa.user");
                        StartActivityForResult(Intent, 9999);
                    }
                    else
                    {
                        var package = PackageName;
                        ShowToast("Google pay is not available in this device");
                        this.Finish();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ShowToast("Payment through Google Pay failed");
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
                    Android.Util.Log.Debug("Google pay result", data.GetStringExtra("Status"));
                    if (data.GetStringExtra("Status").Contains("SUCCESS"))
                    {
                        ShowToast("Payment through Google Pay success"); //! Success
                                                                         //  SetSetting("SUCCESS");
                    }
                    else
                    {
                        ShowToast("Payment through Google Pay failed"); //! Fail
                                                                        //  SetSetting("FAILURE");
                    }
                }
                var b = data?.Extras;
                var d = new Dictionary<string, string>();
                if (b != null)
                {
                    foreach (var key in b.KeySet())
                    {
                        d.Add(key, b.Get(key).ToString());
                        if (key == "Status" && b.Get(key).ToString().Contains("FAILURE"))
                        {
                            ShowToast("Payment through Google pay fail"); //! If failed
                        }
                        if (key == "Status" && b.Get(key).ToString().Contains("SUCCESS"))
                        {
                            ShowToast("Payment through Google pay success"); //! If success
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Exception while google payment :" + ex.Message);
                ShowToast("Payment through Google pay failed");  //! If any exception occur 
            }
            this.Finish();
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
        public override void OnBackPressed()
        {

        }
    }
}