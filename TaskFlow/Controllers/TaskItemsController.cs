using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.DTOs;
using TaskFlow.Models;

namespace TaskFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemReadDto>>> GetTaskItems()
        {
            var taskItems = await _context.TaskItems
                .Select(t => new TaskItemReadDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsDone = t.IsDone,
                    CreatedAt = t.CreatedAt,
                    ProjectId = t.ProjectId
                })
                .ToListAsync();

            return Ok(taskItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemReadDto>> GetTaskItem(int id)
        {
            var taskItem = await _context.TaskItems
                .Where(t => t.Id == id)
                .Select(t => new TaskItemReadDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsDone = t.IsDone,
                    CreatedAt = t.CreatedAt,
                    ProjectId = t.ProjectId
                })
                .FirstOrDefaultAsync();

            if (taskItem == null)
            {
                return NotFound();
            }

            return Ok(taskItem);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemReadDto>> CreateTaskItem(TaskItemCreateDto dto)
        {
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == dto.ProjectId);

            if (!projectExists)
            {
                return BadRequest("Project with this id does not exist.");
            }

            var taskItem = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                ProjectId = dto.ProjectId
            };

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            var result = new TaskItemReadDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                IsDone = taskItem.IsDone,
                CreatedAt = taskItem.CreatedAt,
                ProjectId = taskItem.ProjectId
            };

            return CreatedAtAction(nameof(GetTaskItem), new { id = taskItem.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskItem(int id, TaskItemUpdateDto dto)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            taskItem.Title = dto.Title;
            taskItem.Description = dto.Description;
            taskItem.IsDone = dto.IsDone;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}