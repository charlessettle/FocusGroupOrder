using System;
using Api.Focus.Models;
using Api.Focus.ViewModels;

namespace Api.Focus.Interfaces
{
	public interface IFocusGroupOrderRepository
	{
        /// <summary>
        /// gets all orders for a current user/email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <remarks>will create user if the user does not exists</remarks>
        Task<User> GetAllOrdersForUser(string email);

        /// <summary>
        /// create new order
        /// </summary>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        /// <remarks>notifications to other users who were invited to this order (not the creator of the order)</remarks>
        Task<(bool success, string error, int orderId)> CreateNewOrder(NewOrder newOrder);

        /// <summary>
        /// user adds line items for their part of a group order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <remarks>if all users have added their items, then the order is complete, notify the creator of the order</remarks>
        Task<(bool success, string error)> EditOrderForUser(EditOrder order);

        Task<List<Product>> GetProducts();
    }
}