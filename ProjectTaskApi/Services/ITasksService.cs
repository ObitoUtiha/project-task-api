using ProjectTaskApi.DTOs.Tasks;

namespace ProjectTaskApi.Services
{
    public interface ITasksService
    {
        Task<List<TaskGetDto>> GetTasksAsync(
            bool? status, Guid? projectId);
    }
}
