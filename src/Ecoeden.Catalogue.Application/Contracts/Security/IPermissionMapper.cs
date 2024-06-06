using Ecoeden.Catalogue.Domain.Models.Enums;

namespace Ecoeden.Catalogue.Application.Contracts.Security;
public interface IPermissionMapper
{
    List<string> GetPermissionsForRole(ApiAccess role);
}
