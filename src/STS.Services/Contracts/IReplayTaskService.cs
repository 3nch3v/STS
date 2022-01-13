using System.Threading.Tasks;

using STS.Data.Dtos.Task;

namespace STS.Services.Contracts
{
    public interface IReplayTaskService
    {
        Task<ReplayTaskDto> ReplayTaskAsync<T>(string userId, T replay);
    }
}
