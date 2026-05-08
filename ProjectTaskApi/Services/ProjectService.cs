using Microsoft.EntityFrameworkCore;
using ProjectTaskApi.Data;
using ProjectTaskApi.DTOs;

namespace ProjectTaskApi.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationContext _context;

        public ProjectService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectGetDto>> GetProjectsAsync(int page, int pageSize)
        {
            return await _context.Projects
                 .AsNoTracking()
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .Select(x => new ProjectGetDto
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Description = x.Description,
                     CreatedAt = x.CreatedAt,
                     UpdatedAt = x.UpdatedAt
                 })
                 .ToListAsync();
        }
    }
}
