using Lab_App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class myDBcontext : IdentityDbContext<ApplicationUser>
{
    public myDBcontext(DbContextOptions<myDBcontext> options) : base(options) { }

    public DbSet<mainTable> mainTableS { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<License> Licenses { get; set; }  // ✅ Ensure License table exists
    public DbSet<Labs> Labs { get; set; }  // ✅ Ensure License table exists

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ✅ Ensure LicenseKey is correctly mapped for ApplicationUser
        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.LicenseKey)
            .HasMaxLength(100) // Adjust as needed
            .IsRequired(false); // Set to false if LicenseKey is optional

        // ✅ Configure relationship between ApplicationUser and License
        modelBuilder.Entity<License>()
            .HasOne(l => l.User)
            .WithOne(u => u.License) // One-to-One Relationship
            .HasForeignKey<License>(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<mainTable>()
       .HasOne(m => m.Labs)
       .WithMany(l => l.MainTables)
       .HasForeignKey(m => m.labId)
       .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete
    }
}
