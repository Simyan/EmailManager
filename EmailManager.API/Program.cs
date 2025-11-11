using EmailManager.API;
using EmailManager.Application;
using Microsoft.EntityFrameworkCore;
using EmailManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmailManagerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("EmailManagerDbConnection")));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IEmailManagerService, EmailManagerService>();
builder.Services.AddScoped<IMailKitService, MailKitService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

EmailManagerEndpoints.Map(app);

app.Run();
