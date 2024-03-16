using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudLearn.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChillController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("/adminAlik")]
        public string Admintext()
        {
            return "Salom";
        }

        [Authorize(Roles = "User")]
        [HttpGet("/userAlik")]
        public string UserText()
        {
            return "Alik";
        }
    }
}