using EmailManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailManager.Infrastructure.Configurations;

public class InboxEntityTypeConfiguration :  IEntityTypeConfiguration<Inbox>
{
    public void Configure(EntityTypeBuilder<Inbox> builder)
    {
        builder.ToTable("Inbox");
        builder.HasKey(i => i.Id);
    }
}