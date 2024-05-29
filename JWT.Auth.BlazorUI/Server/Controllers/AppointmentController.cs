using JWT.Auth.BlazorUI.Server;
using JWT.Auth.BlazorUI.Server.data;
using JWT.Auth.BlazorUI.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWT.Auth.BlazorUI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly MyWorldDbContext _context;

        public AppointmentController(MyWorldDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> AddAppointment(Appointment appointment)
        {
            try
            {
                // Ensure model state is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Add appointment to the context and save changes
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                // Return the newly created appointment
                return CreatedAtAction(nameof(GetAppointments), new { id = appointment.Id }, appointment);
            }
            catch (Exception ex)
            {
                // Return error response if something went wrong
                return StatusCode(500, $"Failed to add appointment: {ex.Message}");
            }
        }
    }
}
