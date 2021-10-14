using System.Threading.Tasks;
using CryptoAPI.Models;

namespace CryptoAPI.Interfaces
{
    public interface IMailService
    {
        //Task SendEmailAsync(string to);
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
