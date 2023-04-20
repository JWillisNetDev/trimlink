using trimlink.data.Models;
using Microsoft.EntityFrameworkCore;

namespace trimlink.data;

public class TrimLinkDbContext : DbContext
{
    public TrimLinkDbContext(DbContextOptions<TrimLinkDbContext> options) : base(options)
    {
    }

    public DbSet<Link> Links => Set<Link>();
}