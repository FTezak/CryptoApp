using System;
using System.Threading.Tasks;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;

namespace CryptoAPI.Data
{
    public class CryptoLikeRepository : ICryptoLikeRepository
    {
        private readonly DataContext _context;



        public CryptoLikeRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsCryptoLikedByUser(string cryptoSym)
        {
 
            return true;
        }

        public void AddCryptoToFavoriteForUser(CryptoCurrency crypto, AppUser user)
        {
            throw new NotImplementedException();
        }

        public void RemoveCryptoFromFavoriteForUser(CryptoCurrency crypto, AppUser user)
        {
            throw new NotImplementedException();
        }

    }
}
