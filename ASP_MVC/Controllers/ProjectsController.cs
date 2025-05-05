using Business.Services;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

public class ProjectsController(IProjectService projectService) : Controller
    {
    private readonly IProjectService _projectService = projectService;

    public async Task<IActionResult> Index()
        {
        var model = new ProjectsViewModel
            {
            Projects = (IEnumerable<ProjectViewModel>)await _projectService.GetProjectAsync()
            };
        return View(model);
        }

    [HttpPost]
    public async Task<IActionResult> Add(AddProjectViewModel model)
        {
        var addProjectFormData = model.MapTo<AddProjectFormData>();

        var result = await _projectService.CreateProjectAsync(addProjectFormData);

        return Json(new { });
        }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateProjectViewModel model)
        {
        var updateProjectFormData = model.MapTo<UpdateProjectFormData>();
        var result = await _projectService.UpdateProjectAsync(updateProjectFormData);

        return Json(new { });
        }
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
        {
        var result = await _projectService.DeleteProjectAsync(id);
        if (result.Success)
            {
            return Json(new { });
            }
        return Json(new { });
        }
    }


