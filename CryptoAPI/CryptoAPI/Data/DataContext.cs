using CryptoAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CryptoAPI.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<CryptoCurrency> CryptoCurrency { get; set; }
        public DbSet<CryptoCurrencyData> CryptoCurrencyData { get; set; }
        public DbSet<CryptoWallet> CryptoWallet { get; set; }
        public DbSet<Newsletter> NewsletterConfig { get; set; }
        public DbSet<Templates> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CryptoCurrencyData>()
                .HasKey(k => k.Id)
                .IsClustered(false);

        
            builder.Entity<CryptoCurrencyData>().Property(p => p.Open).HasPrecision(38, 18);
            builder.Entity<CryptoCurrencyData>().Property(p => p.Close).HasPrecision(38, 18);
            builder.Entity<CryptoCurrency>().Property(p => p.AllTimeHigh).HasPrecision(38, 18);
            builder.Entity<CryptoWallet>().Property(p => p.Amount).HasPrecision(38, 18);

            builder.Entity<AppUser>()
                    .HasMany(ur => ur.UserRoles)
                    .WithOne(u => u.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

            builder.Entity<AppUser>()
                .HasOne(ur => ur.Newsletter)
                .WithOne()
                .HasForeignKey<Newsletter>(n => n.userId);


            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<CryptoWallet>()
                .HasKey(w => new { w.UserId, w.CryptoId });

            builder.Entity<CryptoWallet>()
                .HasOne(u => u.User)
                .WithMany(w => w.CryptoWallet)
                .HasForeignKey(k => k.UserId);

            builder.Entity<CryptoWallet>()
                .HasOne(c => c.Crypto)
                .WithMany()
                .HasForeignKey(k => k.CryptoId);

        }
    }
}
