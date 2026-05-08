using ProjectTaskApi.DTOs;

namespace ProjectTaskApi.Services
{
        public interface IProjectService
        {
            Task<List<ProjectGetDto>> GetProjectsAsync(
                int page,
                int pageSize);

             Task<ProjectDetailsDto?> GetProjectDetails(Guid id);
        }
}
