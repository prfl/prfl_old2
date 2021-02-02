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
    public class ScheduleController : ControllerBase
    {
        private readonly ProfileContext _context;
        private readonly UserManager<ProfileUser> _userManager;

        public ScheduleController(ProfileContext context, UserManager<ProfileUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Schedule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedule()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Schedule.Where(a => a.ProfileUserId == userId).ToListAsync();
        }

        
        // GET: api/schedule/u/{username}
        [HttpGet("u/{username}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetScheduleByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _context.Schedule.Where(a => a.ProfileUserId == user.Id).ToListAsync();
        }

        // GET: api/Schedule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetSchedule(string id)
        {
            var schedule = await _context.Schedule.FindAsync(id);

            if (schedule == null)
            {
                return NotFound();
            }

            return schedule;
        }

        // PUT: api/Schedule/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(string id, Schedule schedule)
        {
            var userId = _userManager.GetUserId(User);

            if (id != schedule.ScheduleId)
            {
                return BadRequest();
            }

            _context.Entry(schedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Schedule).FirstOrDefaultAsync(f => f.LinkId == id);

                var favoriteController = new FavoriteController(_context, _userManager);

                if(favorite != null && schedule.IsFavorite == false) {
                    await favoriteController.DeleteFavorite(favorite.FavoriteId);
                }

                else if(favorite == null && schedule.IsFavorite == true) {
                    var newFavorite = new Favorite(){
                        LinkId = schedule.ScheduleId,
                        ProfileUserId = userId,
                        Type = LinkType.Schedule,
                        Name = schedule.Name,
                        Description = schedule.Description,
                        Url = schedule.Url,
                        IconUrl = "/assets/icons/kanban.svg",
                    };
                    await favoriteController.PostFavorite(newFavorite);
                }

                else if(favorite != null && schedule.IsFavorite == true) {
                    favorite.ModifedOn = DateTime.Now;
                    favorite.Name = schedule.Name;
                    favorite.Description = schedule.Description;
                    favorite.Url = schedule.Url;
                    await favoriteController.PutFavorite(favorite.FavoriteId, favorite);
                }
                                

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(id))
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

        // POST: api/Schedule
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Schedule>> PostSchedule(Schedule schedule)
        {
            var userId = _userManager.GetUserId(User);
            schedule.ProfileUserId = userId;

            _context.Schedule.Add(schedule);
            await _context.SaveChangesAsync();

            if(schedule.IsFavorite == true) {
                var favorite = new Favorite() {
                    LinkId = schedule.ScheduleId,
                    ProfileUserId = userId,
                    Type = LinkType.Schedule,
                    Name = schedule.Name,
                    Description = schedule.Description,
                    Url = schedule.Url,
                    IconUrl = "/assets/icons/calendar2.svg"
                };
                _context.Favorite.Add(favorite);
                await _context.SaveChangesAsync();
            }
                

            return CreatedAtAction("GetSchedule", new { id = schedule.ScheduleId }, schedule);
        }

        // DELETE: api/Schedule/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            _context.Schedule.Remove(schedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScheduleExists(string id)
        {
            return _context.Schedule.Any(e => e.ScheduleId == id);
        }
    }
}
