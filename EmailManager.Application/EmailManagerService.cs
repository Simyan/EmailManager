using EmailManager.Core.Entities;
using EmailManager.Infrastructure;
using EmailManager.Shared;
using Microsoft.EntityFrameworkCore;

namespace EmailManager.Application;

public interface IEmailManagerService
{
    public Task RegisterEmailInbox(ConnectionInfo connectionInfo);
    public Task<List<EmailOverview>> FetchTopMostReceivedEmails(string emailId);
    public Task CategorizeEmails(EmailIdsByCategory emailIdsByCategory);
}

public interface IEmailManagerServiceForProcessor
{
    public Task ProcessPendingRequests();
}
public class EmailManagerService : IEmailManagerService,  IEmailManagerServiceForProcessor
{
    private IMailKitService _mailKitService;
    private readonly EmailManagerDbContext _dbContext;
    public EmailManagerService(IMailKitService mailKitService, EmailManagerDbContext dbContext)
    {
        _mailKitService = mailKitService;
        _dbContext = dbContext;
    }

    public async Task CategorizeEmails(EmailIdsByCategory emailIdsByCategory)
    {
        var entities = await _dbContext.EmailDetails
            .Where(x => 
                emailIdsByCategory.EmailIds.Contains(x.SenderAddress) &&
                x.Inbox.Email == emailIdsByCategory.InboxEmailId)
            .ToListAsync();

        foreach (var entity in entities)
        {
            entity.SetCategory(emailIdsByCategory.Category);
        }

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<EmailOverview>> FetchTopMostReceivedEmails(string emailId)
    {
        return await _dbContext
            .EmailDetails
            .Where(x => x.Inbox.Email == emailId)
            .GroupBy(x => new { x.SenderAddress, x.SenderName })
            .Select(x => new EmailOverview
            (
                x.Key.SenderAddress,
                x.Key.SenderName, 
                x.Min(s => s.SentOn),
                x.Max(s => s.SentOn),
                x.Count(),
                x
                    .OrderByDescending(o => o.SentOn)
                    .Select(s => s.Subject)
                    .Take(5)
            ))
            .OrderByDescending(o => o.Frequency)
            .Take(50)
            .ToListAsync();
    }

    public async Task RegisterEmailInbox(ConnectionInfo connectionInfo)
    {
        //take email connection info from api request and make a new record entry in db
        //mark this record as pending so that it can be processed later

        var item = await _dbContext.Inboxes.FirstOrDefaultAsync(x => x.Email == connectionInfo.Email);
        if (item != null)
        {
            return;
        }
        
        Inbox entity = new Inbox(
            connectionInfo.Email, 
            connectionInfo.Host, 
            connectionInfo.Password, 
            connectionInfo.Port, 
            connectionInfo.IsSSL);
        await _dbContext.Inboxes.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task ProcessPendingRequests()
    {
        var items = await _dbContext.Inboxes
                                .Where(x => !x.IsProcessed).ToListAsync();

        foreach (var item in items)
        {
            await StoreEmailOverview(item);
            item.MarkAsProcessed();
        }
    }

    private async Task StoreEmailOverview(Inbox inbox)
    {
        var emails = await _mailKitService.FetchEmailsInParallel(inbox);
        var emailDetails = emails.Select(x => new EmailDetail(x, inbox.Id)).ToList();
        await _dbContext.EmailDetails.AddRangeAsync(emailDetails);
        await _dbContext.SaveChangesAsync();
    }
}