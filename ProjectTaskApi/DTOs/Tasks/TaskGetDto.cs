using ProjectTaskApi.Entities;

namespace ProjectTaskApi.DTOs.Tasks
{
    public class TaskGetDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }
}
