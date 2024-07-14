namespace Ecoeden.Catalogue.Domain.Models.Dtos;
public sealed class CategoryDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ImageFile { get; set; }
    public string Slug { get; set; }
    public MetaDataDto MetaData { get; set; }
}
