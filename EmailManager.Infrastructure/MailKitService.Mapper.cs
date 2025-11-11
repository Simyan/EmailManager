
using EmailManager.Shared;
using MailKit;
using MimeKit;

namespace EmailManager.Infrastructure;

public partial class MailKitService
{
    private void MapEmail(IList<IMessageSummary> fetchedEmails, List<Email> emails)
    {
        foreach (var email in fetchedEmails)
        {
            var item = email.Envelope;
            Email emailDto = new();
            emailDto.From = item.From.ToString();
            emailDto.SentOn = item.Date.Value.UtcDateTime;
            emailDto.Subject = item.Subject;
            emailDto.EmailHeaderDto = new();
            emailDto.EmailHeaderDto.EmailId = item.From.ToString();
            emailDto.EmailHeaderDto.Sender = item.Sender.ToString();
            
            emails.Add(emailDto);
        }
    }
    
    private void MapEmail(MimeMessage msg, List<Email> emails)
    {
        Email emailDto = new Email();
        emailDto.EmailHeaderDto = new Email.EmailHeader();
        emailDto.TextBody = msg.TextBody;
        emailDto.HtmlBody = msg.HtmlBody;
        emailDto.From = msg.From.ToString();
        emailDto.SentOn = msg.Date.DateTime;
        emailDto.SentOn = msg.Date.DateTime;
        MapHeaders(msg.Headers, emailDto);
        emails.Add(emailDto);

    }
    private void MapHeaders(HeaderList headers, Email emailDto)
    {
           
        foreach (Header header in headers)
        {
            switch (header.Field)
            {

                case "Reply-To":
                    emailDto.EmailHeaderDto.ReturnEmail = header.Value;
                    break;
                case "List-Unsubscribe":
                    emailDto.EmailHeaderDto.UnsubscribeLink = header.Value;
                    break;
                case "Message-ID":
                    emailDto.EmailHeaderDto.Id = header.Value;
                    break;
                case "From":
                    emailDto.EmailHeaderDto.EmailId = header.Value;
                    break;
                case "Date":
                    emailDto.EmailHeaderDto.SentOn = header.Value;
                    break;
                case "Subject":
                    emailDto.EmailHeaderDto.Subject = header.Value;
                    break;
                case "Sender":
                    emailDto.EmailHeaderDto.Sender = header.Value;
                    break;
                default:
                    break;
            }
        }
    }
    
}