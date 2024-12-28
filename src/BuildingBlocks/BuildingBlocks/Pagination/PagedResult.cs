using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Pagination;

public class PagedResult<T>
{
    public const int UpperPageSize = 100;
    public const int DefaultPageSize = 15;
    public const int DefaultPageIndex = 1;



    private PagedResult(List<T> items, int pageIndex, int pageSize, int totalCount)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    private List<T> Items { get; }
    private int PageIndex { get; }
    private int PageSize { get; }
    private int TotalCount { get; }

    public bool HasNextPage => PageIndex * PageSize < TotalCount;
    public bool HasPreviousPage => PageIndex > 1;


    public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> query, int pageIndex, int pageSize)
    {
        pageIndex = pageSize <= 0 ? DefaultPageIndex : pageIndex;
        pageSize = pageSize <= 0 ? DefaultPageSize : pageSize > UpperPageSize ? UpperPageSize : pageSize;


        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return new(items, pageIndex, pageSize, totalCount);
    }

    public static PagedResult<T> Create(List<T> items, int pageIndex, int pageSize, int totalCount)
        => new(items, pageIndex, pageSize, totalCount);
    
    
    
}