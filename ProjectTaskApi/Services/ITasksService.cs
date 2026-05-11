using ProjectTaskApi.DTOs.Tasks;

namespace ProjectTaskApi.Services
{
    public interface ITasksService
    {
        Task<List<TaskGetDto>> GetTasksAsync(
            bool? status, Guid? projectId);

        Task<TaskGetDto?> GetTaskById (Guid id);

        Task<TaskGetDto?> PostTaskAsync(TaskCreateDto task);
    }
}
