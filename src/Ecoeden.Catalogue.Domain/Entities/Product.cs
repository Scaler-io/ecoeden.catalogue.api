using Ecoeden.Catalogue.Domain.Extensions;

namespace Ecoeden.Catalogue.Domain.Entities;
public class Product(string name, string description, string category, decimal price, string imageFile) : EntityBase
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public string Category { get; set; } = category;
    public decimal Price { get; set; } = price;
    public string ImageFile { get; set; } = imageFile;
    public string Slug { get; set; } = name.GetSlug();
    public string SKU { get; set; } = DomainExtensions.GetSku();
}
