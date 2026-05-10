using Microsoft.AspNetCore.Mvc;
using ProjectTaskApi.Services;

namespace ProjectTaskApi.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(bool? status, Guid? projectId)
        {
            var tasks = await _tasksService.GetTasksAsync(status, projectId);
            return Ok(tasks);
        }
    }
}
