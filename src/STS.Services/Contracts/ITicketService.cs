﻿using System.Collections.Generic;
using System.Threading.Tasks;
using STS.Data.Models;

namespace STS.Services.Contracts
{
    public interface ITicketService
    {
        Ticket GetById(int ticketId);

        IEnumerable<Ticket> GetAll(string userId);

        Task CreateAsync<T>(string userId, T ticketDto);

        Task EditAsync<T>(int ticketId, T ticketDto);

        Task DeleteAsync(int ticketId);
    }
}