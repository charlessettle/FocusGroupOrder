using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit.ObjectModel;

namespace FocusGroupOrder.ViewModels
{
    public class UserOrderViewModel : BaseViewModel
    {
        public UserOrderViewModel()
        {
            List<OrderItemViewModel> presetItems = new List<OrderItemViewModel>();
            //1. Show 5 menu items ( Quesadilla $5, Tacos $6, Nachos $10, Coca Cola $2 and Flan $5)
            presetItems.Add(new OrderItemViewModel { ItemName = "Quesadilla", Price = 5 });
            presetItems.Add(new OrderItemViewModel { ItemName = "Tacos", Price = 6 });
            presetItems.Add(new OrderItemViewModel { ItemName = "Nachos", Price = 10 });
            presetItems.Add(new OrderItemViewModel { ItemName = "Coca Cola ", Price = 2 });
            presetItems.Add(new OrderItemViewModel { ItemName = "Flan", Price = 5 });
            Items.AddRange(presetItems);
        }

        public ObservableRangeCollection<OrderItemViewModel> Items { get; set; } = new ObservableRangeCollection<OrderItemViewModel>();
        public int Position { get; set; }
        public OrderItemViewModel CurrentItem { get; private set; }

        /// <summary>
        /// user for this part of the order
        /// </summary>
        public GroupOrderUserViewModel MyUser { get; set; } = new GroupOrderUserViewModel();

        int _howMany = 1;
        public int HowMany
        {
            get { return _howMany; }
            set
            {
                SetProperty(ref _howMany, value);
                BtnAddText = $"Add {HowMany} {CurrentItemDisplayed}";
            }
        }

        string _btnAddText = $"Add 1 Quesadilla";
        public string BtnAddText
        {
            get { return _btnAddText; }
            set
            {
                SetProperty(ref _btnAddText, value);
            }
        }

        string _currentItemDisplayed = $"Quesadilla";
        public string CurrentItemDisplayed
        {
            get { return _currentItemDisplayed; }
            set
            {
                SetProperty(ref _currentItemDisplayed, value);

            }
        }
    }
}