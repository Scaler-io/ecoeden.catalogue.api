using System.Text.RegularExpressions;

namespace Ecoeden.Catalogue.Domain.Extensions;
public static class DomainExtensions
{
    public static string GetSlug(this string input)
    {
        string slug = input.Trim().ToLower();

        // Replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s+", "-");

        // Remove special characters except hyphens
        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

        // Remove consecutive hyphens
        slug = Regex.Replace(slug, @"-+", "-");

        // Remove leading/trailing hyphens
        slug = slug.Trim('-');

        return slug;
    }

    public static string GetSku(this string input)
    {
        return "ECO" + input.Substring(input.Length - 6) + new Random().Next(100, 999);
    }
}
