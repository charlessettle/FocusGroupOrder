using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FocusGroupOrder.Views;
using Xamarin.CommunityToolkit.ObjectModel;

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

        public ObservableRangeCollection<GroupOrderUserViewModel> Users { get; set; } = new ObservableRangeCollection<GroupOrderUserViewModel>();

        public IAsyncCommand CommandStartOrder { private set; get; }
        public ICommand CommandAddMyself { private set; get; }
        public ICommand CommandAddOtherUser { private set; get; }

        public CreateOrderViewModel()
        {
            CommandStartOrder = CommandFactory.Create(async () => await StartOrder());
            CommandAddMyself = CommandFactory.Create(() => AddMyself());
            CommandAddOtherUser = CommandFactory.Create(() => AddOtherUser());

            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });

            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
            //Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
        }

        public void LoadUsers()
        {
            //todo: auto-load current user email! from secure storage
            try
            {
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
                Users.Add(new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = "support@beskush.com" });
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
                await App.Current.MainPage.Navigation.PushAsync(new EditGroupOrderPage());
                //await ((AppShell)Application.Current.MainPage).GoToAsync("..");
            }
            catch { } //todo: appcenter tracking on errors
            finally
            {
                IsBusy = false;
            }
        }

        void AddMyself()
        {
            try
            {
                IsBusy = true;
                //todo: validate
                Users.Insert(0, new GroupOrderUserViewModel { IsCreatorOfGroup = true, Email = CreatorEmail, TextColor = Xamarin.Forms.Color.Purple });
            }
            catch { } //todo: appcenter tracking on errors
            finally
            {
                IsBusy = false;
            }
        }

        void AddOtherUser()
        {
            try
            {
                IsBusy = true;
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