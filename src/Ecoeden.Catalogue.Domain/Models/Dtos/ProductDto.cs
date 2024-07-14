namespace Ecoeden.Catalogue.Domain.Models.Dtos;

public sealed class ProductDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string ImageFile { get; set; }
    public decimal Price { get; set; }
    public string Slug { get; set; }
    public string SKU { get; set; }
    public MetaDataDto MetaData { get; set; }
}
