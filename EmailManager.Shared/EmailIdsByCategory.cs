namespace EmailManager.Shared;

public record EmailIdsByCategory(
    string InboxEmailId,
    string Category,
    IEnumerable<string> EmailIds);

    
