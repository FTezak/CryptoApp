using System.Threading.Tasks;

namespace CryptoAPI.Interfaces
{
    public interface INewsletterSenderService
    {
        Task<bool> sendNewsletters();

        
    }
}
