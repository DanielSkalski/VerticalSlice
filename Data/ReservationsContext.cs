using System.Data;
using Microsoft.EntityFrameworkCore;

namespace VerticalSlice.Data;

public class ReservationsContext : DbContext
{
    public DbSet<Reservation> Reservations { get; set; }

    public ReservationsContext(DbContextOptions<ReservationsContext> options) 
        : base(options)
    { 
    }

    public IDbConnection DbConnection => Database.GetDbConnection(); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Reservation>().ToTable("Reservations");
    }
}
