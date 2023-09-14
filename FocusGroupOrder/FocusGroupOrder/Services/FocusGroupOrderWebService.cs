using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FocusGroupOrder.Interfaces;
using FocusGroupOrder.ViewModels;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FocusGroupOrder.Services
{
    public class FocusGroupOrderWebService : BaseService, IFocusGroupOrderWebService
    {
        const string uriAzureBase = "https://api-focuscodingchallenge.azurewebsites.net/"; 
        public FocusGroupOrderWebService() { }

        public async Task<User> GetAllOrdersForUser(string email)
        {
            string uri = $"{uriAzureBase}GetUser";
            try
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("applicaton/json"));
                string args = JsonConvert.SerializeObject(new
                {
                   email = email
                });
                var content = new StringContent(args);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await PostAsync(uri, content); //await client.PostAsync(uri, content);

                string rawValue = await response.Content.ReadAsStringAsync();
                var bizObj = JsonConvert.DeserializeObject<User>(rawValue);
                return bizObj;
            }
            catch (HttpRequestException e)
            {
                //TODO: LOG TO APPCENTER, SEND EMAIL,ETC, ETC
                System.Diagnostics.Debug.WriteLine($"error message: {e.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"error message: {ex.Message}");
            }
            return null;
        }

        public async Task<(bool success, string error, int orderId)> CreateNewOrder(NewOrder newOrder)
        {
            string uri = $"{uriAzureBase}CreateOrder";
            try
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("applicaton/json"));
                string args = JsonConvert.SerializeObject(new
                {
                    creatorEmail = newOrder.CreatorEmail.Trim().ToLower(),
                    otherUsersEmails = newOrder.otherUsersEmails
                });
                var content = new StringContent(args);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await PostAsync(uri, content); //await client.PostAsync(uri, content);

                string rawValue = await response.Content.ReadAsStringAsync();
                //var results = JsonConvert.DeserializeObject<(bool success, string error, int orderId)>(rawValue);
                //return results;
                return (rawValue.ToLower().Contains("true"), null, Convert.ToInt32(rawValue.Substring(rawValue.LastIndexOf(":") + 1).Replace("}", "")));
            }
            catch (HttpRequestException e)
            {
                //TODO: LOG TO APPCENTER, SEND EMAIL,ETC, ETC
                System.Diagnostics.Debug.WriteLine($"error message: {e.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"error message: {ex.Message}");
            }
            return (false, null, -1);
        }

        public async Task<(bool success, string error)> EditOrderForUser(EditOrder order)
        {
            string uri = $"{uriAzureBase}EditOrder";
            try
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("applicaton/json"));
                string args = JsonConvert.SerializeObject(new
                {
                    orderId = order.OrderId,
                    email = order.Email,
                    lineItemsPerUser = order.LineItemsPerUser
                });
                var content = new StringContent(args);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await PostAsync(uri, content); //await client.PostAsync(uri, content);

                string rawValue = await response.Content.ReadAsStringAsync();
                //var bizObj = JsonConvert.DeserializeObject<(bool success, string error)>(rawValue);
                return (rawValue.ToLower().Contains("true"), null);
            }
            catch (HttpRequestException e)
            {
                //TODO: LOG TO APPCENTER, SEND EMAIL,ETC, ETC
                System.Diagnostics.Debug.WriteLine($"error message: {e.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"error message: {ex.Message}");
            }
            return (false, null);
        }

        public async Task<List<Product>> GetProducts()
        {
            string uri = $"{uriAzureBase}getproducts";
            try
            {
                HttpResponseMessage response = await GetAsync(uri);
                response.Headers.Add("Cache-Control", "no-cache");

                string rawValue = await response.Content.ReadAsStringAsync();
                var bizObj = JsonConvert.DeserializeObject<List<Product>>(rawValue);
                return bizObj;
            }
            catch (HttpRequestException e)
            {
                //TODO: LOG TO APPCENTER, SEND EMAIL,ETC, ETC
                System.Diagnostics.Debug.WriteLine($"error message: {e.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"error message: {ex.Message}");
            }
            return null;
        }
    }
}

