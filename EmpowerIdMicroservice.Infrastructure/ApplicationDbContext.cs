using EmpowerIdMicroservice.Domain.AggregateModules.ApplicationUserAggregate;
using EmpowerIdMicroservice.Domain.AggregateModules.CommentAggregate;
using EmpowerIdMicroservice.Domain.AggregateModules.PostAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmpowerIdMicroservice.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Post> Post { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            // Additional configurations for ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FullName)
                    .HasMaxLength(50); 
            });

            // Configure IdentityUserLogin<string>
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey }); // Configure a composite primary key
            });

            // Configure IdentityUserRole<string>
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId }); // Configure a composite primary key
            });

            // Configure IdentityUserToken<string>
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                // Configure a composite primary key
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            var builder = new ConfigurationBuilder();
           
            IConfiguration Configuration = builder.Build();
            
            // TODO : Riminder get from appsettings

            optionsBuilder.UseSqlServer("Server=.;Initial Catalog=EmpowerId;MultipleActiveResultSets=True;User Id=sa;Password=123;Encrypt=False");
            base.OnConfiguring(optionsBuilder);
        }
    }
}