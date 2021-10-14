using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using CryptoAPI.DTOs;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;
using CryptoAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CryptoAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        public AccountController(IMapper mapper, ITokenService tokenService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IConfiguration config, IUserService userService)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _config = config;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userService.UserExists(registerDto.UserName)) return BadRequest("Username is taken");

            if (await _userService.EmailExists(registerDto.Email)) return BadRequest("Email is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.UserName;

            user.Email = registerDto.Email;

            var result = await _userService.CreateAsync(user, registerDto.Password);

            if (!result) return BadRequest("greška");
            
            var userFromDb = await _userService.FindByNameAsync(registerDto.UserName);

            var token = await _userService.GenerateEmailConfirmationTokenAsync(userFromDb);

            var uriBuilder = new UriBuilder(_config["AppPath"] + "confirm-email");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userid"] = userFromDb.Id.ToString();
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();

            EmailData data = new EmailData()
            {
                EmailToId = registerDto.Email,
                EmailToName = registerDto.Email,
                EmailSubject = "PinOpt - Email confirmation",
                EmailBody = "Click the link below to confirm Email address " + registerDto.Email + " for account at PinOpt.com \n \n \n" + urlString,

            };

            Log.Information("Sending confirm email to " + registerDto.Email);

            string sentStatus = await _emailService.SendEmail(data);

            Log.Information("Send confirm email status: " + sentStatus);

            return new UserDto
            {
                UserName = user.UserName,
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(LoginDto loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!_userManager.IsEmailConfirmedAsync(user).Result)
            {
                return Unauthorized("Email is not confirmed");
            }

            if (!result.Succeeded) return Unauthorized(result);

            return new UserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }


        [HttpPost("changePassword")]
        public async Task<ActionResult> changePassword(ChangePasswordDto changePasswordDto)
        {
           
            var user = await _userService.FindByEmailAsync(changePasswordDto.email);

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.oldPassword, changePasswordDto.password);

            if (result.Succeeded) return Ok();

            return BadRequest("Password change error");
        }

        [HttpPost("confirmEmail")]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {

            var user = await _userManager.FindByIdAsync(confirmEmailDto.userid.ToString());

            var result = await _userManager.ConfirmEmailAsync(user, confirmEmailDto.token);

            if(result.Succeeded) return Ok();

            return BadRequest("Confirm Email error");
        }
    }
}
