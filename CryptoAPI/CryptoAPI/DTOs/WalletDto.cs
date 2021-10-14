using System.Collections.Generic;

namespace CryptoAPI.DTOs
{
    public class WalletDto
    {
        public bool HasBinance { get; set; }
        public List<CryptosWallet> Cryptos { get; set; }
        public decimal totalValue { get; set; }
    }

    public class CryptosWallet
    {
        public decimal amount { get; set; }
        public decimal binanceAmount { get; set; }
        public int CryptoApiReference { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
