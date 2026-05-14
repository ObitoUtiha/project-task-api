using Microsoft.EntityFrameworkCore;
using ProjectTaskApi.Entities;

namespace ProjectTaskApi.Data
{
    public static class DbInitializer
    {
        public static async Task SeedDataAsync(ApplicationContext context)
        {
            if (await context.Projects.AnyAsync())
                return;

            var projects = new List<Project>
        {
            new Project
            {
                Id = Guid.NewGuid(),
                Name = "Project one",
                Description = "Test project",
                CreatedAt = DateTime.UtcNow,

                Tasks = new List<TaskItem>
                {
                    new TaskItem
                    {
                        Id = Guid.NewGuid(),
                        Title = "Open Docker",
                        Description = "Run docker",
                        IsCompleted = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    new TaskItem
                    {
                        Id = Guid.NewGuid(),
                        Title = "Push commit",
                        Description = "Push changes to GitHub",
                        IsCompleted = false,
                        CreatedAt = DateTime.UtcNow
                    }
                }
            }
        };

            await context.Projects.AddRangeAsync(projects);
            await context.SaveChangesAsync();
        }
    }
}
