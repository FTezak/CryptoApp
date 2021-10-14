namespace CryptoAPI.Entities
{
    public class Newsletter
    {
        public int Id { get; set; }
        public int userId { get; set; }
        //public AppUser user { get; set; }
        public int frequency { get; set; }
        public bool walletData { get; set; }
        public bool favoriteData { get; set; }
    }
}
   
