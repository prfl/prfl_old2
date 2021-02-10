using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    public class LinkController : ControllerBase
    {
        private readonly ProfileContext _context;
        private readonly UserManager<ProfileUser> _userManager;

        public LinkController(ProfileContext context, UserManager<ProfileUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Link
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Link>>> GetLink()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Link.Where(l => l.ProfileUserId == userId).ToListAsync();
        }

        // GET: api/u/{username}
        [HttpGet("u/{username}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Link>>> GetLinkByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _context.Link.Where(l => l.ProfileUserId == user.Id).ToListAsync();
        }

        // GET: api/Link/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Link>> GetLink(string id)
        {
            var link = await _context.Link.FindAsync(id);

            if (link == null)
            {
                return NotFound();
            }

            return link;
        }

        // PUT: api/Link/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLink(string id, Link link)
        {
            if (id != link.LinkId)
            {
                return BadRequest();
            }

            _context.Entry(link).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var userId = _userManager.GetUserId(User);

                var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Link).FirstOrDefaultAsync(f => f.LinkId == id);

                var favoriteController = new FavoriteController(_context, _userManager);

                if(favorite != null && link.IsFavorite == false) {
                    await favoriteController.DeleteFavorite(favorite.FavoriteId);
                }
                else if(favorite == null && link.IsFavorite == true) {
                    var newFavorite = new Favorite(){
                        LinkId = link.LinkId,
                        ProfileUserId = userId,
                        Type = LinkType.Link,
                        Name = link.Name,
                        Description = link.Description,
                        Url = link.Url,
                        IconUrl = "/assets/icons/link.svg",
                    };
                    await favoriteController.PostFavorite(newFavorite, userId);
                }
                else if(favorite != null && link.IsFavorite == true) {
                    favorite.ModifedOn = DateTime.Now;
                    favorite.Name = link.Name;
                    favorite.Description = link.Description;
                    favorite.Url = link.Url;
                    await favoriteController.PutFavorite(favorite.FavoriteId, favorite);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinkExists(id))
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

        // POST: api/Link
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Link>> PostLink(Link link)
        {
            var userId = _userManager.GetUserId(User);
            link.ProfileUserId = userId;

            _context.Link.Add(link);
            await _context.SaveChangesAsync();

            if(link.IsFavorite == true) {
                var favorite = new Favorite() {
                    LinkId = link.LinkId,
                    ProfileUserId = userId,
                    Name = link.Name,
                    Type = LinkType.Link,
                    Description = link.Description,
                    Url = link.Url,
                    IconUrl = "/assets/icons/link.svg"
                };
                _context.Favorite.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetLink", new { id = link.LinkId }, link);
        }

        // DELETE: api/Link/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLink(string id)
        {
            var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Link).FirstOrDefaultAsync(f => f.LinkId == id);

            if(favorite != null) {
                _context.Favorite.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            
            var link = await _context.Link.FindAsync(id);
            if (link == null)
            {
                return NotFound();
            }

            _context.Link.Remove(link);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LinkExists(string id)
        {
            return _context.Link.Any(e => e.LinkId == id);
        }
    }
}
