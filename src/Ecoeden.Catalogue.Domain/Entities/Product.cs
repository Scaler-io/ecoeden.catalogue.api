using Ecoeden.Catalogue.Domain.Extensions;

namespace Ecoeden.Catalogue.Domain.Entities;
public class Product : EntityBase
{
    public Product(string name, string description, string category, decimal price)
    {
        Name = name;
        Description = description;
        Category = category;
        Price = price;
        Slug = name.GetSlug();
        SKU = Id.GetSku();
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public string Slug { get; set; }
    public string SKU { get; set; }
}
