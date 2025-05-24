using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.API.Data;
using System.Security.Claims;

namespace SmartTaskTracker.API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var username = User.Identity.Name;

            var total = await _context.Tasks.Where(t => t.OwnerUserName == username).CountAsync();
            var completed = await _context.Tasks.Where(t => t.OwnerUserName == username && t.IsCompleted).CountAsync();
            var pending = total - completed;

            return Ok(new
            {
                total,
                completed,
                pending
            });
        }

        //[HttpGet("chart-data")]
        //public async Task<IActionResult> GetChartData()
        //{
        //    var username = User.Identity.Name;

        //    var data = await _context.Tasks
        //        .Where(t => t.OwnerUserName == username)
        //        .GroupBy(t => t.CreatedAt.Date)
        //        .Select(g => new
        //        {
        //            date = g.Key,
        //            count = g.Count()
        //        })
        //        .OrderBy(g => g.date)
        //        .ToListAsync();
           

        //    return Ok(data);
        //}


        [HttpGet("chart-data")]
        [Authorize]
        public async Task<IActionResult> GetChartData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Unauthorized();

            var tasks = await _context.Tasks
                .Where(t => t.UserId == int.Parse(userId))
                .ToListAsync();

            var completed = tasks.Count(t => t.IsCompleted);
            var pending = tasks.Count(t => !t.IsCompleted);

            var createdPerDay = tasks
                .GroupBy(t => t.CreatedAt.Date)
                .Select(g => new {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToList();

            return Ok(new
            {
                status = new { completed, pending },
                timeline = createdPerDay
            });
        }
    }
}
