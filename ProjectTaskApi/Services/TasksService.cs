using Microsoft.EntityFrameworkCore;
using ProjectTaskApi.Common.Results;
using ProjectTaskApi.Data;
using ProjectTaskApi.DTOs.Tasks;
using ProjectTaskApi.Entities;

namespace ProjectTaskApi.Services
{
    public class TasksService : ITasksService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<TasksService> _logger;

        public TasksService(ApplicationContext context, ILogger<TasksService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
            {
                _logger.LogWarning(
                        "Task with id {TaskId} not found",
                        id);
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                    "Task with id {TaskId} deleted",
                    id);

            return true;
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
                _logger.LogWarning(
                        "Project with id {ProjectId} not found",
                        task.ProjectId);
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

            _logger.LogInformation(
                    "Task with id {TaskId} created",
                    newTask.Id);

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

        public async Task<UpdateTaskResult> PutTaskAsync(TaskCreateDto task, Guid id)
        {
            var currentTask = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (currentTask == null)
            {
                _logger.LogWarning(
                        "Task with id {TaskId} not found",
                        id);
                return UpdateTaskResult.TaskNotFound;
            }


            var projectExists = await _context.Projects
                .AnyAsync(x => x.Id == task.ProjectId);

            if (!projectExists)
            {
                _logger.LogWarning(
                        "Project with id {ProjectId} not found",
                        task.ProjectId);
                return UpdateTaskResult.ProjectNotFound;
            }
            

            currentTask.Title = task.Title;
            currentTask.Description = task.Description;
            currentTask.IsCompleted = task.IsCompleted;
            currentTask.ProjectId = task.ProjectId;
            currentTask.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                    "Task with id {TaskId} changed",
                    id);

            return UpdateTaskResult.Success;

        }
    }
}
