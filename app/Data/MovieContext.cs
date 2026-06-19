using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Data;

// Контекст бази даних для кінокаталогу (Code-First, провайдер PostgreSQL/Npgsql).
public class MovieContext : DbContext
{
    public MovieContext(DbContextOptions<MovieContext> options) : base(options)
    {
    }

    public DbSet<Director> Directors => Set<Director>();
    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Зв'язок: фільм -> режисер (один режисер має багато фільмів)
        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Director)
            .WithMany(d => d.Movies)
            .HasForeignKey(m => m.DirectorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
