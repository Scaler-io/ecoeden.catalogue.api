using System.Runtime.Serialization;

namespace Ecoeden.Catalogue.Domain.Models.Enums;
public enum ApiAccess
{
    [EnumMember(Value = "report:read")]
    ReportRead,
    [EnumMember(Value = "inventory:read")]
    InventoryRead,
    [EnumMember(Value = "report:write")]
    ReportWrite,
    [EnumMember(Value = "inventory:write")]
    InventoryWrite
}
