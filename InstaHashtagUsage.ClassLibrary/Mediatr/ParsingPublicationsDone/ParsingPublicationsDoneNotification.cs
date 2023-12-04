namespace InstaHashtagUsage.ClassLibrary.Mediatr;

public record ParsingPublicationsDoneNotification (string Hashtag, int Count) : INotification;
