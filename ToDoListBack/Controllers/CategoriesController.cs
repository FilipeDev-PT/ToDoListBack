using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;
using ToDoListBack.Models;


namespace AppControle.Controllers;

[Route("api/categories")]
[ApiController]
public class categoriesController : ControllerBase
{
    private readonly TaskContext _context;

    public categoriesController(TaskContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categories>>> GetAllCategories()
    {
        return await _context.Categories
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }
     
    [HttpGet("{id}")]
    public async Task<ActionResult<Categories>> GetCategoriesById(int id)
    {
        var Categorie = await _context.Categories.FindAsync(id);

        if (Categorie == null)
        {
            return NotFound();
        }

        return ItemToDTO(Categorie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategories(int id, Categories todoDTO)
    {
        var Category = await _context.Categories.FindAsync(id);

        if (Category == null)
        {
            return NotFound();
        }

        Category.Id = todoDTO.Id;
        Category.Name = todoDTO.Name;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!CategoriesExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Categories>> PostCategories(Categories todoDTO)
    {
        var Category = new Categories
        {
            Id = todoDTO.Id,
            Name = todoDTO.Name,
        };

        _context.Categories.Add(Category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetAllCategories),
            new { id = Category.Id },
            ItemToDTO(Category));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategories(int id)
    {
        var Category = await _context.Categories.FindAsync(id);
        if (Category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(Category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoriesExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }

    private static Categories ItemToDTO(Categories Category) =>
    new Categories
    {
        Id = Category.Id,
        Name = Category.Name,
    };
}
