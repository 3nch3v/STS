using System.Collections.Generic;
using System.Threading.Tasks;
using STS.Data.Models;

namespace STS.Services.Contracts
{
    public interface ITicketService
    {
        int GetTicketsCount(string userId);

        Ticket GetById(int ticketId);

        IEnumerable<Ticket> GetAll(string userId, int page, int ticketsPerPage, string keyword);

        Task CreateAsync<T>(string userId, T ticketDto);

        Task EditAsync<T>(int ticketId, T ticketDto);

        Task DeleteAsync(int ticketId);
    }
}
