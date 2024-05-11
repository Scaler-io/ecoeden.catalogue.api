using Ecoeden.Catalogue.Domain.Extensions;

namespace Ecoeden.Catalogue.Domain.Entities;
public sealed class Category(string name) : EntityBase
{
    public string Name { get; set; } = name;
    public string Slug { get; set; } = name.GetSlug();
}
