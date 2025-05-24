using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.API.Data;
using SmartTaskTracker.API.DTOs;

namespace SmartTaskTracker.API.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class ProfileController: ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) return NotFound();

            return Ok(new
            {
                user.FullName,
                user.Email,
                user.UserName,
                user.Role
            });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile(UserProfileUpdateDTO dto)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) return NotFound();

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Profile updated successfully" });

        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.Password))
            {
                return BadRequest("Incorrect current password");
            }

            user.Password= BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _context.SaveChangesAsync();
            return Ok(new {message= "Password changed successfully" });
        }
    }
}
