using InstaHashtagUsage.ClassLibrary.Mediatr;

namespace InstaHashtagUsage.ClassLibrary.Services
{
	public interface IParsingNotificationService
	{
		event Action<ParsingPublicationsDoneNotification> OnParsingDoneNotificationReceived;

		void NewParsingDoneNotification(ParsingPublicationsDoneNotification notification);
	}
}