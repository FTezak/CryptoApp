using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CryptoAPI.DTOs;
using CryptoAPI.Entities;
using CryptoAPI.Extensions;
using CryptoAPI.Interfaces;
using CryptoAPI.Services;
using CryptoAPI.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CryptoAPI.Controllers
{
    public class WalletController : BaseApiController
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly WalletService _ws;
        private readonly Crypting _crypting;
        private readonly ICryptoService _cryptoService;

        public WalletController(IConfiguration config, UserManager<AppUser> userManager, IMapper mapper, IServiceProvider serviceProvider, ICryptoService cryptoService)
        {
            _userManager = userManager;
            _ws = new WalletService(serviceProvider, config);
            _crypting = new Crypting(config);
            _cryptoService = cryptoService;

        }

        
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<WalletDto>> GetUserWallet()
        {

            var sourceUser = await _userManager.Users
                    .Include(r => r.CryptoWallet)
                    .Where(u => u.Id == User.GetUserId())
                    .SingleOrDefaultAsync();

            WalletDto userWallet = await _ws.getUserWallet(sourceUser);

            return Ok(userWallet);
        }

        
        [Authorize]
        [HttpGet("{cryptoSym}")]
        public async Task<ActionResult<decimal>> GetAmountOfCryptoInWalletBySymbol(string cryptoSym)
        {

            var sourceUser = await _userManager.Users
                    .Include(r => r.CryptoWallet)
                    .Where(u => u.Id == User.GetUserId())
                    .SingleOrDefaultAsync();

            var wallet = sourceUser.CryptoWallet;

            CryptoCurrency crypto = await _cryptoService.GetCryptoBySymbolAsync(cryptoSym);

            CryptoWallet walletCrypto = wallet.FirstOrDefault(c => c.CryptoId == crypto.Id);

            if (walletCrypto == null) return 0;

            return walletCrypto.Amount;

        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddToCryptoWalletForUser(CryptoWalletDto walletDto)
        {

            var sourceUser = await _userManager.Users
                .Include(r => r.CryptoWallet)
                .Where(u => u.Id == User.GetUserId())
                .SingleOrDefaultAsync();


            var wallet = sourceUser.CryptoWallet;

            var crypto = await _cryptoService.GetCryptoBySymbolAsync(walletDto.cryptoSym);


            if (wallet.FirstOrDefault(c => c.CryptoId == crypto.Id) != null)
            {
                if (walletDto.amount == 0)
                {
                    wallet.Remove(wallet.FirstOrDefault(c => c.CryptoId == crypto.Id));
                }
                else
                {
                    wallet.FirstOrDefault(c => c.CryptoId == crypto.Id).Amount = walletDto.amount;
                }
            }
            else
            {
                if (walletDto.amount > 0)
                {
                    CryptoWallet cw = new CryptoWallet()
                    {
                        Amount = walletDto.amount,
                        Crypto = crypto,
                        User = sourceUser
                    };

                    wallet.Add(cw);
                }
            }

            await _userManager.UpdateAsync(sourceUser);
            
            return Ok();
        }


        [Authorize]
        [HttpPost("BinanceKeys")]
        public async Task<ActionResult> AddBinanceApiKeys(BinanceKeysDto keys)
        {

            var sourceUser = await _userManager.Users
                .Where(u => u.Id == User.GetUserId())
                .SingleOrDefaultAsync();

            if (keys.key != "" && keys.secretKey != "")
            {
                sourceUser.BinanceApiKey = _crypting.EncryptString(keys.key);
                sourceUser.BinanceSecretApiKey = _crypting.EncryptString(keys.secretKey);
            }
            else
            {
                sourceUser.BinanceApiKey = null;
                sourceUser.BinanceSecretApiKey = null;
            }

            await _userManager.UpdateAsync(sourceUser);
            
            return Ok();
        }
    }
}
