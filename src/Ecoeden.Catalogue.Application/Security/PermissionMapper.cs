using Ecoeden.Catalogue.Application.Contracts.Security;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Domain.Models.Enums;

namespace Ecoeden.Catalogue.Infrastructure.Security;
public class PermissionMapper : IPermissionMapper
{
    public readonly Dictionary<ApiAccess, List<string>> _map = new()
    {
        { ApiAccess.InventoryRead, [ApiAccess.InventoryRead.EnumValueString()] },
        { ApiAccess.ReportRead, [ ApiAccess.ReportRead.EnumValueString()] },
        { ApiAccess.ReportWrite, [ ApiAccess.ReportRead.EnumValueString(), ApiAccess.ReportWrite.EnumValueString()] },
        { ApiAccess.InventoryWrite, [ ApiAccess.InventoryWrite.EnumValueString()] }
    };

    public List<string> GetPermissionsForRole(ApiAccess role)
    {
        return _map[role];
    }
}
