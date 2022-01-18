using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

using STS.Services.Contracts;
using STS.Web.ViewModels.Tickets;
using STS.Data.Models;

namespace STS.Web.Areas.Api.Controllers
{

    [Route("api/tickets")]
    [ApiController]
    [Authorize]
    public class TicketsApiController : ControllerBase
    {
        private readonly ITicketService ticketService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public TicketsApiController(
            ITicketService ticketService, 
            IMapper mapper, 
            UserManager<ApplicationUser> userManager)
        {
            this.ticketService = ticketService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpPut]
        public async Task<ActionResult<TicketViewModel>> Edit([FromBody] TicketEditModel ticketDto)
        {
            var ticket = ticketService.GetById(ticketDto.Id);

            if (ticket == null)
            {
                return NotFound();
            }

            var userId = userManager.GetUserId(User);

            if (ticket.EmployeeId != userId
               && (ticketDto.Title != null 
                  || ticketDto.Content != null))
            {
                return Unauthorized();
            }

            var result = await ticketService.EditAsync(ticketDto.Id, ticketDto);
            var response = mapper.Map<TicketViewModel>(result);

            return Ok(response);
        }
    }
}
