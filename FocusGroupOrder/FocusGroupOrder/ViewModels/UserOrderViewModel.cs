using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FocusGroupOrder.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace FocusGroupOrder.ViewModels
{
    public class UserOrderViewModel : BaseViewModel
    {
        public UserOrderViewModel()
        {
            List<OrderItemViewModel> presetItems = new List<OrderItemViewModel>();
            CommandFinishOrder = CommandFactory.Create(FinishOrder);
            CommandAddItemToOrder = CommandFactory.Create(AddItemToOrder);

            foreach (Product obj in Products)
            {
                presetItems.Add(new OrderItemViewModel { ItemName = obj.Description, Price = Convert.ToDouble(obj.Price), ImageUrl = obj.ImageUrl });
            }
            CurrentItemDisplayed = presetItems[0].ItemName; //the default selection

            Items.AddRange(presetItems);
        }

        public ObservableRangeCollection<OrderItemViewModel> Items { get; set; } = new ObservableRangeCollection<OrderItemViewModel>();
        public int Position { get; set; }

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

        int _orderId = 1;
        public int OrderId
        {
            get { return _orderId; }
            set
            {
                SetProperty(ref _orderId, value);
            }
        }

        //string _CurrentProductDisplay;
        //public string CurrentProductDisplay
        //{
        //    get { return _CurrentProductDisplay; }
        //    set
        //    {
        //        SetProperty(ref _CurrentProductDisplay, value);
        //        //BtnAddText = $"Add {HowMany} {CurrentProductDisplay}";
        //    }
        //}

        string _orderSummary;
        public string OrderSummary
        {
            get { return _orderSummary; }
            set
            {
                SetProperty(ref _orderSummary, value);
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
                BtnAddText = $"Add {HowMany} {CurrentItemDisplayed}";
            }
        }

        public IAsyncCommand CommandFinishOrder { private set; get; }
        public ICommand CommandAddItemToOrder { private set; get; }

        async Task FinishOrder()
        {
            try
            {
                var answer = await App.Current.MainPage.DisplayActionSheet("are you sure", "Cancel", null, new List<string> {"YES", "NO" }.ToArray());
                if (answer != "YES")
                    return;

                IsBusy = true;
                if(MyLineItems.Count == 0)
                {
                    await App.Current.MainPage.DisplayAlert("error", "you must add something to your order. please try again.", "OK");
                    return;
                }
                List<LineItem> culturedLineItems = new List<LineItem>();
                foreach(LineItem obj in MyLineItems)
                {
                    if(!culturedLineItems.Any(z => z.ProductId == obj.ProductId))
                    {
                        culturedLineItems.Add(new LineItem { ProductId = obj.ProductId, Quantity = obj.Quantity });
                    }
                    else
                    {
                        //update the quanity
                        culturedLineItems.First(z => z.ProductId == obj.ProductId).Quantity = culturedLineItems.First(z => z.ProductId == obj.ProductId).Quantity + obj.Quantity;
                    }
                }

                var success = await EditOrderForUser(new EditOrder
                {
                    OrderId = OrderId,
                    Email = CurrentUser.Email,
                    LineItemsPerUser = culturedLineItems,
                    IsComplete = true
                });

                if (success.success)
                {
                    await App.Current.MainPage.DisplayAlert("you have completed your order", "You're Done! but You may have to wait for others to complete this group order too", "OK");
                    Application.Current.MainPage = new AppShell(); ;
                }
                else
                    await App.Current.MainPage.DisplayAlert("error", "an error occurred on the server. please try again.", "OK");
            }
            catch { } //todo: appcenter tracking on errors
            finally
            {
                IsBusy = false;
            }
        }

        void AddItemToOrder()
        {
            var theProduct = Products.First(z => z.Description == CurrentItemDisplayed);
            //store in-memory snapshot of order for user
            if (MyLineItems.Count(z => theProduct.ProductId == z.ProductId) <= 5)
            {
                MyLineItems.Add(new LineItem { ProductId = Products.First(z => z.Description == CurrentItemDisplayed).ProductId, Quantity = HowMany });

                //edit the order summary
                OrderSummary += $"{HowMany} {CurrentItemDisplayed} @ {Products.First(z => z.Description == CurrentItemDisplayed).Price.ToString("c")} each,";
            }
        }

        List<LineItem> MyLineItems = new List<LineItem>();
    }
}