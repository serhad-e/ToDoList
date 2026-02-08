namespace todofinal.Application.Interfaces;
using todofinal.Domain.Entities;
public interface ITodoRepository
{
    Task<List<ToDoTask>> GetAllAsync(); // Tüm listeyi getir
    Task<ToDoTask?> GetByIdAsync(int id); // Tek bir görev getir
    Task AddAsync(ToDoTask task); // Yeni görev ekle
    Task UpdateAsync(ToDoTask task); // Güncelle
    Task DeleteAsync(int id); // Sil
    Task<List<ToDoTask>> GetPagedTasksAsync(int pageNumber, int pageSize);
}