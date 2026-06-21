using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Controllers;
using MovieApi.Data;
using MovieApi.Models;
using Xunit;

namespace MovieApi.Tests;

// Юніт-тести контролера режисерів на EF Core InMemory.
public class DirectorsControllerTests
{
    private static MovieContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MovieContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new MovieContext(options);
        context.Directors.AddRange(
            new Director { Id = 1, Name = "Режисер 1" },
            new Director { Id = 2, Name = "Режисер 2" }
        );
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetDirectors_ReturnsAllDirectors()
    {
        using var context = CreateContext();
        var controller = new DirectorsController(context);

        var result = await controller.GetDirectors();

        var directors = Assert.IsAssignableFrom<IEnumerable<Director>>(result.Value);
        Assert.Equal(2, directors.Count());
    }

    [Fact]
    public async Task GetDirector_ExistingId_ReturnsDirector()
    {
        using var context = CreateContext();
        var controller = new DirectorsController(context);

        var result = await controller.GetDirector(2);

        var director = Assert.IsType<Director>(result.Value);
        Assert.Equal("Режисер 2", director.Name);
    }

    [Fact]
    public async Task CreateDirector_AddsDirector()
    {
        using var context = CreateContext();
        var controller = new DirectorsController(context);

        var newDirector = new Director { Name = "Новий режисер" };
        var result = await controller.CreateDirector(newDirector);

        Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(3, context.Directors.Count());
    }

    [Fact]
    public async Task DeleteDirector_RemovesDirector()
    {
        using var context = CreateContext();
        var controller = new DirectorsController(context);

        var result = await controller.DeleteDirector(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(1, context.Directors.Count());
    }
}
