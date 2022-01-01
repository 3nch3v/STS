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
using STS.Web.ViewModels.User;
using STS.Web.ViewModels.Common;

using static STS.Common.GlobalConstants;

namespace STS.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService ticketService;
        private readonly ICommonService commonService;
        private readonly IAdminService adminService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public TicketsController(
            ITicketService ticketService,
            ICommonService commonService,
            IAdminService userService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.ticketService = ticketService;
            this.commonService = commonService;
            this.adminService = userService;
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
            var departmentId = commonService.GetDepartmentId(userId);

            var ticketDto = mapper.Map<TicketViewModel>(ticket);
            ticketDto.Statuses = mapper.Map<List<StatusViewModel>>(commonService.GetStatuses());
            ticketDto.Employees = mapper.Map<List<BaseUserViewModel>>(commonService.GetEmployeesBase(departmentId));
            ticketDto.Departments = mapper.Map<List<DepartmentViewModel>>(commonService.GetDepartmentsBase());
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
                Tickets = mapper.Map<List<BaseTicketViewModel>>(tickets),
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

            await ticketService.CreateAsync(userId, ticket);

            return RedirectToAction(nameof(Tickets));
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

            await ticketService.DeleteAsync(id);

            return RedirectToAction(nameof(Tickets));
        }
    }
}
