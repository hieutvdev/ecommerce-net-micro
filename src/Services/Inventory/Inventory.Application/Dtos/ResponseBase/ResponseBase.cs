namespace Inventory.Application.Dtos.ResponseBase;

public record ResponseBase(object? Metadata, string Message = "", bool IsSuccess = true);