using Microsoft.EntityFrameworkCore;
using ProjectTaskApi.Data;
using ProjectTaskApi.DTOs;
using ProjectTaskApi.DTOs.Tasks;
using ProjectTaskApi.Entities;

namespace ProjectTaskApi.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationContext _context;

        public ProjectService(ApplicationContext context)
        {
            _context = context;
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
                return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

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

        public async Task<bool> PutProjectAsync(CreateProjectDto project, Guid id)
        {
            var curProject = await _context.Projects.Where(x=>x.Id == id).FirstOrDefaultAsync();

            if (curProject == null)
                return false;
            curProject.Name = project.Name;
            curProject.Description = project.Description;
            curProject.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
