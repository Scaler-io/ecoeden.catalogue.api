using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecoeden.Catalogue.Domain.Entities;
public class EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CreatedBy { get; private set; } = "Default";
    public string UpdatedBy { get; private set; } = "Default";

    public void UpdateCreationData(string userId)
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        CreatedBy = userId;
    }

    public void UpdateUpdationData(string userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }
}
