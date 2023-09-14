using System;
using Xamarin.Forms;

namespace FocusGroupOrder.ViewModels
{
    public class GroupOrderUserViewModel : BaseViewModel
    {
        public GroupOrderUserViewModel() { }

        #region properties
        bool _isCreatorOfGroup;
        public bool IsCreatorOfGroup
        {
            get { return _isCreatorOfGroup; }
            set { SetProperty(ref _isCreatorOfGroup, value); }
        }

        string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        //todo: future feature
        //string _friendlyName;
        //public string FriendlyName
        //{
        //    get { return _friendlyName; }
        //    set { SetProperty(ref _friendlyName, value); }
        //}

        Color _textColor = Color.Black;
        public Color TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
        }
        #endregion
    }
}