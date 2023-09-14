using System;
namespace FocusGroupOrder.ViewModels
{
    public class OrderItemViewModel : BaseViewModel
    {
        public OrderItemViewModel()
        {
        }

        string _itemName;
        /// <summary>
        /// name of food item in an order
        /// </summary>
        public string ItemName
        {
            get { return _itemName; }
            set { SetProperty(ref _itemName, value); }
        }

        string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { SetProperty(ref _imageUrl, value); }
        }

        int _howMany = 1;
        /// <summary>
        /// number of items
        /// </summary>
        public int HowMany
        {
            get { return _howMany; }
            set { SetProperty(ref _howMany, value); }
        }

        double _price;
        public double Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        public string PriceDisplay => Price.ToString("c");
    }
}