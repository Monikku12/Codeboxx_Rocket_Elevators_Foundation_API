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
    public class BatteriesController : ControllerBase
    {
        private readonly RocketElevatorsContext _context;

        public BatteriesController(RocketElevatorsContext context)
        {
            _context = context;
        }

        // End point to get a list of all batteries
        // GET: api/Batteries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Battery>>> Getbatteries()
        {
          if (_context.batteries == null)
          {
              return NotFound();
          }
            return await _context.batteries.ToListAsync();
        }

        // End point to get a specific ID from the list of batteries
        // GET: api/Batteries/(id#) - All data
        [HttpGet("{id}")]
        public async Task<ActionResult<Battery>> GetBattery(long id)
        {
          if (_context.batteries == null)
          {
              return NotFound();
          }
            var battery = await _context.batteries.FindAsync(id);

            if (battery == null)
            {
                return NotFound();
            }

            return battery;
        }

        // End point to get a specific status from a specific battery from the list
        // GET: api/Batteries/status/(id#) - Status
        [HttpGet("status/{id}")]
        public async Task<ActionResult<string>> GetBatteryStatus(long id)
        {
          if (_context.batteries == null)
          {
              return NotFound();
          }
            var battery = await _context.batteries.FindAsync(id);
            var status = battery.Status;

            if (battery == null)
            {
                return NotFound();
            }

            return status;
        }


        // End point to change the status of a specific battery
        // PUT: api/Batteries/status/(id#) - Change Status
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("status/{id}")]
        public async Task<ActionResult<string>> PutBatteryStatus(long id)
        {
            // list of the possible status:
            // ・ Active
            // ・ Inactive
            // ・ Intervention

            var battery = await _context.batteries.FindAsync(id);

            // Add the new status after the equal sign (=) between the quote marks (" ").
            battery.Status = "Inactive";

            var newStatus = battery.Status;

            if (id != battery.Id)
            {
                return BadRequest();
            }

            _context.Entry(battery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatteryExists(id))
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

        // POST: api/Batteries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Battery>> PostBattery(Battery battery)
        {
          if (_context.batteries == null)
          {
              return Problem("Entity set 'RocketElevatorsContext.batteries'  is null.");
          }
            _context.batteries.Add(battery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBattery", new { id = battery.Id }, battery);
        }

        // DELETE: api/Batteries/(id#)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBattery(long id)
        {
            if (_context.batteries == null)
            {
                return NotFound();
            }
            var battery = await _context.batteries.FindAsync(id);
            if (battery == null)
            {
                return NotFound();
            }

            _context.batteries.Remove(battery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BatteryExists(long id)
        {
            return (_context.batteries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
