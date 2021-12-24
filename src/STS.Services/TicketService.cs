using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using STS.Data;
using STS.Data.Models;
using STS.Services.Contracts;

namespace STS.Services
{
    public class TicketService : ITicketService
    {
        private static string[] ticketsNavCategories = { "my", "to me", "answers", "new", "all", "history" };

        private readonly IUserService userService;
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public TicketService(
            IUserService userService,
            ICommonService commonService,
            IMapper mapper,
            ApplicationDbContext dbContext)
        {
            this.userService = userService;
            this.commonService = commonService;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public Ticket GetById(int ticketId)
        {
            return dbContext.Tickets
                .Where(x => x.Id == ticketId)
                .FirstOrDefault();
        }

        public IEnumerable<Ticket> GetAll(string userId, int page, int ticketsPerPage, string keyword)
        {
            Func<Ticket, bool> filter = GetFilter(userId, keyword);

            return dbContext.Tickets
                .Where(filter)
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.PriorityId)
                .Skip((page - 1) * ticketsPerPage)
                .Take(ticketsPerPage)
                .ToList();
        }                                  

        public async Task CreateAsync<T>(string userId, T ticketDto)
        {
            var ticket = mapper.Map<Ticket>(ticketDto);
            ticket.EmployeeId = userId;
            ticket.StatusId = commonService.GetStatusId("Open");
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

        public Dictionary<string, int> GetCategoriesTicketsCount(string userId)
        {
            var statistic = new Dictionary<string, int>();

            foreach (var category in ticketsNavCategories)
            {
                var filter = GetFilter(userId, category);
                var count = GetCount(filter);
                statistic.Add(category, count);
            }

            return statistic;
        }

        public int GetTicketsCount(string userId, string keyword)
        {
            var filter = GetFilter(userId, keyword);
            return GetCount(filter);
        }

        public int GetCount(Func<Ticket, bool> filter)
        {
            return dbContext.Tickets
                .Where(filter)
                .Count();
        }

        private Func<Ticket, bool> GetFilter(string userId, string keyword)
        {
            var userDepartmentId = userService.GetDepartmentId(userId);

            if (keyword == "my")
            {
                Func<Ticket, bool> createdByUser = ticket => ticket.EmployeeId == userId
                                                          && ticket.Status.Name.ToLower() != "closed"
                                                          && ticket.Status.Name.ToLower() != "solved"
                                                          && ticket.IsDeleted == false;
                return createdByUser;
            }

            if (keyword == "to me")
            {
                Func<Ticket, bool> assignedToUser = ticket => (ticket.DepartmentId == userDepartmentId
                                                                && ticket.AssignedToId == userId)
                                                           && (ticket.Status.Name.ToLower() != "closed"
                                                              || ticket.Status.Name.ToLower() != "solved")
                                                           && ticket.IsDeleted == false;
                return assignedToUser;
            }

            if (keyword == "answers")
            {
                Func<Ticket, bool> newAnswers = ticket => ((ticket.DepartmentId == userDepartmentId
                                                                && ticket.AssignedToId == userId) 
                                                          || ticket.EmployeeId == userId)
                                                       && ticket.Status.Name.ToLower() == "open"
                                                       && (ticket.Status.Name.ToLower() != "closed"
                                                          || ticket.Status.Name.ToLower() != "solved")
                                                       && ticket.IsDeleted == false;
                return newAnswers;
            }

            if (keyword == "new")
            {
                Func<Ticket, bool> newTickets = ticket => ticket.DepartmentId == userDepartmentId
                                                       && ticket.Status.Name.ToLower() == "open"
                                                       && (ticket.Status.Name.ToLower() != "closed"
                                                          || ticket.Status.Name.ToLower() != "solved")
                                                       && ticket.IsDeleted == false;
                return newTickets;
            }

            if (keyword == "history")
            {
                Func<Ticket, bool> solved = ticket => ticket.DepartmentId == userDepartmentId
                                                   && (ticket.Status.Name.ToLower() == "closed"
                                                      || ticket.Status.Name.ToLower() == "solved")
                                                   && ticket.IsDeleted == false;
                return solved;
            }

            if (keyword == "all" || keyword == null) 
            {
                Func<Ticket, bool> all = ticket => ticket.DepartmentId == userDepartmentId
                                            && (ticket.Status.Name.ToLower() != "closed"
                                                ||ticket.Status.Name.ToLower() != "solved")
                                            && ticket.IsDeleted == false;
                return all;
            }

            Func<Ticket, bool> searchedTickets = ticket => ticket.DepartmentId == userDepartmentId
                                                        && (ticket.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                                            || ticket.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                                            || ticket.Employee.FirstName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                                            || ticket.Employee.LastName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                                            || ticket.Employee.UserName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                                        && ticket.IsDeleted == false;
            return searchedTickets;
        }
    }
}
