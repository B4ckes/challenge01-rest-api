using System.Collections.Generic;
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
    public class ScoreController : ControllerBase
    {
        [HttpPost("scores")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateScoreViewModel model
        )
        {
            if (!ModelState.IsValid) return BadRequest();

            var score = new Score
            {
                UserId = model.UserId,
                ScoreAmount = model.ScoreAmount,
            };
            
            try
            {
                await context.Scores.AddAsync(score);
                await context.SaveChangesAsync();
                return Created($"v1/score/{score.Id}", score);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("scores")]
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context
        )
        {
            var scores = await context.Scores.OrderBy(s => s.ScoreAmount).Reverse().ToListAsync();

            List<ScoreResultViewModel> scoresResult = new List<ScoreResultViewModel>();
            
            foreach (var score in scores)
            {
                scoresResult.Add(new ScoreResultViewModel
                {
                  Username = context.Users.FirstOrDefaultAsync(u => u.Id == score.UserId).Result?.Username ?? "<DELETED>",
                  ScoreAmount = score.ScoreAmount
                }
                );
            }

            return Ok(scoresResult);
        }
    }
}