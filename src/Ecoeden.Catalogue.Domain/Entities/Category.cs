using Ecoeden.Catalogue.Domain.Extensions;

namespace Ecoeden.Catalogue.Domain.Entities;
public sealed class Category(string name, string imageFile) : EntityBase
{
    public string Name { get; set; } = name;
    public string ImageFile { get; set; } = imageFile;
    public string Slug { get; set; } = name.GetSlug();
}
