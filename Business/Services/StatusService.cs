
using Domain.Models;
using Data.Repositories;
using Business.Models;

namespace Business.Services;

public interface IStatusService
    {
    Task<StatusResult<Status>> GetStatusByIdAsync(int id);
    Task<StatusResult<Status>> GetStatusByNameAsync(string statusName);
    Task<StatusResult<IEnumerable<Status>>> GetStatusesAsync();
    }

public class StatusService(IStatusRepository statusRepository) : IStatusService
    {
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<StatusResult<IEnumerable<Status>>> GetStatusesAsync()
        {
        var result = await _statusRepository.GetAllAsync();
        return result.Success
            ? new StatusResult<IEnumerable<Status>>
                {
                StatusCode = 200,
                Success = true,
                Result = result.Result
                }
            : new StatusResult<IEnumerable<Status>>
                {
                StatusCode = result.StatusCode,
                Success = false,
                Error = result.Error
                };
        }

    public async Task<StatusResult<Status>> GetStatusByNameAsync(string statusName)
        {
        var result = await _statusRepository.GetAsync(x => x.StatustName == statusName);
        return result.Success
    ? new StatusResult<Status>
        {
        StatusCode = 200,
        Success = true,
        Result = result.Result
        }
    : new StatusResult<Status>
        {
        StatusCode = result.StatusCode,
        Success = false,
        Error = result.Error
        };
        }

    public async Task<StatusResult<Status>> GetStatusByIdAsync(int id)
        {
        var result = await _statusRepository.GetAsync(x => x.Id == id);
        return result.Success
   ? new StatusResult<Status>
       {
       StatusCode = 200,
       Success = true,
       Result = result.Result
       }
   : new StatusResult<Status>
       {
       StatusCode = result.StatusCode,
       Success = false,
       Error = result.Error
       };
        }
    }

            
