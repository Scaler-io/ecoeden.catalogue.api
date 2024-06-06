using System.Data;

namespace Ecoeden.Catalogue.Domain.Models.Dtos;
public sealed class UserDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public AuthorizationDto AuthorizationDto { get; set; }

    public bool IsAdmin()
    {
        return AuthorizationDto.Roles.Contains("Admin");
    }
}
