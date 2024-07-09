using Microsoft.EntityFrameworkCore;


public class DatabaseContext : DbContext
{
  public DatabaseContext(DbContextOptions<DatabaseContext> options)
      : base(options)
  {
  }

  public DbSet<Provider> Providers { get; set; } = null!;
  public DbSet<Client> Clients { get; set; }
  public DbSet<Appointment> Appointments { get; set; }
}