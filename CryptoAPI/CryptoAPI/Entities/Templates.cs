using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoAPI.Entities
{
    public class Templates
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string MailTemplate { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string FavoriteDataTemplate { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string FavoriteTemplate { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string WalletDataTemplate { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string WalletTemplate { get; set; }

    }
}
   
