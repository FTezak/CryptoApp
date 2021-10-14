using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CryptoAPI.Controllers
{

    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return Ok(await _userRepository.GetUsersAsync());
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return Ok(await _userRepository.GetUserByIdAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> removeUser(int id)
        {
            return Ok(await _userRepository.DeleteUserById(id));
        }

    }
}
