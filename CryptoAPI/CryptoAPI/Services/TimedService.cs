using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;
using CryptoAPI.Models;
using CryptoAPI.Models.CryptoModels;
using CryptoAPI.Tasks;
using FluentScheduler;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace CryptoAPI.Services
{
    public class TimedService : IHostedService, ITimedService
    {

        private readonly IEmailService _emailService;
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly ICryptoRepository _cryptoRepository;
        private readonly ICryptoDataRepository _cryptoDataRepository;
        private readonly CryptocompareApi _cca;
        private readonly IServiceProvider _services;
        public TimedService(IServiceProvider services, IEmailService emailService, IHostEnvironment hostingEnvironment, IServiceProvider serviceProvider, IConfiguration config)
        {

            _emailService = emailService;
            _hostingEnvironment = hostingEnvironment;
            _cryptoRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICryptoRepository>();
            _cryptoDataRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICryptoDataRepository>();

            _cca = new CryptocompareApi(_cryptoRepository, _cryptoDataRepository, config);
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            var registry = new Registry();


            DateTime date = DateTime.Now.AddHours(1);

            DateTime dateToSend = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);

            Console.WriteLine("SERVIS salje prvi mail u: " + dateToSend);

            Log.Information("SERVIS startan u " + DateTime.Now);
            Log.Information("SERVIS salje prvi mail u " + dateToSend);

            registry.Schedule(async () => await SomeBackgroundTask()).ToRunOnceAt(dateToSend).AndEvery(1).Hours();

            JobManager.Initialize(registry);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
 
            JobManager.RemoveAllJobs();

            Log.Information("Servis staje u " + DateTime.Now);

            return Task.CompletedTask;
        }

        public async Task SomeBackgroundTask()
        {


            Console.WriteLine("RADIM!!! - " + DateTime.Now);

            if (_hostingEnvironment.IsProduction())
            {
                EmailData data = new EmailData()
                {

                    EmailToId = "filip@tezak.com",
                    EmailToName = "filip@tezak.com",
                    EmailSubject = "Pinopt sinkronizacija podataka - " + DateTime.Now,
                    EmailBody = "Preuzimam nove cijene kripto valuta! -> " + DateTime.Now

                };

                Log.Information("--> SLANJE MAIL-a --> " + DateTime.Now);

                string statusSlanja = await _emailService.SendEmail(data);

                Log.Information("--> ODGOVOR SLANJa MAIL-a --> " + statusSlanja);
                
                await _cca.callCurrentPriceApi();
                await _cca.SetAllTimeHigh();

                NewsletterSenderService ns = new NewsletterSenderService(_services);
                await ns.sendNewsletters();



            }

            if (_hostingEnvironment.IsDevelopment())
            {
                Console.WriteLine("Izvrsavam samo stvari koje smijem u developmentu!");

                //NewsletterSenderService ns = new NewsletterSenderService(_services);
                //await ns.sendNewsletters();

                //Log.Information("Slanje mailova završilo! ");


            }
            
        }
        
    }
}
