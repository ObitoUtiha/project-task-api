using Microsoft.AspNetCore.Mvc;
using ProjectTaskApi.DTOs;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var project = await _projectService
                .GetProjectDetails(id);

            if (project == null)
                return NotFound();
            else
                return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectDto dto)
        {
            var createdProject = await _projectService.CreateProjectAsync(dto);

            return CreatedAtAction(
                nameof(GetProjectById),
                new { id = createdProject.Id },
                createdProject);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            if(!result)
                return NotFound();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(CreateProjectDto dto, Guid id)
        {
            var result = await _projectService.PutProjectAsync(dto, id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
