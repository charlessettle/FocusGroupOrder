using System;
using System.Threading.Tasks;

namespace Api.Focus.Interfaces
{
    public interface ITwilioService
    {
        Task<(string error, bool success)> SendEmailMessage(string email, int retry = 0, string msg = null);
    }
}