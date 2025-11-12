using EmailManager.Shared;

namespace EmailManager.Core.Entities;

public class EmailDetail
{
    public Guid Id { get; private set; }
    public string SenderAddress { get; private set; } = string.Empty;
    public string SenderName { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public DateTime SentOn { get; private set; }

    public string? Category { get; private set; } = null;
    public Guid InboxId { get; private set; }
    public Inbox Inbox { get; private set; } = null!;

    private EmailDetail(){}
    public EmailDetail(Email email, Guid inboxId)
    {
        Id = Guid.NewGuid();
        SenderAddress = email.EmailHeaderDto.EmailId;
        SenderName = email.EmailHeaderDto.Sender;
        Subject = email.Subject;
        SentOn = email.SentOn;
        InboxId = inboxId;
    }
    
    public void SetCategory(string category) => Category = category;
}

