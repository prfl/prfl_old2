using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profile.Server.Data;
using Profile.Shared.Models;
using Profile.Shared.Models.Admin;

namespace Profile.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ProfileContext _context;

        public SubscriptionController(ProfileContext context)
        {
            _context = context;
        }

        // GET: api/Subscription
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscription()
        {
            return await _context.Subscription.ToListAsync();
        }

        // GET: api/Subscription/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscription(string id)
        {
            var subscription = await _context.Subscription.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        // PUT: api/Subscription/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscription(string id, Subscription subscription)
        {
            if (id != subscription.SubscriptionId)
            {
                return BadRequest();
            }

            _context.Entry(subscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(id))
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

        // POST: api/Subscription
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
        {
            _context.Subscription.Add(subscription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscription", new { id = subscription.SubscriptionId }, subscription);
        }

        // DELETE: api/Subscription/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(string id)
        {
            var subscription = await _context.Subscription.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscription.Remove(subscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionExists(string id)
        {
            return _context.Subscription.Any(e => e.SubscriptionId == id);
        }
    }
}
