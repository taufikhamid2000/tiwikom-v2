using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TIWIKOM.Entities.Contexts;

/// <summary>
/// Design-time factory for creating ApplicationDbContext instances for EF Core migrations
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Use SQL Server for migrations (this connection string is only used for generating migrations)
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TIWIKOM;Trusted_Connection=true;");
        
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
