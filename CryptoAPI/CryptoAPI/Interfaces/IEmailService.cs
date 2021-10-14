using System.Threading.Tasks;
using CryptoAPI.Models;

namespace CryptoAPI.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendEmail(EmailData emailData);
    }
}
