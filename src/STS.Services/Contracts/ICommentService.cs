using STS.Data.Models;
using System.Threading.Tasks;

namespace STS.Services.Contracts
{
    public interface ICommentService
    {
        Task<Comment> CreateAsync<T>(T commentDto, string userId);

        Task<bool> DeleteAsync(int id);

        Task SendEmailAsync(int ticketId, string content, ApplicationUser sender);

        Comment GetById(int id);
    }
}
