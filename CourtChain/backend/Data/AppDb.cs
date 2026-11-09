using Microsoft.EntityFrameworkCore;
public class AppDb(DbContextOptions<AppDb> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Facility> Facilities => Set<Facility>();
    public DbSet<Court> Courts => Set<Court>();
    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<Audit> Audits => Set<Audit>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Facility>().HasIndex(x => x.Name).IsUnique();
        b.Entity<Facility>().Property(x => x.Version).IsConcurrencyToken();
        b.Entity<Court>().Property(x => x.Version).IsConcurrencyToken();
        b.Entity<Court>().HasIndex(x => new { x.FacilityId, x.Name }).IsUnique();
        b.Entity<Court>().HasOne<Facility>().WithMany().HasForeignKey(x => x.FacilityId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Policy>().HasOne<Facility>().WithMany().HasForeignKey(x => x.FacilityId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Policy>().HasIndex(x => new { x.Scope, x.FacilityId, x.Sport, x.EffectiveStart });
        b.Entity<Policy>().Property(x => x.Regular).HasPrecision(18, 2);
        b.Entity<Policy>().Property(x => x.Peak).HasPrecision(18, 2);
        b.Entity<Policy>().Property(x => x.Weekend).HasPrecision(18, 2);
        b.Entity<Audit>().HasIndex(x => new { x.Entity, x.EntityId, x.At });
    }
}
