using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

using STS.Services.Contracts;
using STS.Web.ViewModels.Tickets;

namespace STS.Web.Areas.Api.Controllers
{

    [Route("api/tickets")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService ticketService;
        private readonly IMapper mapper;

        public TicketsController(ITicketService ticketService, IMapper mapper)
        {
            this.ticketService = ticketService;
            this.mapper = mapper;
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
