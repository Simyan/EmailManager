namespace EmailManager.Shared;

public class ConnectionInfo
{
    public string Host { get; set; }
    public int Port { get; set; }
    public bool IsSSL { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
