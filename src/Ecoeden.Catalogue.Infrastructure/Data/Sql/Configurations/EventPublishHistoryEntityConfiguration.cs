using Ecoeden.Catalogue.Domain.Models.Enums;
using Ecoeden.Catalogue.Domain.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecoeden.Catalogue.Infrastructure.Data.Sql.Configurations;
internal class EventPublishHistoryEntityConfiguration : IEntityTypeConfiguration<EventPublishHistory>
{
    public void Configure(EntityTypeBuilder<EventPublishHistory> builder)
    {
        builder.ToTable("EventPublishHistories");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.EventType).IsRequired();
        builder.Property(c => c.Data).IsRequired();
        builder.Property(c => c.FailureSource).IsRequired();
        builder.Property(c => c.EventStatus)
            .IsRequired()
            .HasConversion(o => o.ToString(), o => (EventStatus)Enum.Parse(typeof(EventStatus), o));
    }
}
