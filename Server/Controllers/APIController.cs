using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profile.Shared.Models.Codgram;

namespace Profile.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly CodgramContext _context;

        public APIController(CodgramContext context)
        {
            _context = context;
        }

        // GET: api/API
        [HttpGet]
        public async Task<ActionResult<IEnumerable<API>>> GetAPI()
        {
            return await _context.API.ToListAsync();
        }

        // GET: api/API/5
        [HttpGet("{id}")]
        public async Task<ActionResult<API>> GetAPI(string id)
        {
            var aPI = await _context.API.FindAsync(id);

            if (aPI == null)
            {
                return NotFound();
            }

            return aPI;
        }

        // PUT: api/API/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAPI(string id, API aPI)
        {
            if (id != aPI.APIId)
            {
                return BadRequest();
            }

            _context.Entry(aPI).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!APIExists(id))
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

        // POST: api/API
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<API>> PostAPI(API aPI)
        {
            _context.API.Add(aPI);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAPI", new { id = aPI.APIId }, aPI);
        }

        // DELETE: api/API/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAPI(string id)
        {
            var aPI = await _context.API.FindAsync(id);
            if (aPI == null)
            {
                return NotFound();
            }

            _context.API.Remove(aPI);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool APIExists(string id)
        {
            return _context.API.Any(e => e.APIId == id);
        }
    }
}
