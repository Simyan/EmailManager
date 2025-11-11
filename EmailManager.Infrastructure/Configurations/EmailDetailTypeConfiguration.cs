using EmailManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailManager.Infrastructure.Configurations;

public class EmailDetailTypeConfiguration : IEntityTypeConfiguration<EmailDetail>
{
    public void Configure(EntityTypeBuilder<EmailDetail> builder)
    {
        builder.ToTable("EmailDetail");
        builder.HasKey(i => i.Id);
    }
}