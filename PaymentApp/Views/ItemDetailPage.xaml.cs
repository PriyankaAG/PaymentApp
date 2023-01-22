using PaymentApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PaymentApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}