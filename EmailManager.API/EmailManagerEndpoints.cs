using EmailManager.Application;
using EmailManager.Core.Entities;

namespace EmailManager.API;

public static class EmailManagerEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/", () => "Hello I am EmailManager!");
        app.MapPost("/registerEmailInbox", 
            async (
                EmailManager.Shared.ConnectionInfo request, 
                IEmailManagerService service) =>
        {
           await service.RegisterEmailInbox(request);
        });
        app.MapGet("/fetchTopMostReceivedEmails", 
            async (
                    string emailId, IEmailManagerService service) => 
                    await service.FetchTopMostReceivedEmails(emailId)
                   );
        app.MapGet("/emailcount", () => "Hello World!");
        app.MapPost("/categorizeEmails", 
            async (
                EmailManager.Shared.EmailIdsByCategory request, 
                IEmailManagerService service) =>
            {
                await service.CategorizeEmails(request);
            });
    }
}