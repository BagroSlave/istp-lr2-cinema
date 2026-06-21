using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Controllers;
using MovieApi.Data;
using MovieApi.Models;
using Xunit;

namespace MovieApi.Tests;

// Юніт-тести контролера фільмів. Використовується EF Core InMemory —
// окрема БД у пам'яті на кожен тест, без потреби у справжньому PostgreSQL.
public class MoviesControllerTests
{
    // Створює контекст з унікальною in-memory БД та наповнює її тестовими даними.
    private static MovieContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MovieContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new MovieContext(options);

        var director = new Director { Id = 1, Name = "Тестовий режисер" };
        context.Directors.Add(director);
        context.Movies.AddRange(
            new Movie { Id = 1, Title = "Фільм A", Year = 2000, Rating = 7.5, DirectorId = 1 },
            new Movie { Id = 2, Title = "Фільм B", Year = 2010, Rating = 8.5, DirectorId = 1 }
        );
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetMovies_ReturnsAllMovies()
    {
        using var context = CreateContext();
        var controller = new MoviesController(context);

        var result = await controller.GetMovies();

        var movies = Assert.IsAssignableFrom<IEnumerable<Movie>>(result.Value);
        Assert.Equal(2, movies.Count());
    }

    [Fact]
    public async Task GetMovie_ExistingId_ReturnsMovie()
    {
        using var context = CreateContext();
        var controller = new MoviesController(context);

        var result = await controller.GetMovie(1);

        var movie = Assert.IsType<Movie>(result.Value);
        Assert.Equal("Фільм A", movie.Title);
    }

    [Fact]
    public async Task GetMovie_MissingId_ReturnsNotFound()
    {
        using var context = CreateContext();
        var controller = new MoviesController(context);

        var result = await controller.GetMovie(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateMovie_AddsMovie()
    {
        using var context = CreateContext();
        var controller = new MoviesController(context);

        var newMovie = new Movie { Title = "Новий фільм", Year = 2024, Rating = 9.0, DirectorId = 1 };
        var result = await controller.CreateMovie(newMovie);

        Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(3, context.Movies.Count());
    }

    [Fact]
    public async Task DeleteMovie_RemovesMovie()
    {
        using var context = CreateContext();
        var controller = new MoviesController(context);

        var result = await controller.DeleteMovie(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(1, context.Movies.Count());
        Assert.Null(await context.Movies.FindAsync(1));
    }
}
