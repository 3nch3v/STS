using System.Collections.Generic;
using System.Threading.Tasks;

using STS.Data.Models;

namespace STS.Services.Contracts
{
    public interface ITicketService
    {
        int GetTicketsCount(string userId, string keyword);

        Ticket GetById(int ticketId);

        Dictionary<string, int> GetCategoriesTicketsCount(string userId);

        IEnumerable<Ticket> GetAll(string userId, int page, int ticketsPerPage, string keyword);

        Task CreateAsync<T>(string userId, T ticketDto);

        Task<Ticket> EditAsync<T>(int ticketId, T ticketDto);

        Task DeleteAsync(int ticketId);
    }
}
