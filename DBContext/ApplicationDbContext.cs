using Microsoft.EntityFrameworkCore;
using IOT;

namespace IOT.DBContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Example DbSet - adjust or remove according to your domain models
    public DbSet<WeatherForecast>? WeatherForecasts { get; set; }
}
