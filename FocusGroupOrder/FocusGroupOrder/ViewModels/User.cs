using System;
using System.Collections.Generic;

namespace FocusGroupOrder.ViewModels
{
    public class User
    {
        public User() { }

        public int UserId { get; set; }

        public string Email { get; set; }

        public List<UserOrder> Orders { get; set; } = new List<UserOrder>();
    }
}