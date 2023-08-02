using Microsoft.EntityFrameworkCore;

class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions options) : base(options) { }
    public DbSet<Customer>? Customers { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<Item>? Items { get; set; }
    public DbSet<Store>? Stores { get; set; }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow; // current datetime

            if (entity.State == EntityState.Added)
            {
                ((BaseModel)entity.Entity).CreatedAt = now;
            }
            ((BaseModel)entity.Entity).UpdatedAt = now;
        }
    }
}