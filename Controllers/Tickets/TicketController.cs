using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Sockets;
using Data.Models.Models.Response.Movie;
using Data.Models.Models.Request.Ticket;
using Business_Logic_Layer.TicketServices;

namespace MyShow.Controllers.Tickets
{
    [ApiController]
    [Route("api/v1.0/Tickets")]

  
        public class TicketController : ControllerBase
        {
            private readonly ITicketService _ticketService;

            public TicketController(ITicketService ticketService)
            {
                _ticketService = ticketService;
            }
            /// <summary>
            /// Booking Ticket Based On UserId
            /// </summary>
            /// <param name="ticket"></param>
            /// <returns></returns>
            
            [HttpPost("BookTicket")]
            public IActionResult BookTicket([FromBody] Ticket ticket)
        {
            try
            {
                if (ticket == null)
                {
                    return BadRequest("Ticket data is null.");
                }

                var BookingTicketID = _ticketService.BookTicket(ticket);

                // Correct action name here
                return Ok("Ticket booked successfully. BookingTicketID: "+ BookingTicketID);
               
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while booking the ticket: {ex.Message}");
            }
        }

              /// <summary>
              /// Update ticket users by ticket id
              /// </summary>
              /// <param name="updateTicket"></param>
              /// <returns></returns>
            [HttpPatch("Update/Ticket")]
            public IActionResult UpdateTicketStatus([FromQuery] TicketUpdate updateTicket)
            {
                try
                {
                    _ticketService.UpdateTicketStatus(updateTicket);
                    return Ok("Ticket status updated successfully");


                }
                catch (Exception ex)
                {
                    return BadRequest($"An error occurred while updating the ticket status: {ex.Message}");
                }
            }


            /// <summary>
            /// Endpoint for getting all tickets (can be restricted based on role as well)
            /// </summary>
            /// <returns></returns>
            [HttpPost("AllTickets")]
                public IActionResult GetAllTickets(GetTicket getTicket)
                {
                    try
                    {
                        var tickets = _ticketService.GetAllTickets(getTicket);
                        return Ok(tickets);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"An error occurred while retrieving the tickets: {ex.Message}");
                    }
                }
            }



    
}
