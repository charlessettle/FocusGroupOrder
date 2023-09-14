using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using FocusGroupOrder.Models;
using FocusGroupOrder.Services;
using System.Threading.Tasks;
using FocusGroupOrder.Interfaces;

namespace FocusGroupOrder.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        IFocusGroupOrderWebService _webService;
        IFocusGroupOrderWebService webService
        {
            get
            {
                if (_webService == null)
                    _webService = new FocusGroupOrderWebService(); //todo: could use some other Dependency injection techniques instead of singleton pattern
                return _webService;
            }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        #region RESTful web methods
        public async Task<List<Product>> GetProducts()
        {
            try
            {
                IsBusy = true;
                Products = await webService.GetProducts();
                return Products;
            }
            catch { }
            finally { IsBusy = false; }
            return null;
        }

        public static List<Product> Products;
        public static User CurrentUser;

        public async Task<User> GetAllOrdersForUser(string email)
        {
            try
            {
                IsBusy = true;
                CurrentUser = await webService.GetAllOrdersForUser(email);
                if (CurrentUser == null || string.IsNullOrWhiteSpace(CurrentUser.Email))
                    CurrentUser = new User { Email = email.Trim().ToLower() };
                return CurrentUser;
            }
            catch { }
            finally { IsBusy = false; }
            return null;
        }

        public async Task<(bool success, string error, int orderId)> CreateNewOrder(NewOrder order)
        {
            try
            {
                IsBusy = true;
                return await webService.CreateNewOrder(order);
            }
            catch { }
            finally { IsBusy = false; }
            return (false, null, -1);
        }

        public async Task<(bool success, string error)> EditOrderForUser(EditOrder order)
        {
            try
            {
                IsBusy = true;
                return await webService.EditOrderForUser(order);
            }
            catch { }
            finally { IsBusy = false; }
            return (false, null);
        }
        #endregion
    }
}

