using System.ComponentModel;
using Xamarin.Forms;
using XTricks.Controls.Sample.ViewModels;

namespace XTricks.Controls.Sample.Views
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