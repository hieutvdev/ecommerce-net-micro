using FluentValidation;
using Inventory.Application.Usecase.V1.Commands.Warehouses.Create;
using Inventory.Domain.Enums;

namespace Inventory.Application.Usecase.V1.Validations.Warehouses.Create;

public class CreateWarehousesCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehousesCommandValidator()
    {
        // RuleFor(x => x.Request.ManagerId)
        //     .NotEmpty()
        //     .WithMessage("ManagerID cannot be null or whitespace");
        // RuleFor(x => x.Request.WarehouseName)
        //     .NotEmpty()
        //     .WithMessage("Warehouse name cannot be null or empty");
        
        

    }
}