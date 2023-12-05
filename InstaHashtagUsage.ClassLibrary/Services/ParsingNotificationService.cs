using InstaHashtagUsage.ClassLibrary.Mediatr;

namespace InstaHashtagUsage.ClassLibrary.Services;

public class ParsingNotificationService : IParsingNotificationService
{
	// Event to be triggered when a parsing notification is received
	public event Action<ParsingPublicationsDoneNotification> OnParsingDoneNotificationReceived;

	public void NewParsingDoneNotification(ParsingPublicationsDoneNotification notification)
	{
		// Notify subscribers (Blazor components) about the new notification
		OnParsingDoneNotificationReceived?.Invoke(notification);
	}
}
