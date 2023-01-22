using System.Threading.Tasks;
namespace PaymentApp.Services
{
    public interface IQRScanningService
    {
        Task<string> ScanAsync();
    }
}
