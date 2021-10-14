using System.Threading.Tasks;
using CryptoAPI.Entities;

namespace CryptoAPI.Interfaces
{
    public interface ITemplateRepository
    {
        Task<Templates> GetTemplates();
        
    }
}
