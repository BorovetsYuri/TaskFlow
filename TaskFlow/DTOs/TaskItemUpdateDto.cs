using System.ComponentModel.DataAnnotations;

namespace TaskFlow.DTOs
{
    public class TaskItemUpdateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsDone { get; set; }
    }
}