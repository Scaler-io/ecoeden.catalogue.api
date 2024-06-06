using System.Runtime.Serialization;

namespace Ecoeden.Catalogue.Application.Extensions;
public static class EnumExtensions
{
    public static string EnumValueString(this Enum enumValue)
    {
        Type enumType = enumValue.GetType();
        string enumName = enumValue.ToString();

        var memberInfo = enumType.GetMember(enumName);
        if (memberInfo.Length > 0)
        {
            var enumMemberAttribute = memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);
            if (enumMemberAttribute.Length > 0)
            {
                if (enumMemberAttribute[0] is EnumMemberAttribute enumMember && enumMember.Value != null)
                {
                    return enumMember.Value;
                }
            }
        }

        return enumName;
    }
}
