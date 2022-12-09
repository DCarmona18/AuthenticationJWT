using AuthenticationJWT.Domain.Interfaces;
using AuthenticationJWT.Domain.Services;
using AuthenticationJWT.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationJWT.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Token"></param>
    /// <param name="IsAuthenticated"></param>
    public record JwtResponse(string Token, bool IsAuthenticated);

    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenService"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthController(ITokenService tokenService, IConfiguration configuration)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException("_tokenService is null.");
            _configuration = configuration ?? throw new ArgumentNullException("_configuration is null.");
        }

        /// <summary>
        /// Simulates login endpoint:
        /// To test this endpoint use the following users
        /// </summary>
        /// <remarks>
        /// Sample request for admin:
        /// 
        ///     POST api/Auth
        ///     {        
        ///       "userName": "daniel.carmona.jaramillo@gmail.com",
        ///       "password": "123456"       
        ///     }
        ///     Sample request for verkaufer:
        /// 
        ///     POST api/Auth
        ///     {        
        ///       "userName": "andereEmail@mail.com",
        ///       "password": "123456"       
        ///     }
        /// </remarks>
        /// <param name="req"></param> 
        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<JwtResponse>> Validate(UserValidationRequestModel req)
        {
            if (req is UserValidationRequestModel)
            {
                var token = await _tokenService.BuildToken(_configuration.GetSection("Jwt").GetValue<string>("Key")!,
                                                    _configuration.GetSection("Jwt").GetValue<string>("Issuer")!,
                                                    new[]
                                                    {
                                                        _configuration.GetSection("Jwt").GetValue<string>("Aud")!
                                                            },
                                                    req.UserName, req.Password);
                return Ok(new JwtResponse(token, true));
            }

            return Accepted(new JwtResponse(string.Empty, false));
        }
    }
}
