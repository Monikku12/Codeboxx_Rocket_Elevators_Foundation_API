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
    public class ColumnsController : ControllerBase
    {
        private readonly RocketElevatorsContext _context;

        public ColumnsController(RocketElevatorsContext context)
        {
            _context = context;
        }

        // End point to get a list of all columns
        // GET: api/Columns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Column>>> Getcolumns()
        {
          if (_context.columns == null)
          {
              return NotFound();
          }
            return await _context.columns.ToListAsync();
        }

        // End point to get a specific ID from the list of columns
        // GET: api/Columns/(id#) - All Data
        [HttpGet("{id}")]
        public async Task<ActionResult<Column>> GetColumn(long id)
        {
          if (_context.columns == null)
          {
              return NotFound();
          }
            var column = await _context.columns.FindAsync(id);

            if (column == null)
            {
                return NotFound();
            }

            return column;
        }

        // End point to get a specific status from a specific elevator from the list
        // GET: api/Columns/(id#) - Status
        [HttpGet("status/{id}")]
        public async Task<ActionResult<string>> GetColumnStatus(long id)
        {
          if (_context.columns == null)
          {
              return NotFound();
          }
            var column = await _context.columns.FindAsync(id);
            var status = column.Status;
            
            if (column == null)
            {
                return NotFound();
            }

            return status;
        }

        // PUT: api/Columns/(id#)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColumn(long id, Column column)
        {
            if (id != column.Id)
            {
                return BadRequest();
            }

            _context.Entry(column).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColumnExists(id))
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

        // End point to change the status of a specific column
        // PUT: api/Columns/status/(id#) - Change Status
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("status/{id}")]
        public async Task<ActionResult<string>> PutColumn(long id)
        {
            // list of the possible status:
            // ・ Active
            // ・ Inactive
            // ・ Intervention

            var column = await _context.columns.FindAsync(id);

            // Add the new status after the equal sign (=) between the quote marks (" ").
            column.Status = "Intervention";

            var newStatus = column.Status;

            if (id != column.Id)
            {
                return BadRequest();
            }

            _context.Entry(column).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColumnExists(id))
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



        // POST: api/Columns
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Column>> PostColumn(Column column)
        {
          if (_context.columns == null)
          {
              return Problem("Entity set 'RocketElevatorsContext.columns'  is null.");
          }
            _context.columns.Add(column);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetColumn", new { id = column.Id }, column);
        }

        // DELETE: api/Columns/(id#)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColumn(long id)
        {
            if (_context.columns == null)
            {
                return NotFound();
            }
            var column = await _context.columns.FindAsync(id);
            if (column == null)
            {
                return NotFound();
            }

            _context.columns.Remove(column);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ColumnExists(long id)
        {
            return (_context.columns?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
