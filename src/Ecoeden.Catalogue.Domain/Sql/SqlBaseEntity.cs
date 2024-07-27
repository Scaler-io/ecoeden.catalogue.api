namespace Ecoeden.Catalogue.Domain.Sql;
public class SqlBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    public string CorrelationId { get; set; }
}
