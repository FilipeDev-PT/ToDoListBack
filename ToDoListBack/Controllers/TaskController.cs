﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;
using ToDoListBack.Models;


namespace AppControle.Controllers;

[Route("api/task")]
[ApiController]
public class taskController : ControllerBase
{
    private readonly TaskContext _context;

    public taskController(TaskContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tasks>>> GetAllTask()
    {
        return await _context.Tasks
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Tasks>> GetTaskById(int id)
    {
        var Task = await _context.Tasks.FindAsync(id);

        if (Task == null)
        {
            return NotFound();
        }

        return ItemToDTO(Task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTask(int id)
    {
        var Task = await _context.Tasks.FindAsync(id);

        var Category = await _context.Categories.FindAsync(Task.CategoryId);

        if (Task == null)
        {
            return NotFound();
        }

        Task.Id = Task.Id;
        Task.Title = Task.Title;
        Task.CategoryId = Task.CategoryId;
        Task.Category = Category.Name;
        Task.IsCompleted = !Task.IsCompleted;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TaskExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Tasks>> PostTask(Tasks todoDTO)
    {
        var Category = await _context.Categories.FindAsync(todoDTO.CategoryId);

        if (Category == null)
        {
            return NotFound();
            
        }

        var Tasks = new Tasks
        { 
            Id = todoDTO.Id,
            Title = todoDTO.Title,
            CategoryId = todoDTO.CategoryId,
            Category = Category.Name,
            IsCompleted = false,
        };

        _context.Tasks.Add(Tasks);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetAllTask),
            new { id = Tasks.Id },
            ItemToDTO(Tasks));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var Task = await _context.Tasks.FindAsync(id);
        if (Task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(Task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TaskExists(int id)
    {
        return _context.Tasks.Any(e => e.Id == id);
    }

    private static Tasks ItemToDTO(Tasks Task) =>
    new Tasks
    {
        Id = Task.Id,
        Title = Task.Title,
        CategoryId = Task.CategoryId,
        Category = Task.Category,
        IsCompleted = Task.IsCompleted,
    };
}
