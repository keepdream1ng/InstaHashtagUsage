using InstaHashtagUsage.ClassLibrary.Mediatr;

namespace InstaHashtagUsage.ClassLibrary.Services
{
	public interface IParsingNotificationService
	{
		event Action<ParsingPublicationsDoneNotification> OnParsingDoneNotificationReceived;
		event EventHandler OnQueueIsProcessedNotificationReceived;

		void NewParsingDoneNotification(ParsingPublicationsDoneNotification notification);
		void QueueIsProcessed(object sender, EventArgs eventArgs);
	}
}