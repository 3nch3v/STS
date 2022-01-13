using System.Threading.Tasks;

using STS.Data.Models;

namespace STS.Services.Contracts
{
    public interface ICommentService
    {
        Comment GetById(int id);

        Task<Comment> CreateAsync<T>(T commentDto, string userId);

        Task<bool> DeleteAsync(int id);

        Task SendEmailAsync(int ticketId, string content, ApplicationUser sender);
    }
}
