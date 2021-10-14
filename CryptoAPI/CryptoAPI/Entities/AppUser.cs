using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CryptoAPI.Entities
{
    public class AppUser : IdentityUser<int>
    {


        public ICollection<AppUserRole> UserRoles { get; set; }

        public ICollection<CryptoCurrency> FavoriteCrypto { get; set; }

        public ICollection<CryptoWallet> CryptoWallet { get; set; }

        public string BinanceApiKey { get; set; }
        public string BinanceSecretApiKey { get; set; }
        public Newsletter Newsletter { get; set; }
    }
}
