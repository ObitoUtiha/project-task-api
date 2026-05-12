using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectTaskApi.Common.Results;
using ProjectTaskApi.Controllers;
using ProjectTaskApi.DTOs.Tasks;
using ProjectTaskApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTaskApi.Tests.Controllers
{
    public class TasksControllerTests
    {
        [Fact]
        public async Task GetTaskById_ShouldReturnOk_WhenTaskExists()
        {
            // Arrange

            var taskId = Guid.NewGuid();

            var taskDto = new TaskGetDto
            {
                Id = taskId,
                Title = "Test Task",
                IsCompleted = false
            };

            var serviceMock = new Mock<ITasksService>();

            serviceMock.Setup(x => x.GetTaskById(taskId)).ReturnsAsync(taskDto);

            var controller = new TasksController(serviceMock.Object);

            // Act

            var result = await controller.GetTaskById(taskId);

            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnedTask = Assert.IsType<TaskGetDto>(okResult.Value);

            Assert.Equal(taskId, returnedTask.Id);
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange

            var serviceMock = new Mock<ITasksService>();

            serviceMock.Setup(x => x.GetTaskById(It.IsAny<Guid>())).ReturnsAsync((TaskGetDto?)null);

            var controller = new TasksController(serviceMock.Object);

            // Act

            var result = await controller.GetTaskById(Guid.NewGuid());

            // Assert

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PostTask_ShouldReturnCreatedAtAction()
        {
            // Arrange

            var dto = new TaskCreateDto
            {
                Title = "Test Task",
                ProjectId = Guid.NewGuid()
            };

            var createdTask = new TaskGetDto
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                ProjectId = dto.ProjectId
            };

            var serviceMock = new Mock<ITasksService>();

            serviceMock.Setup(x => x.PostTaskAsync(dto)).ReturnsAsync(createdTask);

            var controller = new TasksController(serviceMock.Object);

            // Act

            var result = await controller.PostTask(dto);

            // Assert

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task PostTask_ShouldReturnBadRequest_WhenProjectDoesNotExist()
        {
            // Arrange

            var serviceMock = new Mock<ITasksService>();

            serviceMock.Setup(x => x.PostTaskAsync(It.IsAny<TaskCreateDto>())).ReturnsAsync((TaskGetDto?)null);

            var controller = new TasksController(serviceMock.Object);

            // Act

            var result = await controller.PostTask(new TaskCreateDto());

            // Assert

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnNoContent()
        {
            // Arrange

            var serviceMock = new Mock<ITasksService>();

            serviceMock.Setup(x => x.DeleteTaskAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var controller =new TasksController(serviceMock.Object);

            // Act

            var result = await controller.DeleteTask(Guid.NewGuid());

            // Assert

            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task PutTask_ShouldReturnNoContent_WhenTaskUpdated()
        {
            // Arrange

            var serviceMock =new Mock<ITasksService>();

            serviceMock.Setup(x => x.PutTaskAsync(It.IsAny<TaskCreateDto>(),It.IsAny<Guid>()))
                .ReturnsAsync(UpdateTaskResult.Success);

            var controller =new TasksController(serviceMock.Object);

            // Act

            var result = await controller.PutTask(Guid.NewGuid(), new TaskCreateDto());

            // Assert

            Assert.IsType<NoContentResult>(result);
        }
    }
}
