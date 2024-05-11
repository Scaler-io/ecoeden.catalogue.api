namespace Ecoeden.Catalogue.Domain.Configurations;
public sealed class MongoDbOption
{
    public const string OptionName = "MongoDb";
    public string ConnectionString { get; set; }
    public string Database { get; set; }
}
