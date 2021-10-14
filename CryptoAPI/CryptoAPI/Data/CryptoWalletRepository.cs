using System.Threading.Tasks;
using CryptoAPI.Interfaces;

namespace CryptoAPI.Data
{
    public class CryptoWalletRepository : ICryptoWalletRepository
    {
        private readonly DataContext _context;



        public CryptoWalletRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> SaveAllAsyncint()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
