namespace ProjectTaskApi.Entities
{
    public class TaskItem
    {

        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; } = null!;
    }
}
