using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todofinal.Application.Interfaces;
using todofinal.Application.DTOs;
using System.Security.Claims; // Mutlaka ekle!
namespace todofinal.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Artık bu kapıdan sadece tokenı olanlar geçebilir!
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    // Sadece ITodoService kalmalı, diğerlerini sildik
    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }
    
    // YENİ: Token içinden NameIdentifier (ID) bilgisini çeken yardımcı metod
    private int GetUserIdFromToken()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) return 0;
        return int.Parse(claim.Value);
    }
    [HttpGet("list")]
    public async Task<IActionResult> GetAll()
    {
        
        // Senin yazdığın yardımcı metodu burada kullanıyoruz
        int userId = GetUserIdFromToken();

        // Servis katmanına "Sadece bu ID'ye ait olanları getir" diyoruz
        var result = await _todoService.GetTasksByUserIdAsync(userId); 
        return Ok(result);
    }
    [HttpGet("list-paged")]
    public async Task<IActionResult> GetPaged([FromQuery] PaginationParamsDto paginationParams)
    {
        var result = await _todoService.GetPagedTasksAsync(paginationParams);
        return Ok(result);
    }

    [HttpGet("list/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        int userId = GetUserIdFromToken();
        var result = await _todoService.GetTaskByIdAsync(id, userId);
    
        if (result == null) return NotFound(new { message = "Görev bulunamadı!" });
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateTodoDto dto)
    {
        // Token içindeki 'NameIdentifier' (ID) bilgisini çekiyoruz
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
    
        if (userIdClaim == null) 
            return Unauthorized("Kullanıcı bilgisi token içerisinde bulunamadı.");

        int userId = int.Parse(userIdClaim.Value);

        // Servis metoduna hem DTO'yu hem de token'dan gelen userId'yi gönderiyoruz
        var taskId = await _todoService.CreateTaskAsync(dto, userId);
    
        return Ok(new { id = taskId, message = "Görev başarıyla oluşturuldu." });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = GetUserIdFromToken();
        // Servis artık sadece bu ID'li görevi DEĞİL, bu kullanıcıya ait olanı silecek
        var success = await _todoService.DeleteTaskAsync(id, userId); 
    
        if (!success) return NotFound(new { message = "Görev bulunamadı veya bu işlem için yetkiniz yok!" });
        return Ok(new { message = "Görev başarıyla silindi." });
    }

    // TodoController.cs içinde
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateTodoDto dto)
    {
        // Token'dan giriş yapan kişinin ID'sini alıyoruz
        int userId = GetUserIdFromToken();

        // Servise hem güncellenecek veriyi hem de bu kişinin ID'sini gönderiyoruz
        var success = await _todoService.UpdateTaskAsync(dto, userId);

        if (!success) 
            return NotFound(new { message = "Görev bulunamadı veya bu görevi güncelleme yetkiniz yok!" });

        return Ok(new { message = "Görev başarıyla güncellendi!" });
    }
}