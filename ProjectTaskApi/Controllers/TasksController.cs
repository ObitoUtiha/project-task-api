using Microsoft.AspNetCore.Mvc;
using ProjectTaskApi.Common.Results;
using ProjectTaskApi.DTOs.Tasks;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var task = await _tasksService.GetTaskById(id);
            if(task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> PostTask(TaskCreateDto task)
        {
            var newTask = await _tasksService.PostTaskAsync(task);
            if (newTask == null)
                return BadRequest("Project does not exist");
            return CreatedAtAction(
                nameof(GetTaskById),
                new { id = newTask.Id },
                newTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(Guid id, TaskCreateDto dto)
        {
            var result = await _tasksService.PutTaskAsync(dto, id);

            return result switch
            {
                UpdateTaskResult.TaskNotFound =>
                    NotFound("Task not found"),

                UpdateTaskResult.ProjectNotFound =>
                    BadRequest("Project does not exist"),

                UpdateTaskResult.Success =>
                    NoContent(),

                _ => StatusCode(500)
            };
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var result = await _tasksService.DeleteTaskAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
