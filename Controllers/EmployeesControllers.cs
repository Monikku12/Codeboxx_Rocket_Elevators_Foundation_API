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
    public class EmployeesController : ControllerBase
    {
        private readonly RocketElevatorsContext _context;

        public EmployeesController(RocketElevatorsContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        // {
        //     if (_context.employees == null)
        //     {
        //         return NotFound();
        //     }
        //     return await _context.employees.ToListAsync();
        // }

        // GET: api/Employees/(id#)
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetUser(long id)
        {
            if (_context.employees == null)
            {
                return NotFound();
            }
            var employee = await _context.employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // GET: api/Employees?email=(email)
        public async Task<ActionResult<Employee>> GetEmployee(string email)
        {
            if (_context.employees == null)
            {
                return NotFound();
            }
            var employee = await _context.employees.FirstOrDefaultAsync(x => x.Email == email);

            if (employee == null)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        // PUT: api/Employees/(id#)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostUser(Employee employee)
        {
            if (_context.employees == null)
            {
                return Problem("Entity set 'RocketElevatorsContext.employees'  is null.");
            }
            _context.employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/(id#)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            if (_context.employees == null)
            {
                return NotFound();
            }
            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return (_context.employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
