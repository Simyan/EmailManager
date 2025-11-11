namespace EmailManager.Core.Entities;

public class Inbox
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Host { get; private set; } = string.Empty;
    public string Password { get; private set; }  = string.Empty;
    public int Port { get; private set; }
    public bool IsSsl { get; private set; }
    
    public bool IsProcessed { get; private set; }
    
    public ICollection<EmailDetail>  EmailDetails { get; } = new List<EmailDetail>();

    public Inbox(string email, string host, string password, int port, bool isSsl)
    {
        Id = Guid.NewGuid();
        Email = email;
        Host = host;
        Password = password;
        Port = port;
        IsSsl = isSsl;
        IsProcessed = false;
    }

    private Inbox() { }

    public void MarkAsProcessed() => IsProcessed = true;
}