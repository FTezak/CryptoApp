namespace CryptoAPI.Entities
{
    public class CryptoWallet
    {
        public int Id { get; set; }
        public AppUser User { get; set; }
        public int UserId { get; set; }
        public CryptoCurrency Crypto { get; set; }
        public int CryptoId { get; set; }
        public decimal Amount { get; set; }
    }
}
