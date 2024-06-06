using Ecoeden.Catalogue.Domain.Models.Dtos;

namespace Ecoeden.Catalogue.Domain.Models.Core;
public sealed class RequestInformation
{
    public UserDto CurrentUser { get; set; }
    public string CorrelationId { get; set; }
}
