using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;

namespace MovieApi.Controllers;

// REST-контролер для режисерів: GET (всі), GET (за id), POST, PUT, DELETE.
[ApiController]
[Route("api/[controller]")]
public class DirectorsController : ControllerBase
{
    private readonly MovieContext _context;

    public DirectorsController(MovieContext context)
    {
        _context = context;
    }

    // GET: api/directors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Director>>> GetDirectors()
    {
        return await _context.Directors.ToListAsync();
    }

    // GET: api/directors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Director>> GetDirector(int id)
    {
        var director = await _context.Directors.FindAsync(id);
        if (director == null) return NotFound();
        return director;
    }

    // POST: api/directors
    [HttpPost]
    public async Task<ActionResult<Director>> CreateDirector(Director director)
    {
        _context.Directors.Add(director);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDirector), new { id = director.Id }, director);
    }

    // PUT: api/directors/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDirector(int id, Director director)
    {
        if (id != director.Id) return BadRequest();

        _context.Entry(director).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Directors.Any(d => d.Id == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    // DELETE: api/directors/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDirector(int id)
    {
        var director = await _context.Directors.FindAsync(id);
        if (director == null) return NotFound();

        _context.Directors.Remove(director);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
