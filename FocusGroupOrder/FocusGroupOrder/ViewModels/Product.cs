using System;
namespace FocusGroupOrder.ViewModels
{
    public class Product
    {
        public Product() { }

        public int ProductId { get; set; }

        public decimal Price { get; set; }
        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}

