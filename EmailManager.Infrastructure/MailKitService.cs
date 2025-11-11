using EmailManager.Core.Entities;
using EmailManager.Shared;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace EmailManager.Infrastructure;

public interface IMailKitService
{
    Task<List<Email>> FetchEmailsInParallel(Inbox inbox);
}
public partial class MailKitService : IMailKitService
{
    private ImapClient Connect(Inbox inbox)
    {
        ImapClient client = new ImapClient();
        client.Connect(inbox.Host, inbox.Port, inbox.IsSsl);
        client.Authenticate(inbox.Email, inbox.Password);
        return client;
    }
    
    public async Task<List<Email>> FetchEmailsInParallel(Inbox inbox)
    {
        
        Task<List<Email>>[] tasks = new Task<List<Email>>[1];
        List<Email> emailList = new List<Email>();
        int totalEmailCount = await FetchEmailCount(inbox);
        int threadCount = 10;
        
        for (int i = 0; i < threadCount; i++)
        {
            int mailsPerSegment = totalEmailCount / threadCount;
            int start = i * mailsPerSegment;
            int end = start + mailsPerSegment;
            tasks[i] = Task.Run(() => FetchEmailTask((start, end), inbox));
        }

        try
        {
            Task.WaitAll(tasks);

            for (int i = 0; i < 1; i++)
            {
                var emails = await tasks[i];
                emailList.AddRange(emails);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);  
        }
        
        Console.WriteLine("Done");
        return emailList;
    }

    private async Task<int> FetchEmailCount(Inbox inbox)
    {
        using var client = Connect(inbox);
        await client.Inbox.OpenAsync(FolderAccess.ReadOnly);
        return client.Inbox.Count;
    }
    private async Task<List<Email>> FetchEmailTask((int, int) range, Inbox inbox)
    {
        using var client = Connect(inbox);
        await client.Inbox.OpenAsync(FolderAccess.ReadOnly);
        var count = client.Inbox.Count - 1;
        List<Email> emails = new();
        try
        {
            var (curr, end) = range;
            var fetchedEmails =await client.Inbox
                .FetchAsync(count - end, count - curr, MessageSummaryItems.Envelope);
            MapEmail(fetchedEmails, emails);
        }
        catch (Exception ex)                            
        {
            Console.WriteLine(ex);
        }
        
        
        return emails;
    }
    
    //Connect to Email inbox
    
    //Fetch emails with high level information
    
    //Fetch Folders
    
    //Fetch Sub Folders
    
    //Move from one folder to another folder
    
    //Fetch information such as - starred/important/spam?
}