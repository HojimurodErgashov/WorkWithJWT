using AutoMapper;
using Contracts;
using Entities.DTO.User;
using Entities.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrudLearn.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly IOptions<AppSettings> AppSettings;
        private readonly IMapper mapper;
        private readonly JwtSecurityTokenHandler securityTokenHandler;

        public UserController(IRepositoryManager repositoryManager, IOptions<AppSettings> AppSettings, IMapper mapper)
        {
            this.repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
            this.AppSettings = AppSettings ?? throw new ArgumentNullException(nameof(AppSettings));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            securityTokenHandler = new JwtSecurityTokenHandler();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserAuthInfoDTO>> LoginAsync([FromBody] UserCredentials userCredentials, CancellationToken cancellationToken)
        {
            UserAuthInfoDTO userAuthInfoDTO = new UserAuthInfoDTO();
            if (userCredentials == null)
            {
                return BadRequest("No data");
            }

            var user = await repositoryManager.User.
                LoginAsync(userCredentials.Login, userCredentials.Password, false, cancellationToken);

            if (user != null)
            {
                var key = Encoding.ASCII.GetBytes(AppSettings.Value.SecretKey);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.FirstName),
                            new Claim(ClaimTypes.GivenName, user.LastName),
                            new Claim(ClaimTypes.Role, user.Role.ToString())
                        }),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var securityToken = securityTokenHandler.CreateToken(tokenDescriptor);
                userAuthInfoDTO.Token = securityTokenHandler.WriteToken(securityToken);
                userAuthInfoDTO.UserDetails = mapper.Map<UserDTO>(user);
            }

            if (string.IsNullOrEmpty(userAuthInfoDTO.Token))
            {
                return Unauthorized("Error login or password");
            }

            return Ok(userAuthInfoDTO);
        }
    }
}