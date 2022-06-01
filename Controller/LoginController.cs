using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data;
using RestApi.ViewModels;

namespace RestApi.Controller
{
    [ApiController]
    [Route("v1")]
    public class LoginController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] LoginViewModel model
        )
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null) return BadRequest("Email ou senha incorretos");
            
            if (user.Password != model.Password) return BadRequest("Email ou senha incorretos");

            return Ok(user);
        }
    }
}