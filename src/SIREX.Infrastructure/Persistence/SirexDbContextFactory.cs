using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SIREX.Infrastructure.Persistence;

public class SirexDbContextFactory : IDesignTimeDbContextFactory<SirexDbContext>
{
    public SirexDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SirexDbContext>();
        optionsBuilder.UseSqlite("Data Source=sirex_design.db");
        return new SirexDbContext(optionsBuilder.Options);
    }
}
