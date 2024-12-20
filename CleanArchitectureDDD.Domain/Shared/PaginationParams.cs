namespace CleanArchitectureDDD.Domain.Shared;

public record PaginationParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 2;
    public int PageIndex { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string? OrderBy { get; set; }
    public bool OrderAsc { get; set; } = true;
    public string? Search { get; set; }
}