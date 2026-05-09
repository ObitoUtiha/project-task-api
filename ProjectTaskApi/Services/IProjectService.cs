using ProjectTaskApi.DTOs;

namespace ProjectTaskApi.Services
{
        public interface IProjectService
        {
            Task<List<ProjectGetDto>> GetProjectsAsync(
                int page,
                int pageSize);

             Task<ProjectDetailsDto?> GetProjectDetails(Guid id);

             Task<ProjectGetDto> CreateProjectAsync(CreateProjectDto project);

             Task<bool> DeleteProjectAsync(Guid id);

             Task<bool> PutProjectAsync(CreateProjectDto project, Guid id);
    }
}
