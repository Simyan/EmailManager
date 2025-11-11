using EmailManager.Core.Entities;
using EmailManager.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EmailManager.Infrastructure;

public class EmailManagerDbContext : DbContext
{
    public EmailManagerDbContext(DbContextOptions<EmailManagerDbContext> options) : base(options)
    {}

    public DbSet<Inbox> Inboxes => Set<Inbox>();
    public DbSet<EmailDetail> EmailDetails => Set<EmailDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmailManagerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}