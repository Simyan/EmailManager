namespace EmailManager.Shared;

public record EmailOverview(
        string Address,
        string Name,
        DateTime FirstSentOn,
        DateTime LastSentOn,
        int Frequency,
        IEnumerable<string> SampleSubjects);