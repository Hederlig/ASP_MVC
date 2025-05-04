using Domain.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Business.Models;

namespace Business.Services;

public interface IProjectService
    {
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
    Task<ProjectResult<IEnumerable<Project>>> GetProjectAsync();
    Task<ProjectResult<Project>> GetProjectAsync(string id);
    }

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) : IProjectService
    {
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;

    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
        {
        if (formData == null)
            {
            return new ProjectResult
                {
                StatusCode = 400,
                Success = false,
                Error = "Not all requierd field are supplied."
                };
            }
        var projectEntity = formData.MapTo<ProjectEntity>();
        var statusResult = await _statusService.GetStatusByIdAsync(1);
        var status = statusResult.Result;

        projectEntity.StatusId = status!.Id;


        var result = await _projectRepository.AddAsync(projectEntity);
        return result.Success
            ? new ProjectResult
                {
                StatusCode = 201,
                Success = true,
                }
            : new ProjectResult
                {
                StatusCode = result.StatusCode,
                Success = false,
                Error = result.Error
                };

        }





    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectAsync()
        {
        var response = await _projectRepository.GetAllAsync
            (
                orderByDescending: true,
                sortBy: s => s.Created,
                where: null,
                include => include.User,
                include => include.Status,
                include => include.Client
            );

        return new ProjectResult<IEnumerable<Project>>
            {
            StatusCode = 200,
            Success = true,
            Result = response.Result
            };
        }

    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
        {
        var response = await _projectRepository.GetAsync
            (where: x => x.Id == id,
                include => include.User,
                include => include.Status,
                include => include.Client
            );

        return response.Success
            ? new ProjectResult<Project>
                {
                StatusCode = 200,
                Success = true,
                Result = response.Result
                }
            : new ProjectResult<Project>
                {
                StatusCode = 404,
                Success = false,
                Error = $"Project'{id}' was not found"
                };
        }
    }
    