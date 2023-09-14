using System.ComponentModel;
using Xamarin.Forms;
using FocusGroupOrder.ViewModels;

namespace FocusGroupOrder.Views
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
