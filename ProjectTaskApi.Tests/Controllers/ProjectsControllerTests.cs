using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectTaskApi.Controllers;
using ProjectTaskApi.DTOs;
using ProjectTaskApi.Entities;
using ProjectTaskApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTaskApi.Tests.Controllers
{
    public class ProjectsControllerTests
    {
        [Fact]
        public async Task GetProjectById_ShouldReturnOk_WhenProjectExists()
        {
            // Arrange

            var projectId = Guid.NewGuid();

            var projectDto = new ProjectDetailsDto
            {
                Id = projectId,
                Name = "Test Project",
                Description = "Test Description"
            };

            var serviceMock = new Mock<IProjectService>();

            serviceMock.Setup(x => x.GetProjectDetails(projectId)).ReturnsAsync(projectDto);

            var controller = new ProjectsController(serviceMock.Object);

            // Act

            var result = await controller.GetProjectById(projectId);

            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnedProject = Assert.IsType<ProjectDetailsDto>(okResult.Value);

            Assert.Equal(projectId, returnedProject.Id);

            Assert.Equal("Test Project", returnedProject.Name);
        }

        [Fact]
        public async Task PostProject_ShouldReturnCreatedProjectDto()
        {
            // Arrenge

            var dto = new CreateProjectDto
            {
                Name = "Moq test post",
                Description = "Test Description"
            };


            var newProject = new ProjectGetDto
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Description = dto.Description,
                Name = dto.Name
            };

            var serviceMock = new Mock<IProjectService>();
            serviceMock.Setup(x => x.CreateProjectAsync(dto)).ReturnsAsync(newProject);
            var controller = new ProjectsController(serviceMock.Object);

            //Act

            var result = await controller.CreateProject(dto);

            //Assert

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(
                nameof(ProjectsController.GetProjectById),
                createdResult.ActionName);

        }

        [Fact]
        public async Task DeleteProject_ShouldReturnNoContent_WhenProjectDeleted()
        {
            // Arrange

            var serviceMock = new Mock<IProjectService>();

            serviceMock.Setup(x => x.DeleteProjectAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var controller = new ProjectsController(serviceMock.Object);

            // Act

            var result = await controller.DeleteProject(Guid.NewGuid());

            // Assert

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProject_ShouldReturnNotFound_WhenProjectDoesNotExist()
        {
            // Arrange

            var serviceMock = new Mock<IProjectService>();

            serviceMock.Setup(x => x.DeleteProjectAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = new ProjectsController(serviceMock.Object);

            // Act

            var result = await controller.DeleteProject(Guid.NewGuid());

            // Assert

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateProject_ShouldReturnCreatedAtAction()
        {
            // Arrange

            var dto = new CreateProjectDto
            {
                Name = "Test Project",
                Description = "Test Description"
            };

            var createdProject = new ProjectGetDto
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            var serviceMock = new Mock<IProjectService>();

            serviceMock.Setup(x => x.CreateProjectAsync(dto)).ReturnsAsync(createdProject);

            var controller = new ProjectsController(serviceMock.Object);

            // Act

            var result = await controller.CreateProject(dto);

            // Assert

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(
                nameof(ProjectsController.GetProjectById),
                createdResult.ActionName);
        }

        [Fact]
        public async Task PutProject_ShouldReturnNoContent_WhenProjectUpdated()
        {
            // Arrange

            var serviceMock = new Mock<IProjectService>();

            serviceMock.Setup(x => x.PutProjectAsync(It.IsAny<CreateProjectDto>(),It.IsAny<Guid>())).ReturnsAsync(true);

            var controller = new ProjectsController(serviceMock.Object);

            // Act

            var result = await controller.PutProject(new CreateProjectDto(), Guid.NewGuid());

            // Assert

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutProject_ShouldReturnNotFound_WhenProjectDoesNotExist()
        {
            // Arrange

            var serviceMock = new Mock<IProjectService>();

            serviceMock.Setup(x => x.PutProjectAsync(It.IsAny<CreateProjectDto>(),It.IsAny<Guid>())).ReturnsAsync(false);

            var controller =new ProjectsController(serviceMock.Object);

            // Act

            var result = await controller
                .PutProject(
                    new CreateProjectDto(),
                    Guid.NewGuid());

            // Assert

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
