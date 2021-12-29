using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tickets;
using STS.Web.ViewModels.Common;

using static STS.Common.GlobalConstants;

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

            try
            {
                var userId = userManager.GetUserId(User);
                var departmentId = userService.GetDepartmentId(userId);

                var ticketDto = mapper.Map<TicketViewModel>(ticket);
                ticketDto.Statuses = mapper.Map<List<StatusViewModel>>(commonService.GetStatuses());
                ticketDto.Employees = mapper.Map<List<EmployeesViewModel>>(commonService.GetEmployees(departmentId));
                ticketDto.Departments = mapper.Map<List<DepartmentViewModel>>(commonService.GetDepartments());
                ticketDto.LoggedInUserId = userId;

                return View(ticketDto);
            }
            catch (Exception)
            {
                return StatusCode(
                  StatusCodes.Status500InternalServerError,
                  "Requested resource could not be delivered.");
            }
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
                return StatusCode(
                  StatusCodes.Status422UnprocessableEntity,
                  "Invalid input data.");
            }

            var userId = userManager.GetUserId(User);

            try
            {
                await ticketService.CreateAsync(userId, ticket);
                return RedirectToAction(nameof(Tickets));
            }
            catch (Exception)
            {
                return StatusCode(
                   StatusCodes.Status500InternalServerError,
                   "Ticket could not be created.");
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
