using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FocusGroupOrder.Services
{
    public class BaseService
    {
        public static readonly HttpClient client = new HttpClient { Timeout = new TimeSpan(0, 0, 18) };

        public static async Task<HttpResponseMessage> PostAsync(string uri, StringContent content, int retry = 0)
        {
            try
            {
                return await client.PostAsync(uri, content);
            }
            catch (HttpRequestException e)
            {
                if (retry <= 2)
                {
                    return await PostAsync(uri, content, ++retry);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"error message: {e.Message}, exception of type {e.GetType()}");
                    throw e;
                }
            }
            catch (TimeoutException e)
            {
                if (retry <= 2)
                {
                    return await PostAsync(uri, content, ++retry);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"error message: {e.Message}, exception of type {e.GetType()}");
                    throw e;
                }
            }
            catch (Exception e)
            {
                if (retry <= 2)
                {
                    return await PostAsync(uri, content, ++retry);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"error message: {e.Message}, exception of type {e.GetType()}");
                    throw e;
                }
            }
        }

        public static async Task<HttpResponseMessage> GetAsync(string uri, int retry = 0)
        {
            try
            {
                return await client.GetAsync(uri);
            }
            catch (HttpRequestException e)
            {
                if (retry <= 2)
                {
                    return await GetAsync(uri, ++retry);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"error message: {e.Message}, exception of type {e.GetType()}");
                    throw e;
                }
            }
            catch (TimeoutException e)
            {
                if (retry <= 2)
                {
                    return await GetAsync(uri, ++retry);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"error message: {e.Message}, exception of type {e.GetType()}");
                    throw e;
                }
            }
            catch (Exception e)
            {
                if (retry <= 2)
                {
                    return await GetAsync(uri, ++retry);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"error message: {e.Message}, exception of type {e.GetType()}");
                    throw e;
                }
            }
        }
    }
}