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
    public class ProjectController : ControllerBase
    {
        private readonly ProfileContext _context;
        private readonly UserManager<ProfileUser> _userManager;

        public ProjectController(ProfileContext context, UserManager<ProfileUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Project
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProject()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Project.Where(p => p.ProfileUserId == userId).ToListAsync();
        }

        // GET: api/u/{username}
        [HttpGet("u/{username}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _context.Project.Where(a => a.ProfileUserId == user.Id).ToListAsync();
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(string id)
        {
            var project = await _context.Project.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Project/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(string id, Project project)
        {
            var userId = _userManager.GetUserId(User);

            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Project).FirstOrDefaultAsync(f => f.LinkId == id);

                var favoriteController = new FavoriteController(_context, _userManager);

                if(favorite != null && project.IsFavorite == false) {
                    await favoriteController.DeleteFavorite(favorite.FavoriteId);
                }

                else if(favorite == null && project.IsFavorite == true) {
                    var newFavorite = new Favorite(){
                        LinkId = project.ProjectId,
                        ProfileUserId = userId,
                        Type = LinkType.Project,
                        Name = project.Name,
                        Description = project.Description,
                        Url = project.Url,
                        IconUrl = "/assets/icons/kanban.svg",
                    };
                    await favoriteController.PostFavorite(newFavorite, userId);
                }

                else if(favorite != null && project.IsFavorite == true) {
                    favorite.ModifedOn = DateTime.Now;
                    favorite.Name = project.Name;
                    favorite.Description = project.Description;
                    favorite.Url = project.Url;
                    await favoriteController.PutFavorite(favorite.FavoriteId, favorite);
                }
                
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Project
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            var userId = _userManager.GetUserId(User);
            project.ProfileUserId = userId;

            _context.Project.Add(project);
            await _context.SaveChangesAsync();

            if(project.IsFavorite == true) {
                var favorite = new Favorite() {
                    LinkId = project.ProjectId,
                    ProfileUserId = userId,
                    Type = LinkType.Project,
                    Name = project.Name,
                    Description = project.Description,
                    Url = project.Url,
                    IconUrl = "/assets/icons/kanban.svg"
                };
                _context.Favorite.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
        }

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Project).FirstOrDefaultAsync(f => f.LinkId == id);

            if(favorite != null) {
                _context.Favorite.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Project.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(string id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
    }
}
