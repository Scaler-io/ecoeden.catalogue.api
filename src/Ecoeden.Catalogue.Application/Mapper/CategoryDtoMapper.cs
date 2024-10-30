using AutoMapper;
using Ecoeden.Catalogue.Application.Helpers;
using Ecoeden.Catalogue.Domain.Entities;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using System.Globalization;

namespace Ecoeden.Catalogue.Application.Mapper;
public class CategoryDtoMapper : Profile
{
    public CategoryDtoMapper()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(d => d.MetaData, o => o.MapFrom(s => new MetaDataDto
            {
                CreatedAt = new DateTimeOffset(DateTimeHelper.ConvertUtcToIst(s.CreatedAt), TimeSpan.Zero).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                UpdatedAt = new DateTimeOffset(DateTimeHelper.ConvertUtcToIst(s.UpdatedAt), TimeSpan.Zero).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                CreatedBy = s.CreatedBy,
                UpdatedBy = s.UpdatedBy,
            }));
    }
}
