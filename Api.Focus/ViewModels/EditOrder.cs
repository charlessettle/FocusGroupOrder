using System;
using System.ComponentModel.DataAnnotations;
using Api.Focus.Models;

namespace Api.Focus.ViewModels
{
	public class EditOrder
	{
		public EditOrder() { }

		public int OrderId { get; set; }

		/// <summary>
		/// email for user who is editing their part of the group order
		/// </summary>
		public string Email { get; set; }

		public List<LineItem> LineItemsPerUser { get; set; } = new List<LineItem>();

		public bool IsComplete { get; set; }

    }

    public class LineItem
    {
        public LineItem() { }

        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}