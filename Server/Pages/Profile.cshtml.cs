using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profile.Server.Data;
using Profile.Shared.Models;

namespace Profile.Server.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ILogger<ProfileModel> _logger;
        private readonly UserManager<ProfileUser> _userManager;
        private readonly ProfileContext _context;

        public ProfileModel(ILogger<ProfileModel> logger, 
                    UserManager<ProfileUser> userManager,
                    ProfileContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public ProfileUser ProfileUser { get; set; }
        public IList<Favorite> Favorites { get; set; }
        public async Task<IActionResult> OnGetAsync(string username)
        {
            if(!String.IsNullOrEmpty(username)) {
                ProfileUser = await _userManager.FindByNameAsync(username);
                Favorites = await _context.Favorite.Where(u => u.ProfileUser.UserName == username).ToListAsync();

            }
            
            return Page();
        }
    }
}
