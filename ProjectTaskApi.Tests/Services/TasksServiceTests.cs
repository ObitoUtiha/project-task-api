using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectTaskApi.Common.Results;
using ProjectTaskApi.Data;
using ProjectTaskApi.DTOs.Tasks;
using ProjectTaskApi.Entities;
using ProjectTaskApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTaskApi.Tests.Services
{
    public class TasksServiceTests
    {
        private readonly ApplicationContext _context;

        private readonly TasksService _tasksService;



        public TasksServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationContext(options);


            var loggerMock = new Mock<ILogger<TasksService>>();

            _tasksService = new TasksService(_context, loggerMock.Object);
        }

        [Fact]
        public async Task PostTaskAsync_ShouldCreateTask()
        {
            // Arrange

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Test Project",
                CreatedAt = DateTime.UtcNow
            };

            await _context.Projects.AddAsync(project);

            await _context.SaveChangesAsync();

            var dto = new TaskCreateDto
            {
                Title = "Test Task",
                Description = "Test Description",
                IsCompleted = false,
                ProjectId = project.Id
            };

            // Act

            var result = await _tasksService
                .PostTaskAsync(dto);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(dto.Title, result.Title);

            Assert.Equal(dto.Description, result.Description);

            Assert.Single(_context.Tasks);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldDeleteTask()
        {
            // Arrange

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Test Task",
                CreatedAt = DateTime.UtcNow
            };

            await _context.Tasks.AddAsync(task);

            await _context.SaveChangesAsync();

            // Act

            var result = await _tasksService
                .DeleteTaskAsync(task.Id);

            // Assert

            Assert.True(result);

            Assert.Empty(_context.Tasks);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldReturnFalse_WhenTaskNotFound()
        {
            // Arrange

            var fakeId = Guid.NewGuid();

            // Act

            var result = await _tasksService
                .DeleteTaskAsync(fakeId);

            // Assert

            Assert.False(result);
        }

        [Fact]
        public async Task PutTaskAsync_ShouldUpdateTask()
        {
            // Arrange

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Test Project",
                CreatedAt = DateTime.UtcNow
            };

            await _context.Projects.AddAsync(project);

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Old Title",
                Description = "Old Description",
                IsCompleted = false,
                ProjectId = project.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Tasks.AddAsync(task);

            await _context.SaveChangesAsync();

            var dto = new TaskCreateDto
            {
                Title = "New Title",
                Description = "New Description",
                IsCompleted = true,
                ProjectId = project.Id
            };

            // Act

            var result = await _tasksService
                .PutTaskAsync(dto, task.Id);

            // Assert

            Assert.Equal(UpdateTaskResult.Success, result);

            var updatedTask = await _context.Tasks
                .FirstOrDefaultAsync(x => x.Id == task.Id);

            Assert.NotNull(updatedTask);

            Assert.Equal(dto.Title, updatedTask.Title);

            Assert.Equal(dto.Description, updatedTask.Description);

            Assert.Equal(dto.IsCompleted, updatedTask.IsCompleted);
        }
    }
}
