namespace dotnet_rpg_web_api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<Character> Characters => Set<Character>();
    public DbSet<User> Users => Set<User>();

    // Add-Migration InitialCreate
}