using PaymentApp.Droid.Services;
using PaymentApp.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;

[assembly: Dependency(typeof(QrScanningService))]
namespace PaymentApp.Droid.Services
{
    public class QrScanningService : IQRScanningService
    {
        public async Task<string> ScanAsync()
        {
            //var optionsDefault = new MobileBarcodeScanningOptions();
            //var optionsCustom = new MobileBarcodeScanningOptions();

            //var scanner = new ()
            //{
            //    TopText = "Scan the QR Code",
            //    BottomText = "Please Wait",
            //};

            //var scanResult = await scanner.Scan(optionsCustom);
            //return scanResult.Text;
            return "";
        }
    }
}