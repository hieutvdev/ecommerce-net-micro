using AutoMapper;
using Catalog.API.Models;
using Catalog.API.Models.Command;

namespace Catalog.API.Mapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<CreateCommand, Product>().ReverseMap();
    }
}