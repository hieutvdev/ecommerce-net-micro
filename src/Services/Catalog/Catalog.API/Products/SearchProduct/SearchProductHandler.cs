using Catalog.API.Models;
using JasperFx.Core;

namespace Catalog.API.Products.SearchProduct;

public record SearchProductQuery(string? KeySearch, int? PageNumber = 1, int? PageSize = 10) : IQuery<SearchProductResult>;

public record SearchProductResult(IEnumerable<Product> Products);

public class SearchProductHandler
(IDocumentSession session)
: IQueryHandler<SearchProductQuery, SearchProductResult>
{
    public async Task<SearchProductResult> Handle(SearchProductQuery query, CancellationToken cancellationToken)
    {

        if (query.KeySearch is null)
        {
            IEnumerable<Product> products = await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10 ,cancellationToken);
            return new SearchProductResult(products);
        }
        var searchProduct = await session.Query<Product>().Where(c => c.Name.Contains(query.KeySearch, StringComparison.CurrentCultureIgnoreCase))
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10 ,cancellationToken);
        
        return new SearchProductResult(searchProduct);
    }
}