using AutoMapper;
using Contracts.Events;

namespace Ecoeden.Catalogue.Application.Mapper;
public class ProductEventMapper : Profile
{
    public ProductEventMapper()
    {
        CreateMap<Domain.Entities.Product, ProductCreated>()
            .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.LastUpdatedOn, o => o.MapFrom(s => s.UpdatedAt));
    }
}