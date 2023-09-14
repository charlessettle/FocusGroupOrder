using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FocusGroupOrder.Interfaces
{
    public interface IFocusGroupOrderWebService
    {
        //Task<IList<LocalBusiness>> GetDispensaryData(string zipcode);
        //Task<IList<LocalBusiness>> GetDispensaryData(string city, string state);
        Task<IList<LocalBusiness>> GetDispensaryData(double latitude, double longitude);
        Task<LocalBusiness> GetDispensaryDetails(int dispensaryId);

        Task<string> AddDispensaryRating(int dispensaryId, string userEmail, string userName, int rating, string comments);
        Task<string> AddBargain(int dispensaryId, string userEmail, string userName, string bargainItemTitle, string description, double price, DateTime? datePurchased, byte[] bargainImage, int[] tags);
        Task<string> AddBargainLike(int bargainId, string userEmail, string userName, bool like = true);
        Task<HttpStatusCode> AddBargainComment(int bargainId, string userEmail, string userName, string comments);
        //Task<List<Bargain>> GetBargains(int page = 1);
        Task<List<Bargain>> GetBargainsFiltered(string searchString = "", string orderBy = "", List<string> tags = null, int page = 1, double minimumPrice = 0, double maximumPrice = 300, int retryInCaseColdStart = 0);
        Task<List<BargainComment>> GetBargainComments(int bargainId, int page = 1);

        Task<List<BlogCommentViewModel>> GetBlogComments(int blogTagId, string category = null, int page = 1);
        Task<HttpStatusCode> PostBlogComment(int blogTagId, string comment, string userEmail, string userName, string category = null);

        /// <summary>
        /// updates the user's profile pic with an image, avatar, or initials (also avatar)
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="userName"></param>
        /// <param name="profilePic"></param>
        /// <param name="avatar"> if blank, then use the byte array, else it is the initials or avatar view from syncfusion</param>
        /// <returns></returns>
        Task<bool> UpdateProfilePic(string userEmail, string userName, byte[] profilePic, string avatar);

        Task<HttpStatusCode> InvalidateDispensary(int id);
        Task<HttpStatusCode> GiveFeedback(string email, string username, string feedback);
        Task<User> GetUserProfile(string email, string userName);
        Task<HttpStatusCode> AllowEmailUpdates(string userEmail, string userName, bool allowUpdates);

        Task<List<LocalBusinessRating>> GetRatingsByPage(int page = 1, int retryInCaseColdStart = 0);
    }
}