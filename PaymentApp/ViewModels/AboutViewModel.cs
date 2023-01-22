using PaymentApp.Services;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace PaymentApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        private string _amount;
        public string Amount
        {
            get => _amount;
            set
            {
                SetProperty(ref _amount, value);
            }
        }
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            // await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            try
            {
                //ScannerView scanView = new ScannerView();
                //await Shell.Current.GoToAsync(nameof(ScannerView));
                // await Shell.Current.GoToAsync(nameof(CustomScanPage));
                //var customScanPage = new CustomScanPage();
                //var scanner = DependencyService.Get<IQRScanningService>();
                //var result = await scanner.ScanAsync();
                //if (result != null)
                //{
                //    //txtBarcode.Text = result;
                //    IAppPaymentService service = DependencyService.Get<IAppPaymentService>();
                //    service.GooglePay("1");
                //}

                var options = new MobileBarcodeScanningOptions
                {
                    AutoRotate = true,
                    UseFrontCameraIfAvailable = false,
                    TryHarder = true,
                };

                options.PossibleFormats.Clear();
                options.PossibleFormats.Add(ZXing.BarcodeFormat.QR_CODE);

                var overlay = new ZXingDefaultOverlay
                {
                    TopText = "Please scan QR code",
                    BottomText = "Align the QR code within the frame"
                };

                var QRScanner = new ZXingScannerPage(options, overlay) { IsScanning = true, IsAnalyzing = true };

                QRScanner.OnScanResult += async (qrcode) =>
                {
                    // Stop scanning
                    QRScanner.IsAnalyzing = false;
                    QRScanner.IsScanning = false;

                    // Pop the page and show the result
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Shell.Current.CurrentPage.Navigation.PopModalAsync(true);
                    });

                    var paymentUPI = qrcode.Text;

                    if (!string.IsNullOrEmpty(paymentUPI))
                    {
                        var paymentUPIFirstSplit = paymentUPI.Split('&');
                        var paymentUPIFirstSplitpa = paymentUPIFirstSplit[0].Split('=');
                        var paymentUPIFirstSplitpn = paymentUPIFirstSplit[1].Split('=');

                        var pa = paymentUPIFirstSplitpa[1];
                        var pn = paymentUPIFirstSplitpn[1];
                        IAppPaymentService service = DependencyService.Get<IAppPaymentService>();
                        service.GooglePay(pa, pn, Amount);

                    }
                    //if (apiresult.success == true)
                    //{
                    //    Device.BeginInvokeOnMainThread(() =>
                    //    {

                    //    });
                    //}
                    //else
                    //{
                    //    Device.BeginInvokeOnMainThread(async () =>
                    //    {
                    //        await Shell.Current.CurrentPage.DisplayAlert("Error", apiresult.message, "OK");
                    //    });
                    //}

                };

                await Shell.Current.CurrentPage.Navigation.PushModalAsync(QRScanner);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public ICommand OpenWebCommand { get; }
    }
}