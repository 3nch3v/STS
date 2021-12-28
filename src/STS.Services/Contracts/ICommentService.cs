using STS.Data.Models;
using System.Threading.Tasks;

namespace STS.Services.Contracts
{
    public interface ICommentService
    {
        Task<Comment> Create<T>(T commentDto, string userId);

        Task<bool> Delete(int id);

        Comment GetById(int id);
    }
}
