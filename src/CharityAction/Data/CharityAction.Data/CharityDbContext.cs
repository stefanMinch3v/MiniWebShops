namespace CharityAction.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class CharityDbContext : IdentityDbContext<ApplicationUser>
    {
        public CharityDbContext(DbContextOptions<CharityDbContext> options)
            : base(options)
        { }

        public DbSet<Event> Events { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Event>()
                .HasMany(e => e.Images);
        }
    }
}