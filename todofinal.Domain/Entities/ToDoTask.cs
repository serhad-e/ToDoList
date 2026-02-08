namespace todofinal.Domain.Entities;

public class ToDoTask
{
    public int Id { get; set; } // Birincil anahtar
    public string Title { get; set; } = string.Empty; // Görev başlığı
    public string? Description { get; set; } // Opsiyonel açıklama
    public bool IsCompleted { get; set; } // Tamamlandı mı?
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Oluşturulma tarihi
    
    public int UserId { get; set; } // Foreign Key
    public User User { get; set; } = null!; // Navigation Property
}   