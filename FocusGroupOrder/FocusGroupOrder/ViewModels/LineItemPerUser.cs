using System;
namespace FocusGroupOrder.ViewModels
{
    public class LineItemPerUser 
    {
        public LineItemPerUser() { }

        public int LineItemPerUserId { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int UserOrderId { get; set; }

        public UserOrder UserOrder { get; set; }
    }
}