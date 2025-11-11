using EmailManager.Application;
using EmailManager.Core.Entities;

namespace EmailManager.API;

public static class EmailManagerEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");
        app.MapPost("/registerEmailInbox", async (EmailManager.Shared.ConnectionInfo request, IEmailManagerService service) =>
        {
           await service.RegisterEmailInbox(request);
        });
        app.MapGet("/fetchUsageOverview", () => "");
        app.MapGet("/emailcount", () => "Hello World!");
    }
}