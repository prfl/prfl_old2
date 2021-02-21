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

namespace Profile.Server.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly ProfileContext _context;

        public DetailsModel(ProfileContext context)
        {
            _context = context;
        }
        
        public Product Product { get; set; }
        public async Task<IActionResult> OnGetAsync(string productId)
        {
            if(!String.IsNullOrEmpty(productId)) {
                Product = await _context.Product.FirstOrDefaultAsync(p => p.ProductId == productId);
            }
            
            return Page();
        }
    }
}
