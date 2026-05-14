using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ProjectTaskApi.Data;
using ProjectTaskApi.DTOs;
using ProjectTaskApi.DTOs.Tasks;
using ProjectTaskApi.Entities;
using System.Text.Json;

namespace ProjectTaskApi.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ProjectService> _logger;
        private readonly IDistributedCache _cache;

        public ProjectService(ApplicationContext context, ILogger<ProjectService> logger, IDistributedCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        public async Task<ProjectGetDto> CreateProjectAsync(CreateProjectDto project)
        {
            var newProject = new Project
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Name = project.Name,
                Description = project.Description
            };

            await _context.Projects.AddAsync(newProject);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Project created with id {ProjectId}",
                newProject.Id);

            return new ProjectGetDto
            {
                Id = newProject.Id,
                Name = newProject.Name,
                Description = newProject.Description,
                CreatedAt = newProject.CreatedAt,
                UpdatedAt = newProject.UpdatedAt
            };
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
           var project =  await _context.Projects.Where(x=>x.Id == id).FirstOrDefaultAsync();

            if(project == null)
            {
                _logger.LogWarning(
                    "Project with id {ProjectId} not found",
                    id);
                return false;
            }    

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                    "Project deleted with id {ProjectId}",
                    id);

            return true;
        }

        public async Task<ProjectDetailsDto?> GetProjectDetails(Guid id)
        {
            return await _context.Projects
                .AsNoTracking()
                .Where(x=>x.Id == id)
                .Include(x => x.Tasks)
                .Select(x => new ProjectDetailsDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Tasks = x.Tasks.Select(y=> new TaskGetDto
                    {
                        Id =y.Id,
                        Title = y.Title,
                        CreatedAt=y.CreatedAt,
                        UpdatedAt=y.UpdatedAt,
                        IsCompleted = y.IsCompleted,
                        Description = y.Description
                    }).ToList()

                }).FirstOrDefaultAsync();

        }

        public async Task<List<ProjectGetDto>> GetProjectsAsync(int page, int pageSize)
        {
            var cacheKey = $"projects_{page}_{pageSize}";

            var cachedProjects = await _cache.GetStringAsync(cacheKey);

            if (cachedProjects != null)
            {
                return JsonSerializer.Deserialize<List<ProjectGetDto>>(cachedProjects)!;
            }

            var projects = await _context.Projects
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

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(projects),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow =
                            TimeSpan.FromMinutes(5)
                    });

            return projects;
        }

        public async Task<bool> PutProjectAsync(CreateProjectDto project, Guid id)
        {
            var curProject = await _context.Projects.Where(x=>x.Id == id).FirstOrDefaultAsync();

            if (curProject == null)
            {
                _logger.LogWarning(
                        "Project with id {ProjectId} not found",
                        id);
                return false;
            }
            curProject.Name = project.Name;
            curProject.Description = project.Description;
            curProject.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                    "Project with id {ProjectId} changed",
                    id);

            return true;
        }
    }
}
