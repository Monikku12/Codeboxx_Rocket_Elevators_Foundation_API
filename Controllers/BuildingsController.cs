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
    public class BuildingsController : ControllerBase
    {
        private readonly RocketElevatorsContext _context;

        public BuildingsController(RocketElevatorsContext context)
        {
            _context = context;
        }

        // End point to get a list of all buildings
        // GET: api/Buildings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Building>>> Getbuildings()
        {
          if (_context.buildings == null)
          {
              return NotFound();
          }
            return await _context.buildings.ToListAsync();
        }

        // End point to get a specific ID from the list of buildings
        // GET: api/Buildings/(id#)
        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetBuilding(long id)
        {
          if (_context.buildings == null)
          {
              return NotFound();
          }
            var building = await _context.buildings.FindAsync(id);

            if (building == null)
            {
                return NotFound();
            }

            return building;
        }

        // End point to get all the buildings with a requiring intervention status on their equipments
        // GET: api/Buildings/list - not operational elevators list
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Building>>> GetBuildingIntervention(long id)
        {
            if (_context.buildings == null)
            {
                return NotFound();
            }

                var buildinglist = await _context.buildings.ToListAsync();
                var batterylist = await _context.batteries.ToListAsync();
                var columnlist = await _context.columns.ToListAsync();
                var elevatorlist = await _context.elevators.ToListAsync();
                var newbuildinglist = new List<Building>();

            if (_context.buildings != null)
            {
                for (int i = 0; i < buildinglist.Count; i++)
                {
                    if (batterylist[i].Status is "Intervention")
                    {
                        newbuildinglist.Add(await _context.buildings.FindAsync(batterylist[i].Building_id));
                    }
                }
            
                for (int i = 0; i < batterylist.Count; i++)
                {
                    if (columnlist[i].Status is "Intervention")
                    {
                        newbuildinglist.Add(await _context.buildings.FindAsync((await _context.batteries.FindAsync(columnlist[i].Battery_id)).Building_id));
                    }

                }
           
                for (int i = 0; i < columnlist.Count; i++)
                {
                    if (elevatorlist[i].Status is "Intervention")
                    {
                        newbuildinglist.Add(await _context.buildings.FindAsync((await _context.batteries.FindAsync((await _context.columns.FindAsync(elevatorlist[i].Column_id)).Battery_id)).Building_id));
                    }
                }
            }

            if (newbuildinglist == null)
            {
                return NotFound();
            }

            return Ok(newbuildinglist.Distinct());
        }

        // PUT: api/Buildings/(id#)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuilding(long id, Building building)
        {
            if (id != building.Id)
            {
                return BadRequest();
            }

            _context.Entry(building).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Buildings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Building>> PostBuilding(Building building)
        {
          if (_context.buildings == null)
          {
              return Problem("Entity set 'RocketElevatorsContext.buildings'  is null.");
          }
            _context.buildings.Add(building);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBuilding", new { id = building.Id }, building);
        }

        // DELETE: api/Buildings/(id#)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuilding(long id)
        {
            if (_context.buildings == null)
            {
                return NotFound();
            }
            var building = await _context.buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }

            _context.buildings.Remove(building);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BuildingExists(long id)
        {
            return (_context.buildings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
