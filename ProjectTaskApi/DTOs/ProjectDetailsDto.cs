using ProjectTaskApi.DTOs.Tasks;

namespace ProjectTaskApi.DTOs
{
    public class ProjectDetailsDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<TaskGetDto> Tasks { get; set; } = new();
    }
}
