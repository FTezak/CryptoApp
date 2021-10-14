using System.ComponentModel.DataAnnotations;

namespace CryptoAPI.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string oldPassword { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string password { get; set; }

        [Required]
        public string email { get; set; }
    }
}
