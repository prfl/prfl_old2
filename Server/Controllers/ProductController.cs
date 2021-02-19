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
    public class ProductController : ControllerBase
    {
        private readonly ProfileContext _context;
        private readonly UserManager<ProfileUser> _userManager;

        public ProductController(ProfileContext context, UserManager<ProfileUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _context.Product.Where(p => p.ProfileUserId == _userManager.GetUserId(User)).ToListAsync();
        }

        // GET: api/product/u/{username}
        [HttpGet("u/{username}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Product>>> GetLinkByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _context.Product.Where(l => l.ProfileUserId == user.Id).ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(string id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var userId = _userManager.GetUserId(User);

                var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Product).FirstOrDefaultAsync(f => f.LinkId == product.ProductId);

                var favoriteController = new FavoriteController(_context, _userManager);

                if(favorite != null && product.IsFavorite == false) {
                    await favoriteController.DeleteFavorite(favorite.FavoriteId, userId);
                }
                else if(favorite == null && product.IsFavorite == true) {
                    var newFavorite = new Favorite(){
                        LinkId = product.ProductId,
                        ProfileUserId = userId,
                        Type = LinkType.Product,
                        Name = product.Name,
                        Description = product.Description,
                        Url = product.Url,
                        IconUrl = "/assets/icons/box.svg",
                    };
                    await favoriteController.PostFavorite(newFavorite, userId);
                }
                else if(favorite != null && product.IsFavorite == true) {
                    favorite.ModifedOn = DateTime.Now;
                    favorite.Name = product.Name;
                    favorite.Description = product.Description;
                    favorite.Url = product.Url;
                    await favoriteController.PutFavorite(favorite.FavoriteId, favorite);
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var userId = _userManager.GetUserId(User);
            product.ProfileUserId = userId;

            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            var favoriteController = new FavoriteController(_context, _userManager);

            if(product.IsFavorite == true) {

                var favorite = new Favorite() {
                    LinkId = product.ProductId,
                    Name = product.Name,
                    Type = LinkType.Product,
                    Description = product.Description,
                    Url = product.Url,
                    IconUrl = "/assets/icons/box.svg"
                };
                

                await favoriteController.PostFavorite(favorite, userId);
                
            }

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Link).FirstOrDefaultAsync(f => f.LinkId == id);

            if(favorite != null) {
                _context.Favorite.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(string id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
