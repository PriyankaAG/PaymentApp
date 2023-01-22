using PaymentApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace PaymentApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerView : ContentPage
    {
        private ZXingScannerView zxing;
        private ZXingDefaultOverlay overlay;

        public Action<object, ScannerViewResult> ScanCapturedCallback;

        public ScannerView()
        {
            InitializeComponent();

            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Options = new ZXing.Mobile.MobileBarcodeScanningOptions
                {
                    UseNativeScanning = false,
                    DisableAutofocus = false,
                    UseFrontCameraIfAvailable = false
                }
            };

            overlay = new ZXingDefaultOverlay
            {
                TopText = "Scan",
                BottomText = "bottom scan",
                ShowFlashButton = zxing.HasTorch,
            };

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);

#if DEBUG
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            var height = (displayInfo.Orientation == DisplayOrientation.Landscape) ? displayInfo.Width : displayInfo.Height;
            grid.RowDefinitions.Add(new RowDefinition() { Height = height / 4 });
            var button = new Button() { Text = "Manually Enter Scan Code" };
            button.Clicked += async (s, e) =>
            {
                var barCodeText = await this.DisplayPromptAsync("Scan Process Debugging", "Enter barcode text:");
                if (!string.IsNullOrEmpty(barCodeText))
                {
                    this.ScanCapturedCallback?.Invoke(s, new ScannerViewResult(barCodeText));
                    await Navigation.PopAsync(true);
                }
            };

            grid.Children.Add(button, 0, 1);
#endif

            Content = grid;

            Debuglog(() => "Constructor");
        }

        public bool HasTargetIllumination => zxing.HasTorch;

        public bool IsTargetIlluminated => zxing.IsTorchOn;

        public bool IsScanning => zxing.IsScanning;

        private void OnScanResult(ZXing.Result zxResult)
        {
            zxing.IsAnalyzing = false;
            Device.BeginInvokeOnMainThread(async () =>
            {
                var barCode = zxResult.Text.ToUpper();

                this.ScanCapturedCallback?.Invoke(this, new ScannerViewResult(barCode));

                var isOnTop = Navigation.NavigationStack.LastOrDefault() as ScannerView;
                if (isOnTop != null)
                {
                    Debuglog(() => "Pop Page");
                    await Navigation.PopAsync(true);
                }
                else
                {
                    Debuglog(() => "Remove Page");
                    Navigation.RemovePage(this);
                }
            });
        }

        private void OnFlashButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                ToggleTargetIllumination();
            }
        }

        protected override void OnAppearing()
        {
            Debuglog(null);

            base.OnAppearing();

            // Prevents multiple assignments
            zxing.OnScanResult -= OnScanResult;
            overlay.FlashButtonClicked -= OnFlashButtonClicked;

            zxing.OnScanResult += OnScanResult;
            overlay.FlashButtonClicked += OnFlashButtonClicked;
            zxing.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            Debuglog(null);

            // Emulator debugging 
            //OnScanResult(new ZXing.Result("http://kennards.com.au/wp=153053", null, null, ZXing.BarcodeFormat.QR_CODE));
            //OnScanResult(new ZXing.Result("http://www.kennards.com.au/wp=B210118", null, null, ZXing.BarcodeFormat.QR_CODE));
            // 
            // or
            //
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    var barCodeText = await this.DisplayPromptAsync("Scan Process Debugging", "Enter barcode text:");
            //    if (!string.IsNullOrEmpty(barCodeText))
            //    {
            //        //OnScanResult(new ZXing.Result(barCodeText, null, null, ZXing.BarcodeFormat.QR_CODE));
            //        OnScanCaptured(this, new ScanCapturedEventArgs(barCodeText));
            //    }
            //});

            zxing.OnScanResult -= OnScanResult;
            overlay.FlashButtonClicked -= OnFlashButtonClicked;
            base.OnDisappearing();
            zxing.IsScanning = false;
        }

        public void Disable()
        {
            var isOnTop = Navigation.NavigationStack.LastOrDefault() as ScannerView;
            if (isOnTop != null)
            {
                Debuglog(() => "Pop Page");
                Navigation.PopAsync(true);
            }
            else
            {
                Debuglog(() => "Remove Page");
                Navigation.RemovePage(this);
            }
        }

        public void ToggleTargetIllumination()
            => zxing.IsTorchOn = !zxing.IsTorchOn;

        #region Logging

        private static readonly bool _enableLogging = true;
        [Conditional("DEBUG")]
        private void Debuglog(Func<string> messageFunc, [CallerMemberName] string caller = "")
        {
            if (!_enableLogging)
                return;

            var message = messageFunc?.Invoke() ?? "";
            Debug.Print($"{nameof(ScannerView)}.{caller}: {message}");
        }

        #endregion
    }
}