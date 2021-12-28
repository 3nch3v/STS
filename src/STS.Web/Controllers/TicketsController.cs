using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tickets;

using static STS.Common.GlobalConstants;
using STS.Web.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;

namespace STS.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService ticketService;
        private readonly ICommonService commonService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public TicketsController(
            ITicketService ticketService,
            ICommonService commonService,
            IUserService userService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.ticketService = ticketService;
            this.commonService = commonService;
            this.userService = userService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Ticket(int id)
        {
            var ticket = ticketService.GetById(id);

            if (ticket == null)
            {
                return NotFound();
            }

            var userId = userManager.GetUserId(User);
            var departmentId = userService.GetDepartmentId(userId);

            var ticketDto = mapper.Map<TicketViewModel>(ticket);
            ticketDto.Statuses = mapper.Map<List<StatusViewModel>>(commonService.GetStatuses());
            ticketDto.Employees = mapper.Map<List<EmployeesViewModel>>(commonService.GetEmployees(departmentId));
            ticketDto.Departments = mapper.Map<List<DepartmentViewModel>>(commonService.GetDepartments());
            ticketDto.LoggedInUserId = userId;

            return View(ticketDto);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Tickets(string keyword, int page = DefaultPageNumber)
        {
            this.TempData["keyword"] = keyword;
            var userId = userManager.GetUserId(User);
            var tickets = ticketService.GetAll(userId, page, TicketsPerPage, keyword);

            var ticketsDtos = new TicketsListViewModel
            {
                Page = page,
                TicketsCount = ticketService.GetTicketsCount(userId, keyword),
                Tickets = mapper.Map<List<TicketListViewModel>>(tickets),
            };

            return View(ticketsDtos);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(TicketInputModel ticket)
        {
            if(!ModelState.IsValid)
            {
                var inputDto = new TicketInputModel
                {
                    Title = ticket.Title,
                    Content = ticket.Content,
                    PriorityId = ticket.PriorityId,
                    AssignedToId = ticket.AssignedToId,
                    DepartmentId = ticket.DepartmentId,
                    Priorities = mapper.Map<List<PriorityViewModel>>(commonService.GetPriorities()),
                    Departments = mapper.Map<List<DepartmentViewModel>>(commonService.GetDepartments()),
                };

                return View(ticket);
            }

            var userId = userManager.GetUserId(User);

            if (userId == null) 
            {
                return Unauthorized();
            }

            try
            {
                await ticketService.CreateAsync(userId, ticket);
                return RedirectToAction(nameof(Tickets));
            }
            catch (Exception)
            {
                return StatusCode(
                   StatusCodes.Status500InternalServerError,
                   "Item could not be saved.");
            }
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = ticketService.GetById(id);

            if (ticket == null)
            { 
                return NotFound();
            }

            var userId = userManager.GetUserId(User);

            if (ticket.EmployeeId != userId)
            {
                return BadRequest();
            }

            try
            {
                await ticketService.DeleteAsync(id);
                return RedirectToAction(nameof(Tickets));
            }
            catch (Exception)
            {
                return StatusCode(
                   StatusCodes.Status500InternalServerError,
                   "Item could not be deleted.");
            }
        }
    }
}
