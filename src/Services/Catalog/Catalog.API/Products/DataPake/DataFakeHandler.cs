using Catalog.API.Models;

namespace Catalog.API.Products.DataPake;




public class DataFakeHandler
(IDocumentSession session)
: ICommandHandler<DataFakeCommand, DataFakeResult>
{
    public async Task<DataFakeResult> Handle(DataFakeCommand request, CancellationToken cancellationToken)
    {
        List<Product> products = new List<Product>();
        for (int i = 0; i < 10000; i++)
        {
            products.Add(new Product
            {
                
                    Id = Guid.NewGuid(),
                    Name = $"Product {i}",
                    Description = $"{i} This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    ImageFile = $"product-{i}.png",
                    Price =  i,
                    Category = new List<string> { $"{i}" }
                
            });
        }
        
        session.Store(products.ToArray());
        await session.SaveChangesAsync(cancellationToken);

        //return result
        return new DataFakeResult(true);
    }
}

