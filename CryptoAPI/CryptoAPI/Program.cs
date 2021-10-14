using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CryptoAPI.Data;
using CryptoAPI.Entities;
using CryptoAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CryptoAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                //await context.Database.MigrateAsync();
                //await Seed.SeedCrypto(cryptocompareApi: new CryptocompareApi(new CryptoRepository(context), new CryptoDataRepository(context)));
            }
            catch (Exception ex)
            {

                Log.Fatal(ex, "The app failed to start.");
            }

            await host.RunAsync();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                })
        .ConfigureServices(services =>
        {
            services.AddHostedService<TimedService>();
        });
    }
}
