using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TIWIKOM.Entities.Contexts;

/// <summary>
/// Application's database context for TIWIKOM
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Tip> Tips { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<TipLike> TipLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure identity tables
        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>().ToTable("Roles");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>().ToTable("UserTokens");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>().ToTable("RoleClaims");

        // Configure Tip entity
        builder.Entity<Tip>()
            .HasOne(t => t.Author)
            .WithMany(u => u.Tips)
            .HasForeignKey(t => t.AuthorId);

        builder.Entity<Tip>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Tips)
            .HasForeignKey(t => t.CategoryId);

        // Configure Category entity
        builder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        // Configure Comment entity
        builder.Entity<Comment>()
            .HasOne(c => c.Tip)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TipId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany()
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure TipLike entity
        builder.Entity<TipLike>()
            .HasOne(tl => tl.Tip)
            .WithMany(t => t.Likes)
            .HasForeignKey(tl => tl.TipId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<TipLike>()
            .HasOne(tl => tl.User)
            .WithMany()
            .HasForeignKey(tl => tl.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ensure one like per user per tip
        builder.Entity<TipLike>()
            .HasIndex(tl => new { tl.TipId, tl.UserId })
            .IsUnique();
    }
}
