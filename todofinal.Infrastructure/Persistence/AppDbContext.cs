using Microsoft.EntityFrameworkCore;
namespace todofinal.Infrastructure.Persistence;
using todofinal.Domain.Entities;
public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ToDoTask> ToDoTasks { get; set; } // Veritabanındaki tabloyu temsil eder
    public DbSet<User> Users { get; set; }
}