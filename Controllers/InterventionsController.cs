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
    public class InterventionsController : ControllerBase
    {
        private readonly RocketElevatorsContext _context;
        public InterventionsController(RocketElevatorsContext context)
        {
            _context = context;
        }
        // End point to get a list of all interventions
        // GET: api/Interventions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Intervention>>> Getinterventions()
        {
          if (_context.interventions == null)
          {
              return NotFound();
          }
            return await _context.interventions.ToListAsync();
        }
        // End point to get a specific ID from the list of interventions
        // GET: api/Interventions/(id#) - All data
        [HttpGet("{id}")]
        public async Task<ActionResult<Intervention>> GetIntervention(long id)
        {
          if (_context.interventions == null)
          {
              return NotFound();
          }
            var intervention = await _context.interventions.FindAsync(id);
            
            if (intervention == null)
            {
                return NotFound();
            }          
            return intervention;
        }
        // End point to get a specific status from a specific intervention from the list
        // GET: api/Interventions/(id#) - Status
        [HttpGet("status/{id}")]
        public async Task<ActionResult<string>> GetInterventionStatus(long id)
        {
          if (_context.interventions == null)
          {
              return NotFound();
          }
            var intervention = await _context.interventions.FindAsync(id);
            var status = intervention.Status; 
            if (intervention == null)
            {
                return NotFound();
            }
            return status;
        }
        // End point to get a list of not started and pending intervention at the time of a request
        // GET: api/Interventions/list - Not started and pending interventions list
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Intervention>>> GetNotStartedPendingIntervention()
        {
          if (_context.interventions == null)
          {
              return NotFound();
          }
            var interventionlist = await _context.interventions.ToListAsync();
            var newinterventionlist = new List<Intervention>();
            for (int i = 0; i < interventionlist.Count; i++)
            {
                if (interventionlist[i].Intervention_started_at == null && interventionlist[i].Status == "Pending")
                    newinterventionlist.Add(interventionlist[i]);
            }
            return newinterventionlist;
        }
        // End point to change the status and add the start date of a specific intervention
        // PUT: api/Interventions/status/(id#) - Change Status and add start date.
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("status/{id}")]
        public async Task<ActionResult<Intervention>> PutInterventionStatusAndStartDate(long id)
        {
            var intervention = await _context.interventions.FindAsync(id);
            intervention.Status = "InProgress";
            intervention.Intervention_started_at = DateTime.Now;
            
            var newStatus = intervention.Status;
            var startDate = intervention.Intervention_started_at;
            if (id != intervention.Id)
            {
                return BadRequest();
            }
            _context.Entry(intervention).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterventionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return intervention;
        }
         // End point to change the status and add the start date of a specific intervention
        // PUT: api/Interventions/result/(id#) - Change Status and add start date.
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("result/{id}")]
        public async Task<ActionResult<Intervention>> PutInterventionResultAndEndDate(long id)
        {
            var intervention = await _context.interventions.FindAsync(id);
            intervention.Result = "Completed";
            intervention.Intervention_ended_at = DateTime.Now;
            
            var newStatus = intervention.Result;
            var endDate = intervention.Intervention_ended_at;
            if (id != intervention.Id)
            {
                return BadRequest();
            }
            _context.Entry(intervention).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterventionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return intervention;
        }
        // POST: api/Interventions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Intervention>> PostIntervention(Intervention intervention)
        {
          if (_context.interventions == null)
          {
              return Problem("Entity set 'RocketInterventionsContext.interventions'  is null.");
          }
            _context.interventions.Add(intervention);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetIntervention", new { id = intervention.Id }, intervention);
        }
        // DELETE: api/Interventions/(id#)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntervention(long id)
        {
            if (_context.interventions == null)
            {
                return NotFound();
            }
            var intervention = await _context.interventions.FindAsync(id);
            if (intervention == null)
            {
                return NotFound();
            }
            _context.interventions.Remove(intervention);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool InterventionExists(long id)
        {
            return (_context.interventions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}