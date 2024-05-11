using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecoeden.Catalogue.Domain.Entities;
public class EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = "Default";
    public string UpdatedBy { get; set; } = "Default";
}
