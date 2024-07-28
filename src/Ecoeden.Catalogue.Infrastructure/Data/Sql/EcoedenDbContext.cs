using Ecoeden.Catalogue.Domain.Sql;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ecoeden.Catalogue.Infrastructure.Data.Sql;
public class EcoedenDbContext : DbContext
{

    public EcoedenDbContext(DbContextOptions<EcoedenDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<EventPublishHistory> EventPublishHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach(var entry in ChangeTracker.Entries<SqlBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdateAt = DateTime.UtcNow;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
