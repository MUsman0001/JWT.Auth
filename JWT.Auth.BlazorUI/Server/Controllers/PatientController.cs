using JWT.Auth.BlazorUI.Server;
using JWT.Auth.BlazorUI.Server.data;
using JWT.Auth.BlazorUI.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWT.Auth.BlazorUI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly MyWorldDbContext _context;

        public PatientController(MyWorldDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Patient>> AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPatients), new { id = patient.Id }, patient);
        }

    }
}
