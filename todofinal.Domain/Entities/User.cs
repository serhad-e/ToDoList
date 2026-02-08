namespace todofinal.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // Şifreyi şifrelenmiş tutacağız
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // İlişki: Bir kullanıcının birden fazla görevi olabilir
    public ICollection<ToDoTask> Tasks { get; set; } = new List<ToDoTask>();
}