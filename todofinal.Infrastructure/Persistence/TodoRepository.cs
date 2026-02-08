namespace todofinal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using todofinal.Application.Interfaces;
using todofinal.Domain.Entities;
public class TodoRepository:ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ToDoTask>> GetAllAsync()
    {
        return await _context.ToDoTasks.ToListAsync();
    }

    public async Task<ToDoTask?> GetByIdAsync(int id)
    {
        return await _context.ToDoTasks.FindAsync(id);
    }

    public async Task AddAsync(ToDoTask task)
    {
        await _context.ToDoTasks.AddAsync(task);
        await _context.SaveChangesAsync(); // Değişiklikleri veritabanına işle
    }

    public async Task UpdateAsync(ToDoTask task)
    {
        _context.ToDoTasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var task = await _context.ToDoTasks.FindAsync(id);
        if (task != null)
        {
            _context.ToDoTasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<ToDoTask>> GetPagedTasksAsync(int pageNumber, int pageSize)
    {
        return await _context.ToDoTasks
            .OrderByDescending(x => x.CreatedDate) // En yeni görevler en üstte
            .Skip((pageNumber - 1) * pageSize)    // Önceki sayfaları atla
            .Take(pageSize)                       // İstenen miktar kadar al
            .ToListAsync();    }

    public async Task<List<ToDoTask>> GetTasksByUserIdAsync(int userId)
    {
        return await _context.ToDoTasks
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }
}