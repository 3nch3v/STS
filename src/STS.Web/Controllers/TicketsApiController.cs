using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
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
        [Authorize]
        public async Task<ActionResult<TicketViewModel>> Edit([FromBody] TicketEditModel ticketDto)
        {
            var ticket = ticketService.GetById(ticketDto.Id);

            if (ticket == null)
            {
                return NotFound();
            }

            var userId = userManager.GetUserId(User);

            if (!isRequestValid(ticketDto, userId, ticket.EmployeeId)) 
            {
                return StatusCode(
                   StatusCodes.Status422UnprocessableEntity,
                   "Comment could not be updated.");
            }

            try
            {
                var result = await ticketService.EditAsync(ticketDto.Id, ticketDto);
                var response = mapper.Map<TicketViewModel>(result);
                return Ok(response);
            }
            catch(Exception)
            {
                return StatusCode(
                   StatusCodes.Status500InternalServerError,
                   "Comment could not be updated.");
            }
        }

        private bool isRequestValid(TicketEditModel ticketDto, string userId, string ticketEmployeeId)
        {
            if (userId == null ||
                ((ticketDto.Title != null || ticketDto.Content != null)
                  && ticketEmployeeId != userId))
            {
                return false;
            }

            if (ticketDto.Title != null && 
                (ticketDto.Title.Length < 2 || ticketDto.Title.Length > 100))
            {
                return false;
            }

            if (ticketDto.Content != null &&
               (ticketDto.Content.Length < 5 || ticketDto.Content.Length > 2000))
            {
                return false;
            }

            return true;
        }
    }
}
