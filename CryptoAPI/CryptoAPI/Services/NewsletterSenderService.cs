using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CryptoAPI.DTOs;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;
using CryptoAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CryptoAPI.Services
{
    public class NewsletterSenderService : INewsletterSenderService

    {
        
        private readonly IWalletService _walletService;
        private readonly ICryptoService _cryptoService;
        private readonly IEmailService _emailService;
        private readonly ITemplateRepository _templateRepository;
        private readonly IUserService _userService;

        public NewsletterSenderService(IServiceProvider serviceProvider)
        {
            _walletService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IWalletService>();
            _cryptoService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICryptoService>();
            _emailService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IEmailService>();
            _templateRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ITemplateRepository>();
            _userService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IUserService>();
        }



        public async Task<bool> sendNewsletters()
        {

            NumberFormatInfo setPrecision = new NumberFormatInfo();

            setPrecision.NumberDecimalDigits = 3;

            Templates templates = await _templateRepository.GetTemplates();


            List<AppUser> users = await _userService.GetAllUsersWithWalletNewsletterFavorite();

            foreach (var user in users)
            {
                if (user.Newsletter != null)
                {
                    Log.Information("--> Newsletter dohvaćen za --> " + user.Email + " --> nije null!");

                    int hour = DateTime.Now.Hour;

                    int frequency = user.Newsletter.frequency;
                    int sendSpan = 24 / frequency;

                    if (frequency == 1 && hour == 12 || frequency > 1 && hour % sendSpan == 0)
                    {

                        //Šalji

                        Log.Information("--> Konfiguriram mail na --> " + user.Email);
                        
                        string WalletTemplate = templates.WalletTemplate;
                        string FavoriteTemplate = templates.FavoriteTemplate;

                        string WalletDataTemplate = templates.WalletDataTemplate;
                        string FavoriteDataTemplate = templates.FavoriteDataTemplate;

                        string MailTemplate = templates.MailTemplate;

                        MailTemplate = MailTemplate.Replace("[user]", user.UserName);

                        if (user.Newsletter.walletData)
                        {
                            //učitaj wallet usera

                            WalletDto userWallet = await _walletService.getUserWallet(user);

                            string walletData = "";

                            foreach (var crypto in userWallet.Cryptos)
                            {
                                decimal totalAmout = (crypto.amount + crypto.binanceAmount);
                                decimal totalValue = totalAmout * crypto.Price;
                                walletData += WalletDataTemplate
                                                .Replace("[cryptoName]", crypto.Name)
                                                .Replace("[cryptoAmount]", totalAmout.ToString("N", setPrecision))
                                                .Replace("[cryptoValue]", totalValue.ToString("N", setPrecision))
                                                .Replace("[cryptoApiReference]", crypto.CryptoApiReference.ToString());
                            }

                            WalletTemplate = WalletTemplate.Replace("[walletValue]", userWallet.totalValue.ToString("N", setPrecision));
                            WalletTemplate = WalletTemplate.Replace("[walletData]", walletData);
                            MailTemplate = MailTemplate.Replace("[wallet]", WalletTemplate);
                        }

                        if (user.Newsletter.favoriteData)
                        {
                            //učitaj vrijednosti korisnikovih favorita

                            string favoriteData = "";

                            foreach (var crypto in user.FavoriteCrypto)
                            {
                                decimal cryptoCurrentValue = await _cryptoService.getCurrentCryptoPriceFromDB(crypto);

                                favoriteData += FavoriteDataTemplate
                                    .Replace("[cryptoName]", crypto.Name)
                                    .Replace("[cryptoValue]", cryptoCurrentValue.ToString("N", setPrecision))
                                    .Replace("[cryptoApiReference]", crypto.CryptoApiReference.ToString());
                            }


                            FavoriteTemplate = FavoriteTemplate.Replace("[favoriteData]", favoriteData);
                            MailTemplate = MailTemplate.Replace("[favorite]", FavoriteTemplate);
                        }


                        EmailData data = new EmailData()
                        {

                            EmailToId = user.Email,
                            EmailToName = user.UserName,
                            EmailSubject = "PinOpt",
                            EmailBody = MailTemplate

                        };

                        Log.Information("--> SLANJE MAIL-a na "+ user.Email + " --> " + DateTime.Now);

                        string statusSlanja = await _emailService.SendEmail(data);

                        Log.Information("--> ODGOVOR SLANJa MAIL-a na " + user.Email + " --> " + statusSlanja);

                    }

                }
            }

            return false;
        }
    }
}
