using STS.Data.Models;
using System.Threading.Tasks;

namespace STS.Services.Contracts
{
    public interface ICommentService
    {
        Task<Comment> CreateAsync<T>(T commentDto, string userId);

        Task<bool> DeleteAsync(int id);

        Comment GetById(int id);
    }
}
