using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RocketElevators.Models;

namespace RocketElevators.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElevatorsController : ControllerBase
    {
        private readonly RocketElevatorsContext _context;

        public ElevatorsController(RocketElevatorsContext context)
        {
            _context = context;
        }

        // End point to get a list of all elevators
        // GET: api/Elevators
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Elevator>>> Getelevators()
        {
            if (_context.elevators == null)
            {
                return NotFound();
            }
            return await _context.elevators.ToListAsync();
        }

        // End point to get a specific ID from the list of elevators
        // GET: api/Elevators/(id#) - All data
        [HttpGet("{id}")]
        public async Task<ActionResult<Elevator>> GetElevator(long id)
        {
            if (_context.elevators == null)
            {
                return NotFound();
            }
            var elevator = await _context.elevators.FindAsync(id);
            
            if (elevator == null)
            {
                return NotFound();
            }          

            return elevator;
        }

        // End point to get a specific status from a specific elevator from the list
        // GET: api/Elevators/(id#) - Status
        [HttpGet("status/{id}")]
        public async Task<ActionResult<string>> GetElevatorStatus(long id)
        {
            if (_context.elevators == null)
            {
                return NotFound();
            }
            var elevator = await _context.elevators.FindAsync(id);
            var status = elevator.Status; 
            if (elevator == null)
            {
                return NotFound();
            }

            return status;
        }

        // End point to get a list of non-operational elevator at the time of a request
        // GET: api/Elevators/list - not operational elevators list
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Elevator>>> GetOperationalElevator()
        {
            if (_context.elevators == null)
            {
                return NotFound();
            }
            var elevatorlist = await _context.elevators.ToListAsync();
            var newelevatorlist = new List<Elevator>();

            for (int i = 0; i < elevatorlist.Count; i++)
            {
                if (elevatorlist[i].Status is not "Active")
                    newelevatorlist.Add(elevatorlist[i]);

            }

            return newelevatorlist;
        }

        // End point to change the status of a specific elevator
        // PUT: api/Elevators/status/(id#) - Change Status
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("status/{id}")]
        public async Task<ActionResult<string>> PutElevator(long id)
        {
            // list of the possible status:
            // ・ Active
            // ・ Inactive
            // ・ Intervention

            var elevator = await _context.elevators.FindAsync(id);

            // Add the new status after the equal sign (=) between the quote marks (" ").
            elevator.Status = "Active";
            
            var newStatus = elevator.Status;

            if (id != elevator.Id)
            {
                return BadRequest();
            }

            _context.Entry(elevator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElevatorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return newStatus;
        }

        // POST: api/Elevators
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Elevator>> PostElevator(Elevator elevator)
        {
            if (_context.elevators == null)
            {
                return Problem("Entity set 'RocketElevatorsContext.elevators'  is null.");
            }
            _context.elevators.Add(elevator);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetElevator", new { id = elevator.Id }, elevator);
        }

        // DELETE: api/Elevators/(id#)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteElevator(long id)
        {
            if (_context.elevators == null)
            {
                return NotFound();
            }
            var elevator = await _context.elevators.FindAsync(id);
            if (elevator == null)
            {
                return NotFound();
            }

            _context.elevators.Remove(elevator);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ElevatorExists(long id)
        {
            return (_context.elevators?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
