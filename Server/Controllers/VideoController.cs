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
    public class VideoController : ControllerBase
    {
        private readonly ProfileContext _context;
        private readonly UserManager<ProfileUser> _userManager;

        public VideoController(ProfileContext context, UserManager<ProfileUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Video
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideo()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Video.Where(v => v.ProfileUserId == userId).ToListAsync();
        }

        [HttpGet("u/{username}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideoByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _context.Video.Where(l => l.ProfileUserId == user.Id).ToListAsync();
        }

        // GET: api/Video/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetVideo(string id)
        {
            var video = await _context.Video.FindAsync(id);

            if (video == null)
            {
                return NotFound();
            }

            return video;
        }

        // PUT: api/Video/5
        // To protect from overVideoing attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideo(string id, Video video)
        {
            var userId = _userManager.GetUserId(User);
            if (id != video.VideoId)
            {
                return BadRequest();
            }

            _context.Entry(video).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Video).FirstOrDefaultAsync(f => f.LinkId == id);

                var favoriteController = new FavoriteController(_context, _userManager);

                if(favorite != null && video.IsFavorite == false) {
                    await favoriteController.DeleteFavorite(favorite.FavoriteId);
                }

                else if(favorite == null && video.IsFavorite == true) {
                    var newFavorite = new Favorite(){
                        LinkId = video.VideoId,
                        ProfileUserId = userId,
                        Type = LinkType.Video,
                        Name = video.Name,
                        Description = video.Description,
                        Url = video.Url,
                        IconUrl = "/assets/icons/video.svg",
                    };
                    await favoriteController.PostFavorite(newFavorite);
                }

                else if(favorite != null && video.IsFavorite == true) {
                    favorite.ModifedOn = DateTime.Now;
                    favorite.Name = video.Name;
                    favorite.Description = video.Description;
                    favorite.Url = video.Url;
                    await favoriteController.PutFavorite(favorite.FavoriteId, favorite);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
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

        // Video: api/Video
        // To protect from overVideoing attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Video>> VideoVideo(Video video)
        {
            var userId = _userManager.GetUserId(User);
            video.ProfileUserId = userId;

            _context.Video.Add(video);
            await _context.SaveChangesAsync();

            if(video.IsFavorite == true) {
                var favorite = new Favorite() {
                    LinkId = video.VideoId,
                    ProfileUserId = userId,
                    Name = video.Name,
                    Type = LinkType.Video,
                    Description = video.Description,
                    Url = video.Url,
                    IconUrl = "/assets/icons/video.svg"
                };
                _context.Favorite.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetVideo", new { id = video.VideoId }, video);
        }

        // DELETE: api/Video/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(string id)
        {
            var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Video).FirstOrDefaultAsync(f => f.LinkId == id);

            if(favorite != null) {
                _context.Favorite.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            
            var video = await _context.Video.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            _context.Video.Remove(video);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VideoExists(string id)
        {
            return _context.Video.Any(e => e.VideoId == id);
        }
    }
}
