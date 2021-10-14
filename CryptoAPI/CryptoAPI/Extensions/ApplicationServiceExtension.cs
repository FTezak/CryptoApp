using CryptoAPI.Data;
using CryptoAPI.Helpers;
using CryptoAPI.Interfaces;
using CryptoAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoAPI.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITimedService, TimedService>();
            services.AddScoped<INewsletterSenderService, NewsletterSenderService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICryptoRepository, CryptoRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<ICryptoDataRepository, CryptoDataRepository>();
            services.AddScoped<ICryptoWalletRepository, CryptoWalletRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging(true);
            });

            return services;
        }
    }
}
