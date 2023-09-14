using System;
using Api.Focus.Interfaces;
using Api.Focus.Models;
using Api.Focus.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Api.Focus.Services
{
	public class FocusGroupOrderRepository : IFocusGroupOrderRepository
    {
        private FocusDbContext context;
        ITwilioService twilioService;

        public FocusGroupOrderRepository(FocusDbContext dbContext)
		{
            context = dbContext;
            this.twilioService = new TwilioService();
        }

        public async Task<(bool success, string error, int orderId)> CreateNewOrder( NewOrder newOrder)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newOrder.CreatorEmail))
                    throw new Exception("missing user email");

                var usersForThisOrderInDb = await context.Users.Where(z => z.Email == newOrder.CreatorEmail
                || newOrder.otherUsersEmails.Any(e => e == z.Email.ToLower())).ToListAsync();

                //if creator doesn't exist, create.
                if(!usersForThisOrderInDb.Any(z => z.Email == newOrder.CreatorEmail.Trim().ToLower()))
                {
                    await context.Users.AddAsync(new User { Email = newOrder.CreatorEmail.Trim().ToLower() });
                }

                List<User> otherUsersToAddToDb = new List<User>();
                //for all other users, create new users as needed
                foreach(string obj in newOrder.otherUsersEmails)
                {
                    if(!string.IsNullOrWhiteSpace(obj) && !usersForThisOrderInDb.Any(z => z.Email == obj))
                    {
                        otherUsersToAddToDb.Add(new User { Email = obj.Trim().ToLower() });
                    }
                }
                if(otherUsersToAddToDb.Count > 0)
                {
                    await context.Users.AddRangeAsync(otherUsersToAddToDb);
                }
                await context.SaveChangesAsync();
                //create the order
                Order myOrder = new Order { CreatorId = (await context.Users.FirstAsync(z => z.Email == newOrder.CreatorEmail.Trim().ToLower())).UserId  };
                await context.Orders.AddAsync(myOrder);
                await context.SaveChangesAsync();

                //this could be optimized, but for time's sake, just get all users to retrieve a subset of user ids
                var usersInDb = await context.Users.Where(z => z.Email == newOrder.CreatorEmail
                || newOrder.otherUsersEmails.Any(e => e == z.Email.ToLower())).ToListAsync();

                //add user orders for each user
                foreach (User user in usersInDb)
                {
                    await context.UserOrders.AddAsync(new UserOrder { OrderId = myOrder.OrderId, UserID = user.UserId });
                }

                await context.SaveChangesAsync();

                //send all users twilio emails, letting them know it is time to place their order!!
                if (usersInDb.Where(z => z.Email != newOrder.CreatorEmail.Trim().ToLower()).ToList().Count > 0)
                {
                    foreach (User obj in usersInDb.Where(z => z.Email != newOrder.CreatorEmail.Trim().ToLower()).ToList())
                    {
                        await twilioService.SendEmailMessage(obj.Email);
                    }
                }
                
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return (true, null, myOrder.OrderId);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            }
            catch(Exception ex)
            {
                return (false, ex.Message, -1);
            }
        }

        public async Task<(bool success, string error)> EditOrderForUser(EditOrder order)
        {
            try
            {
                //get user via emailu
                var myUser = await context.Users.FirstOrDefaultAsync(z => z.Email == order.Email);

                if (myUser == null)
                    throw new Exception("user does not exist");

                //get the uncompleted userOrder first
                var userOrder = await context.UserOrders.FirstOrDefaultAsync(z => z.OrderId == order.OrderId && z.UserID == myUser.UserId);

                userOrder.IsComplete = true;
                userOrder.DateModified = DateTime.UtcNow;

                if (order.LineItemsPerUser.Count > 0)
                {
                    List<LineItemPerUser> lineItems = new List<LineItemPerUser>();

                    foreach(LineItem obj in order.LineItemsPerUser)
                    {
                        lineItems.Add(new LineItemPerUser { ProductId = obj.ProductId, UserOrderId = userOrder.UserOrderId, Quantity = obj.Quantity });
                    }
                    userOrder.LineItemsPerUser = new List<LineItemPerUser>();
                    userOrder.LineItemsPerUser.AddRange(lineItems);
                }

                context.UserOrders.Update(userOrder);

                await context.SaveChangesAsync();

                #region if all userorders for this order is complete, notify the creator of the order!
                var updatedOrders = await context.UserOrders.Include(z => z.User).Where(z => z.OrderId == order.OrderId).ToListAsync();

                if (updatedOrders.Count > 0 && !updatedOrders.Any(z => z.IsComplete == false || z.IsComplete == null))
                {
                    await twilioService.SendEmailMessage(updatedOrders.FirstOrDefault(z => z.OrderId == order.OrderId).User.Email, msg: "your order is now complete!");
                }

                #endregion  

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return (true, null);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<User> GetAllOrdersForUser(string email)
        {
            var myUser = await context.Users.Include(z => z.UserOrders).ThenInclude(z => z.Order)
                .Include(z => z.UserOrders).ThenInclude(z => z.LineItemsPerUser).FirstOrDefaultAsync(z => z.Email == email.Trim().ToLower()) ?? new User();

            var otherUsersOnOrders = await context.UserOrders.Include(z => z.LineItemsPerUser).Include(z => z.User) //todo: optimize call
                .Where(z => z.UserID != myUser.UserId).ToListAsync();

            if(otherUsersOnOrders != null && otherUsersOnOrders.Count > 0 && myUser.UserOrders.Count > 0)
                myUser.UserOrders.AddRange(otherUsersOnOrders.Where(z => myUser.UserOrders.Select(e => e.OrderId).ToList().Any( a => a == z.OrderId)));

            return myUser;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await context.Products.ToListAsync();
        }
    }
}