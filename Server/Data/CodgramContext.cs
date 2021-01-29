using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profile.Shared.Models.Codgram;

    public class CodgramContext : DbContext
    {
        public CodgramContext (DbContextOptions<CodgramContext> options, ILogger<CodgramContext> logger)
            : base(options)
        {
        }

        public DbSet<Profile.Shared.Models.Codgram.API> API { get; set; }
    }
