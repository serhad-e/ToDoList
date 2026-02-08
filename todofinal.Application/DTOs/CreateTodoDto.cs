namespace todofinal.Application.DTOs;

public class CreateTodoDto
{
    // Kullanıcıdan sadece bu ikisini bekliyoruz
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int UserId { get; set; } // Bu satırı ekle!
}