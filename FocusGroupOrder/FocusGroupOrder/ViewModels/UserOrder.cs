using System;
using System.Collections.Generic;

namespace FocusGroupOrder.ViewModels
{
    public class UserOrder 
    {
        public UserOrder() { }

        public int UserOrderId { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public string Email { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public bool? IsComplete { get; set; }

        public List<LineItemPerUser> LineItemsPerUser { get; set; }
    }
}