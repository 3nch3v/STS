﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using STS.Data;
using STS.Data.Models;
using STS.Services.Contracts;

namespace STS.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public TicketService(
            IUserService userService,
            IMapper mapper,
            ApplicationDbContext dbContext)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public Ticket GetById(int ticketId) 
        {
            return dbContext.Tickets
                .Where(x => x.Id == ticketId)
                .FirstOrDefault();
        }

        public IEnumerable<Ticket> GetAll(string userId)
        {
            var userDepartmentId = userService.GetDepartmentId(userId);

            return dbContext.Tickets
                .Where(x => x.DepartmentId == userDepartmentId)
                .ToList();
        }

        public async Task CreateAsync<T>(string userId, T ticketDto) 
        {
            var ticket = mapper.Map<Ticket>(ticketDto);

            ticket.EmployeeId = userId;

            await dbContext.Tickets.AddAsync(ticket);

            await dbContext.SaveChangesAsync();
        }

        public async Task EditAsync<T>(int ticketId, T ticketDto)
        {
            var ticket = dbContext.Tickets
                .Where(x => x.Id == ticketId)
                .FirstOrDefault();

            var ticketInput = mapper.Map<Ticket>(ticketDto);

            if (ticket.Title != ticketInput.Title)
            {
                ticket.Title = ticketInput.Title;
            }
            if (ticket.Content != ticketInput.Content)
            {
                ticket.Content = ticketInput.Content;
            }
            if (ticket.Status != ticketInput.Status)
            {
                ticket.Status = ticketInput.Status;
            }
            if (ticket.Priority != ticketInput.Priority)
            {
                ticket.Priority = ticketInput.Priority;
            }
            if (ticket.AssignedTo != ticketInput.AssignedTo)
            {
                ticket.AssignedTo = ticketInput.AssignedTo;
            }

            await dbContext.Tickets.AddAsync(ticketInput);

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int ticketId)
        {
            var ticket = dbContext.Tickets
               .Where(x => x.Id == ticketId)
               .FirstOrDefault();

            dbContext.Tickets.Remove(ticket);

            await dbContext.SaveChangesAsync();
        }
    }
}