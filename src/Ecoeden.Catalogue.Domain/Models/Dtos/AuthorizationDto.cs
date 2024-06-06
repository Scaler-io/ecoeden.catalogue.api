namespace Ecoeden.Catalogue.Domain.Models.Dtos;

public class AuthorizationDto
{
    public IList<string> Roles { get; set; }
    public IList<string> Permissions { get; set; }
    public string Token { get; set; }
}