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
    public class AccountController : ControllerBase
    {
        private readonly ProfileContext _context;
        private readonly UserManager<ProfileUser> _userManager;

        public AccountController(ProfileContext context, UserManager<ProfileUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccount()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Account.Include(a => a.Application).Where(a => a.ProfileUserId == userId).ToListAsync();
        }

        // GET: api/Account/u/{username}
        [HttpGet("u/{username}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _context.Account.Include(a => a.Application).Where(a => a.ProfileUserId == user.Id).ToListAsync();
        }


        // GET: api/Account/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            var account = await _context.Account.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Account/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(string id, Account account)
        {
            var userId = _userManager.GetUserId(User);

            if (id != account.AccountId)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Account).FirstOrDefaultAsync(f => f.LinkId == id);
                var application = await _context.Application.FirstOrDefaultAsync(a => a.ApplicationId == account.ApplicationId);

                var favoriteController = new FavoriteController(_context, _userManager);

                if(favorite != null && account.IsFavorite == false) {
                    await favoriteController.DeleteFavorite(favorite.FavoriteId);
                }

                else if(favorite == null && account.IsFavorite == true) {
                    var newFavorite = new Favorite(){
                        LinkId = account.AccountId,
                        ProfileUserId = userId,
                        Type = LinkType.Account,
                        Name = account.Username,
                        Description = application.Name,
                        Url = application.ApplicationUserLink + account.Username,
                        IconUrl = $"/assets/Logo/{application.Name}.svg",
                    };
                    await favoriteController.PostFavorite(newFavorite);
                }

                else if(favorite != null && account.IsFavorite == true) {
                    favorite.ModifedOn = DateTime.Now;
                    favorite.Name = account.Username;
                    favorite.Url =  account.Application.ApplicationUserLink + account.Username;
                    await favoriteController.PutFavorite(favorite.FavoriteId, favorite);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        // POST: api/Account
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            var userId = _userManager.GetUserId(User);

            account.ProfileUserId = userId;

            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            var application = await _context.Application.FirstOrDefaultAsync(a => a.ApplicationId == account.ApplicationId);

            if(account.IsFavorite == true) {
                var favorite = new Favorite() {
                    LinkId = account.AccountId,
                    ProfileUserId = userId,
                    Type = LinkType.Account,
                    Name = account.Username,
                    Description = application.Name,
                    Url = application.ApplicationUserLink + account.Username,
                    IconUrl = $"/assets/Logo/{application.Name}.svg",
                };
                _context.Favorite.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
        }


        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var favorite = await _context.Favorite.Where(f => f.Type == LinkType.Account).FirstOrDefaultAsync(f => f.LinkId == id);

            if(favorite != null) {
                _context.Favorite.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            var account = await _context.Account.FindAsync(id);
            
            if (account == null)
            {
                return NotFound();
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(string id)
        {
            return _context.Account.Any(e => e.AccountId == id);
        }
    }
}
