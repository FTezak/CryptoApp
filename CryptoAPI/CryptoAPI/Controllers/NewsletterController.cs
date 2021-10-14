using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CryptoAPI.Entities;
using CryptoAPI.Extensions;
using CryptoAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CryptoAPI.Controllers
{
    public class NewsletterController : BaseApiController
    {

        private readonly UserManager<AppUser> _userManager;


        public NewsletterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Newsletter>> GetUserNewsletterSettings()
        {
            
            var sourceUser = await _userManager.Users
                    .Include(r => r.Newsletter)
                    .Where(u => u.Id == User.GetUserId())
                    .SingleOrDefaultAsync();

            if (sourceUser.Newsletter == null)
            {
                return Ok(new Newsletter()
                {
                    favoriteData = false,
                    frequency = 0,
                    walletData = false
                });
            }

            return Ok(sourceUser.Newsletter);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddUserNewsletterSettings(Newsletter newsletterConfig)
        {
            
            var sourceUser = await _userManager.Users
                .Include(r => r.Newsletter)
                .Where(u => u.Id == User.GetUserId())
                .SingleOrDefaultAsync();

            if (newsletterConfig.frequency == 0)
            {
                sourceUser.Newsletter = null;
            }
            else
            {
                sourceUser.Newsletter = newsletterConfig;
            }
            
            await _userManager.UpdateAsync(sourceUser);
            
            return Ok();
        }
        
    }
}
