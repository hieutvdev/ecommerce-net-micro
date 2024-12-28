namespace Catalog.API.Products.DataPake;

public record DataFakeCommand() : ICommand<DataFakeResult>;

public record DataFakeResult(bool IsSuccess);