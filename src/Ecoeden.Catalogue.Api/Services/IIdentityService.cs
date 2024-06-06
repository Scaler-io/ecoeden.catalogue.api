using Ecoeden.Catalogue.Domain.Models.Dtos;

namespace Ecoeden.Catalogue.Api.Services;

public interface IIdentityService
{
    UserDto PrepareUser();
}
