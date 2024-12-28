using AutoMapper;
using Inventory.Application.Dtos.Warehouses.Requests;

namespace Inventory.Application.Mappers;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<CreateWarehouseRequest, Warehouse>().ReverseMap();
    }
}