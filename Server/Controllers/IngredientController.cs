using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profile.Server.Data;
using Profile.Shared.Models;

namespace Profile.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly ProfileContext _context;

        public IngredientController(ProfileContext context)
        {
            _context = context;
        }

        // GET: api/Ingredient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredient()
        {
            return await _context.Ingredient.ToListAsync();
        }

        //GET: api/ingredient/recipe/5
        [HttpGet("recipe/{recipeId}")]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredientByRecipeId(string recipeId) {
            return await _context.Ingredient.Where(i => i.RecipeId == recipeId).OrderBy(i => i.Order).ToListAsync();
        }

        // GET: api/Ingredient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredient(string id)
        {
            var ingredient = await _context.Ingredient.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            return ingredient;
        }

        // GET: api/Ingredient/5
        [HttpGet("/recipe/{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(string id)
        {
            var recipe = await _context.Recipe.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        // PUT: api/Ingredient/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredient(string id, Ingredient ingredient)
        {
            if (id != ingredient.IngredientId)
            {
                return BadRequest();
            }

            _context.Entry(ingredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
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

        // POST: api/Ingredient
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ingredient>> PostIngredient(Ingredient ingredient, [FromQuery] string recipeId)
        {
            
            var lastIngredient = await _context.Ingredient.Where(i => i.RecipeId == recipeId).AnyAsync();
            if(!lastIngredient) {
                ingredient.Order = 1;
            }
            else {
                var lastIngredientOrder = await _context.Ingredient.Where(i => i.RecipeId == recipeId).MaxAsync(i => i.Order);
                ingredient.Order = lastIngredientOrder + 1;
            }
            
            
            ingredient.RecipeId = recipeId;

            _context.Ingredient.Add(ingredient);
            await _context.SaveChangesAsync();

            var recipe = await _context.Recipe.FindAsync(ingredient.RecipeId);

            return CreatedAtAction("GetIngredient", new { id = ingredient.IngredientId }, ingredient);
        }

        // DELETE: api/Ingredient/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(string id)
        {
            var ingredient = await _context.Ingredient.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            _context.Ingredient.Remove(ingredient);
            await _context.SaveChangesAsync();

            // Reorder Ingredients
            var ingredients = await _context.Ingredient.Where(i => i.RecipeId == ingredient.RecipeId).OrderBy(i => i.Order).ToListAsync();

            foreach(var item in ingredients) {
                item.Order -= 1;
                await PutIngredient(item.IngredientId, item);
            }

            return NoContent();
        }

        private bool IngredientExists(string id)
        {
            return _context.Ingredient.Any(e => e.IngredientId == id);
        }
    }
}
