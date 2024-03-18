using AutoMapper;
using Contracts;
using CrudLearn.Attributes;
using Entities.DTO.User;
using Entities.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrudLearn.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("logOut")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return NotFound();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<UserDTO>> DeleteAsync(Guid id)
        {
            User user = await repositoryManager.User.DeleteAsync(id);
            if (user != null)
            {
                await repositoryManager.SaveAsync();
                UserDTO userDTO = mapper.Map<UserDTO>(user);
                return userDTO;
            }
            return NotFound();
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<UserDTO>> GetByIdAsync(Guid id , CancellationToken cancellationToken)
        {
            User user = await repositoryManager.User.GetById(id, false, cancellationToken);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<UserDTO>(user));
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult<UserDTO>> CreateAsync([FromBody]UserCreateDTO userCreateDTO)
        {
            if(userCreateDTO == null)
            {
                return BadRequest();
            }

            User user = mapper.Map<User>(userCreateDTO);
            user.Role = RoleEnum.User;
            await repositoryManager.User.CreateAsync(user);
            await repositoryManager.SaveAsync();
            UserDTO userDTO = mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }

        [HttpGet("getAll")]
        public ActionResult<List<UserDTO>> GetAll()
        {
            List<UserDTO> userDTOs = new List<UserDTO>();
            List<User> users = repositoryManager.User.GetAll();
            if(users == null)
            {
                return NoContent();
            }

            foreach(var user in users)
            {
                if(user == null)
                {
                    continue;
                }
                var userDTO = mapper.Map<UserDTO>(user);
                userDTOs.Add(userDTO);
            }
            return Ok(userDTOs);

        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<UserDTO>> UpdateAsync([FromBody] UserDTO userDTO, Guid id, CancellationToken cancellationToken)
        {
            if (userDTO == null)
            {
                return BadRequest("Siz hech qanday ma'lumot kiritmadingzi!");
            }

            User user = await repositoryManager.User.GetById(id, false, cancellationToken);

            if (user == null)
            {
                return NotFound("Bunday id da user mavjud emas");
            }

            MapUserDTOFieldsToUserFields(user , userDTO);

            user = await repositoryManager.User.UpdateAsync(user);
            await repositoryManager.SaveAsync();
            userDTO = mapper.Map<UserDTO>(user);
            return userDTO;
        }

        private static void MapUserDTOFieldsToUserFields(User user , UserDTO userDTO)
        {
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Login = userDTO.Login;
        }

    }
}