using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestApi.Data;
using RestApi.ViewModels;
using RestApi.Models;

namespace RestApi.Controller
{
    [ApiController]
    [Route("v1")]
    public class UserController : ControllerBase
    {
        [HttpPost("users")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateUserViewModel model
        )
        {
            if (!ModelState.IsValid) return BadRequest();

            var existentUser = await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(model.Email));

            if (existentUser != null) return BadRequest("Já existe um cadastro com este email.");

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
            };

            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return Created($"v1/user/{user.Id}", user);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> Get(
            [FromServices] AppDbContext context    
        )
        {
            var users = await context.Users.ToListAsync();

            return Ok(users);
        }
    
        [HttpGet]
        [Route("users/{id:int}")]
        public async Task<IActionResult> GetById(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            var user = await context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(user);
        }

        [HttpPut("users/{id:int}")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] UpdateUserInfoViewModel model,
            [FromRoute] int id
        )
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await context
                .Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return NotFound();

            try
            {
                user.Username = model.Username ?? user.Username;
                user.Email = model.Email ?? user.Email;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Ok(user);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("users/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id
        )
        {
            var todo = await context
                .Users
                .FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                context.Users.Remove(todo);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("users/{id:int}/scores")]
        public async Task<IActionResult> GetUserScoresAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id
        )
        {
            var scores = await context
                .Scores
                .Where(s => s.UserId == id)
                .ToListAsync();

            return Ok(scores);
        }
    }
}