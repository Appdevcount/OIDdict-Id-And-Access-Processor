using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace Id_And_Access_Processor
{
    public class OpenIddictContext : OpenIddictContext<OpenIddictContext>
    {
        public OpenIddictContext(DbContextOptions<OpenIddictContext> options)
          : base(options)
        { }
    }
    public class OpenIddictContext<TContext> : DbContext 
    where TContext : DbContext 
    {
        public OpenIddictContext(DbContextOptions<TContext> options)
          : base(options)
        { }

        public DbSet<OpenIddictEntityFrameworkCoreApplication> Applications { get; set; }

        public DbSet<OpenIddictEntityFrameworkCoreAuthorization> Authorizations { get; set; }

        public DbSet<OpenIddictEntityFrameworkCoreScope> Scopes { get; set; }

        public DbSet<OpenIddictEntityFrameworkCoreToken> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("OID");

            builder.UseOpenIddict();
        }
    }
}
