using System.Threading.Tasks;
using CryptoAPI.Entities;

namespace CryptoAPI.Interfaces
{
    public interface ICryptoLikeRepository
    {

        Task<bool> SaveAllAsync();
        Task<bool> IsCryptoLikedByUser(string cryptoSym);
        void AddCryptoToFavoriteForUser(CryptoCurrency crypto, AppUser user);
        void RemoveCryptoFromFavoriteForUser(CryptoCurrency crypto, AppUser user);
    }
}
