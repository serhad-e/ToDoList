namespace todofinal.Application.DTOs;

public class TodoStatsDto
{
    public int ToplamGorevSayisi { get; set; }
    public int TamamlananGorevSayisi { get; set; }
    public int BekleyenGorevSayisi { get; set; }
    public double TamamlanmaYuzdesi { get; set; }
}