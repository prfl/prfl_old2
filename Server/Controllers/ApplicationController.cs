using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profile.Server.Data;
using Profile.Shared.Models.Admin;

namespace Profile.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly ProfileContext _context;

        public ApplicationController(ProfileContext context)
        {
            _context = context;
        }

        // GET: api/Application
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplication()
        {
            return await _context.Application.ToListAsync();
        }

        // GET: api/Application/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Application>> GetApplication(string id)
        {
            var application = await _context.Application.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            return application;
        }

        private bool ApplicationExists(string id)
        {
            return _context.Application.Any(e => e.ApplicationId == id);
        }
    }
}
