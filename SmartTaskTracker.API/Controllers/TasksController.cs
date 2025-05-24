using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.API.Data;
using SmartTaskTracker.API.Models;

namespace SmartTaskTracker.API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController:ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var username = User.Identity?.Name;
            var tasks = await _context.Tasks
                .Where(t => t.OwnerUserName == username)
                .OrderByDescending(t => t.CreatedAt)
                .ToArrayAsync();
            return Ok(new { tasks });
        }

      
        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task)
        {
            task.OwnerUserName = User.Identity?.Name!;
            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            task.CreatedAt = DateTime.UtcNow;

            task.UserId = userId;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(new { task });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem updatedTask)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null || task.OwnerUserName != User.Identity?.Name)
                return NotFound();

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;
            task.Deadline = updatedTask.Deadline;
            task.Priority = updatedTask.Priority;

            await _context.SaveChangesAsync();
            return Ok(new { task });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null || task.OwnerUserName != User.Identity?.Name)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
