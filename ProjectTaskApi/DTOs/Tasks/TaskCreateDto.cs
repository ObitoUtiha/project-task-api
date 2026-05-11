namespace ProjectTaskApi.DTOs.Tasks
{
    public class TaskCreateDto
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public Guid ProjectId { get; set; }
    }
}
