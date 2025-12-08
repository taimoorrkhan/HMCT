using HMCT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace HMCT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TaskItem : ControllerBase
    {
        private readonly AppDbContext _context;
        public TaskItem(AppDbContext context)
        {
            _context = context;
        }

        public class CreateTaskRequest
        {
            [Required, MinLength(3)]
            public required string Title { get; set; }

            [MaxLength(250)]
            public string? Description { get; set; }

            [Required]
            [RegularExpression("^(Pending|InProgress|Completed)$", ErrorMessage = "Invalid status")]
            public required string Taskstatus { get; set; }

            [Required]
            public DateTime DueDateTime { get; set; }
        }

        [HttpPost]
        [Route("createTask")]
        public async Task<IActionResult> createTask([FromBody] CreateTaskRequest request)
        {
            var task = new Models.TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                Taskstatus = request.Taskstatus,
                DueDateTime = request.DueDateTime
            };
            _context.Add(task);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Task created successfully", Task = task });
        }
    }
}
