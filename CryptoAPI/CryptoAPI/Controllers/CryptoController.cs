using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CryptoAPI.DTOs;
using CryptoAPI.Entities;
using CryptoAPI.Extensions;
using CryptoAPI.Helpers;
using CryptoAPI.Interfaces;
using CryptoAPI.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CryptoAPI.Controllers
{
    public class CryptoController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly CryptocompareApi _cca;
        private readonly ICryptoService _cryptoService;
        private readonly ICryptoDataService _cryptoDataService;

        public CryptoController(IConfiguration config, ICryptoRepository cryptoRepository, ICryptoDataRepository cryptoDataRepository, UserManager<AppUser> userManager, IMapper mapper, ICryptoService cryptoService, ICryptoDataService cryptoDataService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _cca = new CryptocompareApi(cryptoRepository, cryptoDataRepository, config);
            _cryptoService = cryptoService;
            _cryptoDataService = cryptoDataService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CryptoCurrency>>> GetCryptos([FromQuery] CryptoParams cryptoParams)
        {
            var data = await _cryptoService.GetCryptosPagedAsync(cryptoParams);

            Response.AddPaginationHeader(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);

            return Ok(data);
        }

        [HttpGet("home")]
        public async Task<ActionResult<IEnumerable<CryptoDataDto>>> GetCryptosHome([FromQuery] CryptoParams cryptoParams)
        {

            var listOfData = await _cca.getCryptosDtoHomeData(cryptoParams);

            if (User.Identity.IsAuthenticated)
            {
                var sourceUser = await _userManager.Users.AsNoTracking()
                        .Include(r => r.FavoriteCrypto)
                        .Where(u => u.Id == User.GetUserId())
                        .SingleOrDefaultAsync();

                foreach (var item in listOfData)
                {
                    if (sourceUser.FavoriteCrypto.FirstOrDefault(c => c.Symbol == item.Symbol) != null)
                    {
                        item.Favorite = true;
                    }
                }
            }

            Response.AddPaginationHeader(listOfData.CurrentPage, listOfData.PageSize, listOfData.TotalCount,
                listOfData.TotalPages);

            return Ok(listOfData);
        }


        [HttpGet("home/{cryptoSym}")]
        public async Task<ActionResult<CryptoDataDto>> GetCryptosHomeForCrypto(string cryptoSym)
        {
            
            var Data = await _cca.getCryptosDtoHomeDataForCrypto(cryptoSym);

            if (User.Identity.IsAuthenticated)
            {
                var sourceUser = await _userManager.Users
                    .Include(r => r.FavoriteCrypto).AsNoTracking()
                    .Where(u => u.Id == User.GetUserId())
                    .SingleOrDefaultAsync();

                
                    if (sourceUser.FavoriteCrypto.FirstOrDefault(c => c.Symbol == Data.Symbol) != null)
                    {
                        Data.Favorite = true;
                    }
            }
            
            return Ok(Data);
        }


        [Authorize]
        [HttpGet("userfav")]
        public async Task<ActionResult<IEnumerable<CryptoDto>>> GetFavoriteCryptos()
        {
            
                var sourceUser = await _userManager.Users.AsNoTracking()
                    .Include(r => r.FavoriteCrypto)
                    .Where(u => u.Id == User.GetUserId())
                    .SingleOrDefaultAsync();

                var list = _mapper.Map<List<CryptoDto>>(sourceUser.FavoriteCrypto);
            
            return Ok(list);
        }


        [HttpGet("searchCrypto/{name_like}")]
        public async Task<ActionResult<IEnumerable<CryptoDto>>> GetSearchCryptos(string name_like)
        {
            
            var crypto = await _cryptoService.GetCryptoByNameLikeAsync(name_like);
            
            return Ok(crypto);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<CryptoDto>>> GetAllCryptos()
        {
            
            var crypto = await _cryptoService.GetAllCryptosAsync();
            
            return Ok(crypto);
        }


        [Authorize]
        [HttpPost("{cryptoSym}")]
        public async Task<ActionResult> AddLike(string cryptoSym)
        {
          
            var sourceUser = await _userManager.Users
                .Include(r => r.FavoriteCrypto)
                .Where(u => u.Id == User.GetUserId())
                .SingleOrDefaultAsync();
            
            var crypto = sourceUser.FavoriteCrypto.FirstOrDefault(c => c.Symbol == cryptoSym);

            if (crypto == null)
            {
                crypto = await _cryptoService.GetCryptoBySymbolAsync(cryptoSym);
            }

            if (sourceUser.FavoriteCrypto.Contains(crypto))
            {
                sourceUser.FavoriteCrypto.Remove(crypto);
            }
            else
            {
                sourceUser.FavoriteCrypto.Add(crypto);
            }
            
            await _userManager.UpdateAsync(sourceUser);

            return Ok();
        }


        [Authorize]
        [HttpPost("fav/{cryptoSym}")]
        public async Task<ActionResult<bool>> IsCryptoFavorite(string cryptoSym)
        {
            
            var sourceUser = await _userManager.Users
                .Include(r => r.FavoriteCrypto)
                .Where(u => u.Id == User.GetUserId())
                .SingleOrDefaultAsync();

            var crypto = await _cryptoService.GetCryptoBySymbolAsync(cryptoSym);

            if (sourceUser.FavoriteCrypto.Contains(crypto))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        [HttpGet("details/{cryptoSym}")]
        public async Task<ActionResult<IEnumerable<CryptoCurrencyDetailsData>>> GetCryptoDetails(string cryptoSym)
        {

            IEnumerable<CryptoCurrencyData> dataFromDB = await _cryptoDataService.GetCryptoDataAsyncBySymbol(cryptoSym);

            var listOfData = await _cca.GetCryptosMinuteDataForDayBySymbol(cryptoSym);

            List<CryptoCurrencyData> list = new List<CryptoCurrencyData>();

            list.AddRange(dataFromDB);

            list.AddRange(listOfData);

            list.Sort(new Comparison<CryptoCurrencyData>((x,y) => DateTime.Compare(x.Date, y.Date)));

            List<CryptoCurrencyDetailsData> list2 = new List<CryptoCurrencyDetailsData>();
            
            foreach (var item in list)
            {
                list2.Add(new CryptoCurrencyDetailsData()
                {
                    Date = item.Date,
                    Open = item.Open
                });
            }

            return Ok(list2);
        }

    }
}
