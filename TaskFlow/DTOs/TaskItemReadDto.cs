namespace TaskFlow.DTOs
{
    public class TaskItemReadDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreatedAt { get; set; }

        public int ProjectId { get; set; }
    }
}