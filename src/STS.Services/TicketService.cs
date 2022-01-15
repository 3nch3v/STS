using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using AutoMapper;

using STS.Data;
using STS.Data.Dtos.Ticket;
using STS.Data.Models;
using STS.Services.Contracts;

namespace STS.Services
{
    public class TicketService : ITicketService
    {
        private static string[] ticketsNavCategories = { "my", "to me", "new", "all", "history" };
        private readonly IAdminService adminService;
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        private readonly StsDbContext dbContext;

        public TicketService(
            IAdminService adminService,
            ICommonService commonService,
            IMapper mapper,
            StsDbContext dbContext)
        {
            this.adminService = adminService;
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

        public IEnumerable<TicketListingDto> GetAll(string userId, int page, int ticketsPerPage, string keyword)
        {
            Func<Ticket, bool> filter = GetFilter(userId, keyword);

            return dbContext.Tickets
                .Where(filter)
                .Select(x => new TicketListingDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    PriorityName = x.Priority.Name,
                    StatusName = x.Status.Name,
                    AssignedToUserName = x.AssignedTo?.UserName,
                    CreatedOn = x.CreatedOn,
                })
                .OrderByDescending(x => x.CreatedOn)
                .Skip((page - 1) * ticketsPerPage)
                .Take(ticketsPerPage)
                .ToList();
        }

        public async Task CreateAsync<T>(string userId, T ticketDto)
        {
            var ticket = mapper.Map<Ticket>(ticketDto);
            ticket.EmployeeId = userId;
            ticket.StatusId = commonService.GetStatusId("Open");
            var assigneToUser = adminService.GetUserById(ticket.AssignedToId);

            if (assigneToUser == null) 
            {
                ticket.AssignedToId = null;
            }

            await dbContext.Tickets.AddAsync(ticket);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Ticket> EditAsync<T>(int ticketId, T ticketDto)
        {
            var dbTicket = GetById(ticketId);

            var ticketInput = mapper.Map<TicketDto>(ticketDto);

            if (ticketInput.Title != null
                && dbTicket.Title != ticketInput.Title)
            {
                dbTicket.Title = ticketInput.Title;
            }
            if (ticketInput.Content != null
                && dbTicket.Content != ticketInput.Content)
            {
                dbTicket.Content = ticketInput.Content;
            }
            if (ticketInput.AssignedToId != null
                && dbTicket.AssignedToId != ticketInput.AssignedToId)
            {
                dbTicket.AssignedToId = ticketInput.AssignedToId;
            }
            if (ticketInput.StatusId != null
                && dbTicket.StatusId != ticketInput.StatusId)
            {
                dbTicket.StatusId = (int)ticketInput.StatusId;
            }
            if (ticketInput.PriorityId != null
                && dbTicket.PriorityId != ticketInput.PriorityId)
            {
                dbTicket.PriorityId = (int)ticketInput.PriorityId;
            }
            if (ticketInput.DepartmentId != null
                && dbTicket.DepartmentId != ticketInput.DepartmentId)
            {
                if(dbTicket.StatusId != commonService.GetStatusId("New reply"))
                {
                    dbTicket.StatusId = commonService.GetStatusId("Open");
                }

                dbTicket.DepartmentId = (int)ticketInput.DepartmentId;
               
                if (commonService.GetDepartmentId(dbTicket.EmployeeId) == ticketInput.DepartmentId)
                {
                    dbTicket.AssignedToId = dbTicket.EmployeeId;
                }
                else 
                {
                    dbTicket.AssignedToId = null;
                    dbTicket.AssignedTo = null;
                }
            }

            await dbContext.SaveChangesAsync();
            var result = GetById(ticketId);

            return result;
        }

        public async Task DeleteAsync(int ticketId)
        {
            var ticket = GetById(ticketId);
            ticket.IsDeleted = true;

            var comments = dbContext.Comments
                .Where(x => x.TicketId == ticketId) 
                .ToList();

            comments.ForEach(c => c.IsDeleted = true);

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
            var userDepartmentId = commonService.GetDepartmentId(userId);

            if (keyword == "my")
            {
                Func<Ticket, bool> createdByUser = ticket => ticket.EmployeeId == userId;
                return createdByUser;
            }

            if (keyword == "to me")
            {
                Func<Ticket, bool> assignedToUser = ticket => (ticket.DepartmentId == userDepartmentId && ticket.AssignedToId == userId)
                                                           && (ticket.Status.Name.ToLower() != "closed" && ticket.Status.Name.ToLower() != "solved");
                return assignedToUser;
            }

            if (keyword == "new")
            {
                Func<Ticket, bool> newTickets = ticket => ticket.DepartmentId == userDepartmentId
                                                       && ticket.Status.Name.ToLower() == "open"
                                                       && ticket.AssignedToId == null
                                                       && (ticket.Status.Name.ToLower() != "closed"
                                                          || ticket.Status.Name.ToLower() != "solved");
                return newTickets;
            }

            if (keyword == "history")
            {
                Func<Ticket, bool> solved = ticket => ticket.DepartmentId == userDepartmentId
                                                   && (ticket.Status.Name.ToLower() == "closed"
                                                      || ticket.Status.Name.ToLower() == "solved");
                return solved;
            }

            if (keyword == "all" || keyword == null)
            {
                Func<Ticket, bool> all = ticket => ticket.DepartmentId == userDepartmentId
                                            && ticket.Status.Name.ToLower() != "closed"
                                            && ticket.Status.Name.ToLower() != "solved";
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
