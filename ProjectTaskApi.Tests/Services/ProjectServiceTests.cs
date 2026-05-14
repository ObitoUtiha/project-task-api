using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectTaskApi.Data;
using ProjectTaskApi.DTOs;
using ProjectTaskApi.Entities;
using ProjectTaskApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTaskApi.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly ApplicationContext _context;

        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationContext(options);

            var loggerMock = new Mock<ILogger<ProjectService>>();

            var cacheMock = new Mock<IDistributedCache>();

            cacheMock.Setup(x => x.GetAsync(It.IsAny<string>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[]?)null);

            _projectService = new ProjectService(_context,loggerMock.Object,cacheMock.Object);
        }

        [Fact]
        public async Task CreateProjectAsync_ShouldCreateProject()
        {
            // Arrange

            var dto = new CreateProjectDto
            {
                Name = "Test Project",
                Description = "Test Description"
            };

            // Act

            var result = await _projectService
                .CreateProjectAsync(dto);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(dto.Name, result.Name);

            Assert.Equal(dto.Description, result.Description);

            Assert.Single(_context.Projects);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldDeleteProject()
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

            // Act

            var result = await _projectService
                .DeleteProjectAsync(project.Id);

            // Assert

            Assert.True(result);

            Assert.Empty(_context.Projects);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnFalse_WhenProjectNotFound()
        {
            // Arrange

            var fakeId = Guid.NewGuid();

            // Act

            var result = await _projectService
                .DeleteProjectAsync(fakeId);

            // Assert

            Assert.False(result);
        }
        [Fact]
        public async Task PutProjectAsync_ShouldUpdateProject()
        {
            // Arrange

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Old Name",
                Description = "Old Description",
                CreatedAt = DateTime.UtcNow
            };

            await _context.Projects.AddAsync(project);

            await _context.SaveChangesAsync();

            var dto = new CreateProjectDto
            {
                Name = "New Name",
                Description = "New Description"
            };

            // Act

            var result = await _projectService
                .PutProjectAsync(dto, project.Id);

            // Assert

            Assert.True(result);

            var updatedProject = await _context.Projects
                .FirstOrDefaultAsync(x => x.Id == project.Id);

            Assert.NotNull(updatedProject);

            Assert.Equal(dto.Name, updatedProject.Name);

            Assert.Equal(dto.Description, updatedProject.Description);
        }

        [Fact]
        public async Task GetProjectsAsync_ShouldReturnPaginatedProjects()
        {
            // Arrange

            var projects = new List<Project>();

            for (int i = 1; i <= 15; i++)
            {
                projects.Add(new Project
                {
                    Id = Guid.NewGuid(),
                    Name = $"Project {i}",
                    Description = $"Description {i}",
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.Projects.AddRangeAsync(projects);

            await _context.SaveChangesAsync();

            // Act

            var result = await _projectService.GetProjectsAsync(page: 2, pageSize: 5);

            // Assert

            Assert.Equal(5, result.Count);

            Assert.Equal("Project 6", result[0].Name);

            Assert.Equal("Project 10", result[^1].Name);
        }
    }
}
