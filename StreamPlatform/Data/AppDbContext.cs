using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StreamPlatform.Models;
using Stream = StreamPlatform.Models.Stream;

namespace StreamPlatform.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
        Database.Migrate();
    }
    public DbSet<Stream> Streams { get; set; }
}
