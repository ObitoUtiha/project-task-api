using ProjectTaskApi.DTOs;

namespace ProjectTaskApi.Services
{
        public interface IProjectService
        {
            Task<List<ProjectGetDto>> GetProjectsAsync(
                int page,
                int pageSize);
        }
}
