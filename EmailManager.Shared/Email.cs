namespace EmailManager.Shared;

public class Email
{
    public EmailHeader EmailHeaderDto { get; set; }
    public string TextBody { get; set; }
    public string HtmlBody { get; set; }
    public string From { get; set; }
    
    public string Subject { get; set; }
    public DateTime SentOn { get; set; }

        
    public class EmailHeader
    {
        public string ReturnEmail { get; set; }
        public string UnsubscribeLink { get; set; }
        public string Id { get; set; }
        public string EmailId { get; set; }
        public string SentOn { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
    }
}
