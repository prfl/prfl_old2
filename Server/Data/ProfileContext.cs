using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Profile.Shared.Models;
using Profile.Shared.Models.Admin;

namespace Profile.Server.Data
{
    public class ProfileContext : ApiAuthorizationDbContext<ProfileUser>
    {
        public ProfileContext(
            DbContextOptions<ProfileContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ProfileUser>(b =>
            {
                b.ToTable("ProfileUser");
            });
            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("Tokens");
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Roles");
            });

            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRoles");
            });

        }
    

        public DbSet<Profile.Shared.Models.Admin.Application> Application { get; set; }
    

        public DbSet<Profile.Shared.Models.Account> Account { get; set; }
    

        public DbSet<Profile.Shared.Models.Project> Project { get; set; }
    

        public DbSet<Profile.Shared.Models.Link> Link { get; set; }
    

        public DbSet<Profile.Shared.Models.Video> Video { get; set; }
    

        public DbSet<Profile.Shared.Models.Favorite> Favorite { get; set; }
    

        public DbSet<Profile.Shared.Models.Admin.Subscription> Subscription { get; set; }
        public DbSet<Profile.Shared.Models.Admin.ReservedLink> ReservedLink { get; set; }
        public DbSet<Profile.Shared.Models.GettingStarted> GettingStarted { get; set; }
        public DbSet<Profile.Shared.Models.Schedule> Schedule { get; set; }
    }
}
