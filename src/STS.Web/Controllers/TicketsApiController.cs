using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tickets;

namespace STS.Web.Controllers
{

    [Route("api/Tickets")]
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

            var result = await ticketService.EditAsync(ticketDto.Id, ticketDto);
            var response = mapper.Map<TicketViewModel>(result);

            return Ok(response);
        }
    }
}
