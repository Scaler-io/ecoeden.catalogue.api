using AutoMapper;
using Ecoeden.Catalogue.Application.Helpers;
using Ecoeden.Catalogue.Domain.Models.Dtos;

namespace Ecoeden.Catalogue.Application.Mapper;

public sealed class ProductDtoMapper : Profile
{
    public ProductDtoMapper()
    {
        CreateMap<Domain.Entities.Product, ProductDto>()
            .ForMember(d => d.MetaData, o => o.MapFrom(s => new MetaDataDto
            {
                CreatedAt = DateTimeHelper.ConvertUtcToIst(s.CreatedAt).ToString("dd/MM/yyyy HH:mm:ss tt"),
                UpdatedAt = DateTimeHelper.ConvertUtcToIst(s.UpdatedAt).ToString("dd/MM/yyyy HH:mm:ss tt"),
                CreatedBy = s.CreatedBy,
                UpdatedBy = s.UpdatedBy,
            }));
    }
}
