using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.Infrastructure.Data.SqlServer.EFConfigurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.Property(entity => entity.Channel)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(entity => entity.Status)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.OwnsOne(entity => entity.Message, templateBuilder =>
        {
            templateBuilder.Property(entity => entity.Value)
                           .IsRequired()
                           .HasMaxLength(1500)
                           .IsUnicode();
        });

        builder.OwnsMany(entity => entity.Parameters, parameterBuilder =>
        {
            parameterBuilder.Property(entity => entity.Order)
                            .IsRequired();

            parameterBuilder.Property(entity => entity.Value)
                            .IsRequired()
                            .HasMaxLength(200)
                            .IsUnicode();
        });

        builder.Property(entity => entity.Data)
               .IsRequired()
               .HasJsonPropertyName(nameof(Data))
               .IsUnicode();

        builder.Property(entity => entity.RequestedBy)
               .IsRequired()
               .HasMaxLength(25)
               .IsUnicode(false);

        builder.Property(entity => entity.RequestedAt)
               .IsRequired();
    }
}