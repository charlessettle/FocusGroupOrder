using System;
using System.Collections.Generic;
using FocusGroupOrder.ViewModels;
using Xamarin.Forms;

namespace FocusGroupOrder.Views
{
    public partial class EditGroupOrderPage : ContentPage
    {
        public EditGroupOrderPage(Order order)
        {
            InitializeComponent();
            ((UserOrderViewModel)this.BindingContext).OrderId = order.OrderId;
        }

        void CarouselView_CurrentItemChanged(System.Object sender, Xamarin.Forms.CurrentItemChangedEventArgs e)
        {
            ((UserOrderViewModel)this.BindingContext).CurrentItemDisplayed = ((OrderItemViewModel) e.CurrentItem).ItemName;
        }
    }
}

