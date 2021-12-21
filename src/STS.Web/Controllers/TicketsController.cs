using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tickets;

namespace STS.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService ticketService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public TicketsController(
            ITicketService ticketService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.ticketService = ticketService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Ticket(int id)
        {
            var ticket = ticketService.GetById(id);

            var ticketDto = mapper.Map<TicketViewModel>(ticket);

            return View(ticketDto);
        }

        [HttpGet]
        public IActionResult Tickets()
        {
            var userId = userManager.GetUserId(User);

            var tickets = ticketService.GetAll(userId);

            var ticketsDtos = new TicketsViewModel
            {
                Tickets = mapper.Map<List<TicketViewModel>>(tickets),
            };

            return View(ticketsDtos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketInputModel ticket)
        {
            if(!ModelState.IsValid)
            {
                return View(ticket);
            }

            var userId = userManager.GetUserId(User);

            await ticketService.CreateAsync(userId, ticket);

            return RedirectToAction(nameof(Tickets));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ticket = ticketService.GetById(id);
            var userId = userManager.GetUserId(User);

            if (ticket.EmployeeId != userId || ticket == null)
            {
                return BadRequest();
            }

            var ticketDto = mapper.Map<TicketViewModel>(ticket);

            return View(ticketDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TicketInputModel ticketInput)
        {
            var ticket = ticketService.GetById(id);
            var userId = userManager.GetUserId(User);

            if (ticket.EmployeeId != userId || ticket == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(ticketInput);
            }

            await ticketService.EditAsync(id, ticketInput);

            return RedirectToAction(nameof(Tickets));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = ticketService.GetById(id);
            var userId = userManager.GetUserId(User);

            if (ticket.EmployeeId != userId || ticket == null)
            {
                return BadRequest();
            }

            await ticketService.DeleteAsync(id);

            return RedirectToAction(nameof(Tickets));
        }
    }
}
