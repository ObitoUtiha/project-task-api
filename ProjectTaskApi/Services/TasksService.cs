using Microsoft.EntityFrameworkCore;
using ProjectTaskApi.Data;
using ProjectTaskApi.DTOs.Tasks;
using ProjectTaskApi.Entities;

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

        public async Task<TaskGetDto?> PostTaskAsync(TaskCreateDto task)
        {
            var projectExists = await _context.Projects
                  .AnyAsync(x => x.Id == task.ProjectId);

            if (!projectExists)
            {
                return null;
            }

            var newTask = new TaskItem
            {
                Id = Guid.NewGuid(),
                Description = task.Description,
                CreatedAt = DateTime.UtcNow,
                IsCompleted = task.IsCompleted,
                Title = task.Title,
                ProjectId = task.ProjectId
            };

            await _context.Tasks.AddAsync(newTask);

            await _context.SaveChangesAsync();

            return new TaskGetDto
            {
                Id = newTask.Id,
                Description = newTask.Description,
                CreatedAt = newTask.CreatedAt,
                IsCompleted = newTask.IsCompleted,
                Title = newTask.Title,
                ProjectId = newTask.ProjectId,
                UpdatedAt = newTask.UpdatedAt
            };
        }
    }
}
