using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FocusGroupOrder.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Linq;

namespace FocusGroupOrder.ViewModels
{
    public class CreateOrderViewModel : BaseViewModel
    {
        string _creatorEmail;
        public string CreatorEmail
        {
            get { return _creatorEmail; }
            set { SetProperty(ref _creatorEmail, value); }
        }

        long _orderId;
        public long OrderId
        {
            get { return _orderId; }
            set { SetProperty(ref _orderId, value); }
        }

        string _otherEmail;
        public string OtherEmail
        {
            get { return _otherEmail; }
            set { SetProperty(ref _otherEmail, value); }
        }


        bool _IsUserLoggedIn;
        /// <summary>
        /// will login once user 'Add Me' when creating/editing an order
        /// </summary>
        public bool IsUserLoggedIn
        {
            get { return _IsUserLoggedIn; }
            set { ShowLoginPanel = !value; SetProperty(ref _IsUserLoggedIn, value); }
        }

        bool _ShowLoginPanel = true;
        /// <summary>
        /// will login once user 'Add Me' when creating/editing an order
        /// </summary>
        public bool ShowLoginPanel
        {
            get { return _ShowLoginPanel; }
            set { SetProperty(ref _ShowLoginPanel, value); }
        }

        public ObservableRangeCollection<GroupOrderUserViewModel> Users { get; set; } = new ObservableRangeCollection<GroupOrderUserViewModel>();

        public IAsyncCommand CommandStartOrder { private set; get; }
        public ICommand CommandAddMyself { private set; get; }
        public ICommand CommandAddOtherUser { private set; get; }

        public CreateOrderViewModel()
        {
            CommandStartOrder = CommandFactory.Create(StartOrder);
            CommandAddMyself = CommandFactory.Create(AddMyself);
            CommandAddOtherUser = CommandFactory.Create(AddOtherUser);
        }

        /// <summary>
        /// test method
        /// </summary>
        public void LoadUsers()
        {
            //todo: auto-load current user email! from secure storage
            try
            {
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            }
            catch { }
            finally { }
        }

        async Task StartOrder()
        {
            try
            {
                IsBusy = true;
                var success = await CreateNewOrder(new NewOrder
                {
                    CreatorEmail = CreatorEmail,
                    otherUsersEmails = Users.Count > 0 && Users.Any(z => z.Email.Trim().ToLower() != CreatorEmail.Trim().ToLower()) ?
                    Users.Where(z => z.Email.Trim().ToLower() != CreatorEmail.Trim().ToLower()).Select(z => z.Email).ToList() : new List<string>()
                });
                if(success.success)
                    await App.Current.MainPage.Navigation.PushAsync(new EditGroupOrderPage(new Order { OrderId = success.orderId }));
                else
                    await App.Current.MainPage.DisplayAlert("error", "an error occurred on the server. please try again.", "OK");
            }
            catch { } //todo: appcenter tracking on errors
            finally
            {
                IsBusy = false;
            }
        }

        async Task AddMyself()
        {
            try
            {
                IsBusy = true;
                //todo: stronger validation that this is an email, this will do for a coding challenge demonstration
                if(string.IsNullOrWhiteSpace(CreatorEmail) || !CreatorEmail.Contains("@") || !CreatorEmail.Contains("."))
                {
                    await App.Current.MainPage.DisplayAlert("error", "not a valid email. please try again.", "OK");
                    return;
                }

                Users.Insert(0, new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = CreatorEmail, TextColor = Xamarin.Forms.Color.Purple });

                //get all orders for this user from cloud now!
                var sytemProducts = await GetProducts();

                var myUserSession = await GetAllOrdersForUser(CreatorEmail.Trim().ToLower());

                //todo: if has an open order, then open EDIT order PAGE... or show progress at least
                if (myUserSession != null && myUserSession.Orders.Count > 0 && myUserSession.Orders.Any(Z => Z.IsComplete != true))
                {
                    //todo: fix this limitation
                    //if there are any open orders, whether you created it or not, you can only view/edit your current order
                    await App.Current.MainPage.Navigation.PushAsync(new EditGroupOrderPage(new Order { OrderId = myUserSession.Orders.First(Z => Z.IsComplete != true).OrderId })); 
                }
                else
                {
                    //create a new order, this user is the creator of that order
                    IsUserLoggedIn = true;
                }
            }
            catch { } //todo: appcenter tracking on errors
            finally
            {
                IsBusy = false;
            }
        }

        async Task AddOtherUser()
        {
            try
            {
                IsBusy = true;
                if (string.IsNullOrWhiteSpace(CreatorEmail) || !CreatorEmail.Contains("@") || !CreatorEmail.Contains("."))
                {
                    await App.Current.MainPage.DisplayAlert("error", "not a valid email. please try again.", "OK");
                    return;
                }
                //todo: validate
                Users.Add(new GroupOrderUserViewModel { Email = OtherEmail });
            }
            catch { } //todo: appcenter tracking on errors
            finally
            {
                IsBusy = false;
            }
        }
    }
}