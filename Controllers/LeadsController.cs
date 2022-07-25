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
    public class LeadsController : ControllerBase
    {
        private readonly RocketElevatorsContext _context;

        public LeadsController(RocketElevatorsContext context)
        {
            _context = context;
        }
        
        // End point to get all the leads with their infos
        // GET: api/Leads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lead>>> Getleads()
        {
          if (_context.leads == null)
          {
              return NotFound();
          }
            return await _context.leads.ToListAsync();
        }

        //End point to get a specific lead and its info
        // GET: api/Leads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lead>> GetLead(long id)
        {
          if (_context.leads == null)
          {
              return NotFound();
          }
            var lead = await _context.leads.FindAsync(id);

            if (lead == null)
            {
                return NotFound();
            }

            return lead;
        }

        // End point to get a list of leads created in the last 30 days and who are not customer
        // GET: api/Leads/creation
        [HttpGet("leadslist")]
        public async Task<ActionResult<IEnumerable<Lead>>> Getleadslist() 
        {
          if (_context.leads == null)
          {
              return NotFound();
          }

            return await _context.leads.Where(lead => lead.Lead_created_at >= DateTime.Now.AddDays(-30)).ToListAsync();
            
        }


        // PUT: api/Leads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLead(long id, Lead lead)
        {
            if (id != lead.Id)
            {
                return BadRequest();
            }

            _context.Entry(lead).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeadExists(id))
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

        // POST: api/Leads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lead>> PostLead(Lead lead)
        {
          if (_context.leads == null)
          {
              return Problem("Entity set 'RocketElevatorsContext.leads'  is null.");
          }
            _context.leads.Add(lead);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLead", new { id = lead.Id }, lead);
        }

        // DELETE: api/Leads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLead(long id)
        {
            if (_context.leads == null)
            {
                return NotFound();
            }
            var lead = await _context.leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }

            _context.leads.Remove(lead);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeadExists(long id)
        {
            return (_context.leads?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
