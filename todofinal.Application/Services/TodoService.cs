using todofinal.Application.DTOs;
using AutoMapper;
using todofinal.Application.Validators;
using todofinal.Domain.Entities;
namespace todofinal.Application.Services;
using todofinal.Application.Interfaces;
using FluentValidation;
public class TodoService:ITodoService
{
    private readonly ITodoRepository _todoRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTodoDto> _validator;
    public TodoService(ITodoRepository todoRepository, IMapper mapper,IValidator<CreateTodoDto> validator)
    {
        _todoRepository = todoRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<List<TodoDto>> GetAllTasksAsync()
    {
        var tasks = await _todoRepository.GetAllAsync();
        return _mapper.Map<List<TodoDto>>(tasks);
    }

    public async Task<TodoDto?> GetTaskByIdAsync(int id)
    {
        var task = await _todoRepository.GetByIdAsync(id);
        return _mapper.Map<TodoDto>(task);
    }

    public async Task<int> CreateTaskAsync(CreateTodoDto createTodoDto,int userId)
    {
        // 1. Doğrulama (Validation)
        var validationResult = await _validator.ValidateAsync(createTodoDto);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        // 2. Map işlemi
        var task = _mapper.Map<ToDoTask>(createTodoDto);
    
        // 3. ATAMA: UserId artık parametreden geliyor, DTO'dan değil!
        task.UserId = userId; 
    
        task.IsCompleted = false; 
        task.CreatedDate = DateTime.UtcNow; 
    
        await _todoRepository.AddAsync(task);
        return task.Id;
    }

    public async Task<bool> UpdateTaskAsync(UpdateTodoDto dto, int userId)
    {
        // 1. Önce görevi veritabanından buluyoruz
        var existingTask = await _todoRepository.GetByIdAsync(dto.Id);

        // 2. KONTROL: Görev yoksa VEYA görevin sahibi giriş yapan kişi değilse işlemi reddet!
        if (existingTask == null || existingTask.UserId != userId)
        {
            return false;
        }

        // 3. Eğer her şey yolundaysa verileri güncelle
        _mapper.Map(dto, existingTask); // DTO'daki yeni bilgileri mevcut nesneye aktar
    
        await _todoRepository.UpdateAsync(existingTask);
        return true;
    }

    public async Task<List<TodoDto>> GetPagedTasksAsync(PaginationParamsDto paginationParams)
    {
        var tasks = await _todoRepository.GetPagedTasksAsync(
            paginationParams.PageNumber, 
            paginationParams.PageSize);
        
        return _mapper.Map<List<TodoDto>>(tasks);    }

    // Delete metodu bool dönmeli
    public async Task<bool> DeleteTaskAsync(int id, int userId)
    {
        var task = await _todoRepository.GetByIdAsync(id);
    
        // KONTROL: Görev yoksa VEYA sahibi giriş yapan kullanıcı değilse silme!
        if (task == null || task.UserId != userId) 
            return false; 

        await _todoRepository.DeleteAsync(id);
        return true; 
    }
    public async Task<TodoDto?> GetTaskByIdAsync(int id, int userId)
    {
        var task = await _todoRepository.GetByIdAsync(id);
    
        // Eğer görev başkasına aitse null dön (sanki hiç yokmuş gibi)
        if (task == null || task.UserId != userId) return null;
    
        return _mapper.Map<TodoDto>(task);
    }
    // TodoService.cs
    public async Task<List<TodoDto>> GetTasksByUserIdAsync(int userId)
    {
        var tasks = await _todoRepository.GetAllAsync();
        // Veritabanındaki tüm görevleri çekip, sadece bu kullanıcıya ait olanları seçiyoruz
        var userTasks = tasks.Where(x => x.UserId == userId).ToList();
    
        return _mapper.Map<List<TodoDto>>(userTasks);
    }

    public async Task<TodoStatsDto> GetStatsByUserIdAsync(int userId)
    {
        var tasks = await _todoRepository.GetTasksByUserIdAsync(userId);

        var stats = new TodoStatsDto
        {
            ToplamGorevSayisi = tasks.Count,
            TamamlananGorevSayisi = tasks.Count(t => t.IsCompleted),
            BekleyenGorevSayisi = tasks.Count(t => !t.IsCompleted)
        };

        stats.TamamlanmaYuzdesi = stats.ToplamGorevSayisi > 0 
            ? Math.Round((double)stats.TamamlananGorevSayisi / stats.ToplamGorevSayisi * 100, 2) 
            : 0;

        return stats;
    }
}