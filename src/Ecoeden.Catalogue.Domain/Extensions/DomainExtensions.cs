using System.Text.RegularExpressions;

namespace Ecoeden.Catalogue.Domain.Extensions;
public static class DomainExtensions
{
    public static string GetSlug(this string input)
    {
        string slug = input.Trim();

        // Replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s+", "-");

        // Remove consecutive hyphens
        slug = Regex.Replace(slug, @"-+", "-");

        // Conve 
        slug = Regex.Replace(slug, @"([a-z])([A-Z])", "$1-$2").ToLower();

        slug = Regex.Replace(slug, @"([a-zA-Z])([0-9])", "$1-$2").ToLower();

        // Remove leading/trailing hyphens
        slug = slug.Trim('-');

        return slug;
    }

    public static string GetSku(this string input)
    {
        return "ECO" + input.Substring(input.Length - 6) + new Random().Next(100, 999);
    }
}
