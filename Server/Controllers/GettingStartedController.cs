using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profile.Server.Data;
using Profile.Shared.Models;

namespace Profile.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GettingStartedController : ControllerBase
    {
        private readonly ProfileContext _context;
        private UserManager<ProfileUser> _userManager;

        public GettingStartedController(ProfileContext context, UserManager<ProfileUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/GettingStarted
        [HttpGet]
        public async Task<ActionResult<GettingStarted>> GetGettingStarted()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.GettingStarted.FirstOrDefaultAsync(g => g.ProfileUserId == userId);
        }

        // GET: api/GettingStarted/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GettingStarted>> GetGettingStarted(string id)
        {
            var gettingStarted = await _context.GettingStarted.FindAsync(id);

            if (gettingStarted == null)
            {
                return NotFound();
            }

            return gettingStarted;
        }

        // PUT: api/GettingStarted/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGettingStarted(string id, GettingStarted gettingStarted)
        {
            if (id != gettingStarted.GettingStartedId)
            {
                return BadRequest();
            }

            _context.Entry(gettingStarted).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GettingStartedExists(id))
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

        // POST: api/GettingStarted
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GettingStarted>> PostGettingStarted(GettingStarted gettingStarted)
        {
            _context.GettingStarted.Add(gettingStarted);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGettingStarted", new { id = gettingStarted.GettingStartedId }, gettingStarted);
        }

        // DELETE: api/GettingStarted/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGettingStarted(string id)
        {
            var gettingStarted = await _context.GettingStarted.FindAsync(id);
            if (gettingStarted == null)
            {
                return NotFound();
            }

            _context.GettingStarted.Remove(gettingStarted);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GettingStartedExists(string id)
        {
            return _context.GettingStarted.Any(e => e.GettingStartedId == id);
        }
    }
}
