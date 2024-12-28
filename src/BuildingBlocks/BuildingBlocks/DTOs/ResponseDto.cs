namespace BuildingBlocks.DTOs;

public record ResponseDto(
    object? MetaData  = null,
    bool IsSuccess = true,
    string Message = "");
