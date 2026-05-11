using Microsoft.EntityFrameworkCore;
using ProjectTaskApi.Data;
using ProjectTaskApi.DTOs.Tasks;

namespace ProjectTaskApi.Services
{
    public class TasksService : ITasksService
    {
        private readonly ApplicationContext _context;

        public TasksService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<TaskGetDto?> GetTaskById(Guid id)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new TaskGetDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedAt = x.CreatedAt,
                    IsCompleted = x.IsCompleted,
                    Description = x.Description,
                    UpdatedAt = x.UpdatedAt,
                    ProjectId = x.ProjectId
                }).FirstOrDefaultAsync();
        }

        public async Task<List<TaskGetDto>> GetTasksAsync(bool? status, Guid? projectId)
        {
            var query = _context.Tasks.AsNoTracking().AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(x => x.IsCompleted == status.Value);
            }

            if (projectId.HasValue)
            {
                query = query.Where(x => x.ProjectId == projectId.Value);
            }

            return await query
                .Select(x => new TaskGetDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedAt = x.CreatedAt,
                    IsCompleted = x.IsCompleted,
                    Description = x.Description,
                    UpdatedAt = x.UpdatedAt,
                    ProjectId = x.ProjectId
                })
                .ToListAsync();

        }
    }
}
