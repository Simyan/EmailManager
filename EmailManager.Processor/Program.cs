using EmailManager.Application;
using EmailManager.Processor;
using Microsoft.EntityFrameworkCore;
using EmailManager.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IEmailManagerServiceForProcessor, EmailManagerService>();
builder.Services.AddSingleton<IMailKitService, MailKitService>();


builder.Services.AddDbContext<EmailManagerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("EmailManagerDbConnection")), ServiceLifetime.Singleton);


var host = builder.Build();
host.Run();