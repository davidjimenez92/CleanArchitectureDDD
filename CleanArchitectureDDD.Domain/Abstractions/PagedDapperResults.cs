using System.Collections;

namespace CleanArchitectureDDD.Domain.Abstractions;

public class PagedDapperResults<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public PagedDapperResults(int totalItems, int pageNumber = 1, int pageSize = 10)
    {
        TotalItems = totalItems;
        PageNumber = pageNumber;
        PageSize = pageSize;
        var mod = TotalItems % PageSize;
        TotalPages = TotalItems / PageSize + (mod == 0 ? 0 : 1);
    }
}