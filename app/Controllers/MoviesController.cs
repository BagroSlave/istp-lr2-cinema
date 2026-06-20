using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;

namespace MovieApi.Controllers;

// REST-контролер для фільмів: GET (всі), GET (за id), POST, PUT, DELETE.
[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MovieContext _context;

    public MoviesController(MovieContext context)
    {
        _context = context;
    }

    // GET: api/movies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
    {
        return await _context.Movies.ToListAsync();
    }

    // GET: api/movies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null) return NotFound();
        return movie;
    }

    // POST: api/movies
    [HttpPost]
    public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
    {
        // Якщо передано ім'я режисера — знаходимо існуючого або створюємо нового.
        if (!string.IsNullOrWhiteSpace(movie.DirectorName))
        {
            var name = movie.DirectorName.Trim();
            var director = await _context.Directors.FirstOrDefaultAsync(d => d.Name == name);
            if (director is null)
            {
                director = new Director { Name = name };
                _context.Directors.Add(director);
                await _context.SaveChangesAsync();
            }
            movie.DirectorId = director.Id;
        }
        movie.DirectorName = null;
        movie.Director = null;

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    // PUT: api/movies/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, Movie movie)
    {
        if (id != movie.Id) return BadRequest();

        _context.Entry(movie).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Movies.Any(m => m.Id == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    // DELETE: api/movies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null) return NotFound();

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
