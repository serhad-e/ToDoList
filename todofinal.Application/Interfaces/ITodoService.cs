namespace todofinal.Application.Interfaces;
using todofinal.Application.DTOs;
public interface ITodoService
{
    Task<List<TodoDto>> GetAllTasksAsync();
    Task<TodoDto?> GetTaskByIdAsync(int id);
    Task<int> CreateTaskAsync(CreateTodoDto dto,int userId);
    // GÜNCELLEME: Delete metodu artık sadece sahibinin silebilmesi için userId almalı
    Task<bool> DeleteTaskAsync(int id, int userId);
    Task<bool> UpdateTaskAsync(UpdateTodoDto updateTodoDto, int userId);
    Task<List<TodoDto>> GetPagedTasksAsync(PaginationParamsDto paginationParams);
    // YENİ: Sadece belirli bir kullanıcıya ait görevleri getiren metot
    Task<TodoDto?> GetTaskByIdAsync(int id, int userId); // userId ekledik
    Task<List<TodoDto>> GetTasksByUserIdAsync(int userId);
    Task<TodoStatsDto> GetStatsByUserIdAsync(int userId);
}