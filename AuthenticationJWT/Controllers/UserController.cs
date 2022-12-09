using AuthenticationJWT.Infrastructure.Context.Entities;
using AuthenticationJWT.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthenticationJWT.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Admin Endpoint where only Admin user can retrieve data
        /// </summary>
        /// <returns>Logged user with its role</returns>
        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi you are an {currentUser.Role}");
        }

        /// <summary>
        /// Verkaufer Endpoint where only Seller user can retrieve data
        /// </summary>
        /// <returns>Logged user with its role</returns>
        [HttpGet("Verkaufer")]
        [Authorize(Roles = "Verkaufer")]
        public IActionResult VerkauferEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi you are an {currentUser.Role}");
        }

        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new User
                {
                    Email = userClaims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.UniqueName)?.Value!,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value!
                };
            }
            return null;
        }
    }
}
