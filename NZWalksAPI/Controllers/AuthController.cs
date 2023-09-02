using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // add roles to this user
                identityResult = await userManager.AddToRoleAsync(identityUser, "Reader");

                if (identityResult.Succeeded)
                {
                    return Ok("User Registered Successfully! Please Login");
                }
            }

            return BadRequest("Somthing Went Wrong");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] RegisterRequestDto registerRequestDto)
        {
            var user = await userManager.FindByEmailAsync(registerRequestDto.Username);

            if(user != null)
            {
                var checkedPasswordResult = await userManager.CheckPasswordAsync(user, registerRequestDto.Password);
                
                if(checkedPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // crete token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        
                        return Ok(jwtToken);
                    }
                    
                }
            }

            return BadRequest("Username of Password incorrect");
        }
    }
}
