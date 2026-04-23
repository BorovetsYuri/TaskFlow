using System.ComponentModel.DataAnnotations;

namespace TaskFlow.DTOs
{
    public class TaskItemCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public int ProjectId { get; set; }
    }
}