using System.ComponentModel.DataAnnotations;

namespace CryptoAPI.DTOs
{
    public class CryptoWalletDto
    {
        [Required]
        public string cryptoSym { get; set; }

        [Required]
        public decimal amount { get; set; }

    }
}
