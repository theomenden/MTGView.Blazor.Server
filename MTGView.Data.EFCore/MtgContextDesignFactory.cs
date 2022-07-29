using Microsoft.EntityFrameworkCore.Design;

namespace MTGView.Data.EFCore;
internal class MtgContextDesignFactory : IDesignTimeDbContextFactory<MagicthegatheringDbContext>
{
    private const string CONNECTION_STRING = "Server=(local);Database=MagicTheGathering.db;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";


    public MagicthegatheringDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MagicthegatheringDbContext>();
        optionsBuilder.UseSqlServer(CONNECTION_STRING)
            .EnableServiceProviderCaching();

        return new MagicthegatheringDbContext(optionsBuilder.Options);
    }
}

