using Microsoft.AspNetCore.Mvc;
using ProjectTaskApi.Services;

namespace ProjectTaskApi.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects(int page = 1, int pageSize = 10)
        {
            var projects = await _projectService
                .GetProjectsAsync(page, pageSize);

            return Ok(projects);
        }
    }
}
