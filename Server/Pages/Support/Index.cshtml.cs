using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Octokit;
using Profile.Server.Data;
using Profile.Shared.Models;

namespace Profile.Server.Pages.Help.Supports
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<ProfileUser> _userManager;
        private readonly ProfileContext _context;

        public IndexModel(ILogger<IndexModel> logger, 
                    UserManager<ProfileUser> userManager,
                    ProfileContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync(string username)
        {

            
            return Page();
        }
    }
}
