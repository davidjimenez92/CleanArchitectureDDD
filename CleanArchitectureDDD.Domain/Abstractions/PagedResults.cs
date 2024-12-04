namespace CleanArchitectureDDD.Domain.Abstractions;

public class PagedResults<TEntity, TEntityId>
where TEntity: Entity<TEntityId>
where TEntityId : class
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalNumberOfPages { get; set; }
    public int TotalNUmberOfRecords { get; set; }
    public IList<TEntity> Records { get; set; } = new List<TEntity>();
}