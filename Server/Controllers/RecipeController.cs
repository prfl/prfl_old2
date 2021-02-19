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
    public class RecipeController : ControllerBase
    {
        private readonly ProfileContext _context;
        private readonly UserManager<ProfileUser> _userManager;

        public RecipeController(ProfileContext context, UserManager<ProfileUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Recipe
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipe()
        {
            return await _context.Recipe.ToListAsync();
        }

        // GET: api/recipe/u/{username}
        [HttpGet("u/{username}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetLinkByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _context.Recipe.Where(l => l.ProfileUserId == user.Id).ToListAsync();
        }

        // GET: api/Recipe/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(string id)
        {
            var recipe = await _context.Recipe.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        // PUT: api/Recipe/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(string id, Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return BadRequest();
            }

            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var userId = _userManager.GetUserId(User);

                var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Recipe).FirstOrDefaultAsync(f => f.LinkId == id);

                var favoriteController = new FavoriteController(_context, _userManager);


                if(favorite != null && recipe.IsFavorite == false) {
                    await favoriteController.DeleteFavorite(favorite.FavoriteId, userId);
                }

                else if(favorite == null && recipe.IsFavorite == true) {
                    var newFavorite = new Favorite(){
                        LinkId = recipe.RecipeId,
                        ProfileUserId = userId,
                        Type = LinkType.Recipe,
                        Name = recipe.Name,
                        Description = recipe.Description,
                        Url = recipe.Url,
                    };
                    if(recipe.Type == RecipeType.Food) {
                    favorite.IconUrl = "/assets/icons/food.svg";
                    }
                    else {
                        favorite.IconUrl = "/assets/icons/alcohol.svg";
                    }
                    await favoriteController.PostFavorite(newFavorite, userId);
                }

                else if(favorite != null && recipe.IsFavorite == true) {
                    favorite.ModifedOn = DateTime.Now;
                    favorite.Name = recipe.Name;
                    favorite.Description = recipe.Description;
                    favorite.Url = recipe.Url;
                    await favoriteController.PutFavorite(favorite.FavoriteId, favorite);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
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

        // POST: api/Recipe
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            var userId = _userManager.GetUserId(User);
            recipe.ProfileUserId = userId;
            
            _context.Recipe.Add(recipe);
            await _context.SaveChangesAsync();

            var favoriteController = new FavoriteController(_context, _userManager);

            if(recipe.IsFavorite == true) {

                var favorite = new Favorite() {
                    LinkId = recipe.RecipeId,
                    Name = recipe.Name,
                    Type = LinkType.Recipe,
                    Description = recipe.Description,
                    Url = recipe.Url
                };
                if(recipe.Type == RecipeType.Food) {
                    favorite.IconUrl = "/assets/icons/food.svg";
                }
                else {
                    favorite.IconUrl = "/assets/icons/alcohol.svg";
                }

                await favoriteController.PostFavorite(favorite, userId);
                
            }

            return CreatedAtAction("GetRecipe", new { id = recipe.RecipeId }, recipe);
        }

        // DELETE: api/Recipe/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(string id)
        {
            var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Recipe).FirstOrDefaultAsync(f => f.LinkId == id);

            if(favorite != null) {
                _context.Favorite.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipe.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeExists(string id)
        {
            return _context.Recipe.Any(e => e.RecipeId == id);
        }
    }
}
