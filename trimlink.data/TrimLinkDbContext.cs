using trimlink.data.Models;
using Microsoft.EntityFrameworkCore;

namespace trimlink.data;

public class TrimLinkDbContext : DbContext
{
    public TrimLinkDbContext() : base()
    {
    }

    public TrimLinkDbContext(DbContextOptions<TrimLinkDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Link> Links => Set<Link>();
}