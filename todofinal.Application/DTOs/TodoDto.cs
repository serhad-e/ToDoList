namespace todofinal.Application.DTOs;

public class TodoDto
{
    public int Id { get; set; } // Birincil anahtar
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; } // Tamamlandı mı?
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Oluşturulma tarihi
    
    
    
}