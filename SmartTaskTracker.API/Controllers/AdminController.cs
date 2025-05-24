using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.API.Data;
using SmartTaskTracker.API.DTOs;

namespace SmartTaskTracker.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController: ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("users-summary")]
        public async Task<ActionResult<List<UserSummaryDTO>>> GetUsersWithTaskCounts()
        {
            var users = await _context.Users
                .Select(user => new UserSummaryDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role,
                    TaskCount = _context.Tasks.Count(t => t.UserId == user.UserId)
                })
                .ToListAsync();

            return Ok(users);  // <-- return array directly, not wrapped
        }
        //[HttpGet("users-summary")]
        //public async Task<ActionResult<List<UserSummaryDTO>>> GetUsersWithTaskCounts()
        //{
        //    var users = await _context.Users
        //        .Select(user => new UserSummaryDTO
        //        {
        //            UserName = user.UserName,
        //            Email = user.Email,
        //            Role = user.Role,
        //            TaskCount = _context.Tasks.Count(t => t.UserId == user.UserId)
        //        })
        //        .ToListAsync();

        //    return Ok(new { users });
        //}
    }
}
