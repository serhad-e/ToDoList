namespace todofinal.Application.DTOs;

public class PaginationParamsDto
{
    private const int MaxPageSize = 50; // Bir seferde en fazla 50 kayıt verilsin
    public int PageNumber { get; set; } = 1; // Varsayılan 1. sayfa
    
    private int _pageSize = 10; // Varsayılan 10 kayıt
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}