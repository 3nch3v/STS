using STS.Data.Dtos.Task;
using System.Threading.Tasks;

namespace STS.Services.Contracts
{
    public interface IReplayTaskService
    {
        Task<ReplayTaskDto> ReplayTaskAsync<T>(string userId, T replay);
    }
}
