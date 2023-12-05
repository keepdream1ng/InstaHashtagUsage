
using InstaHashtagUsage.ClassLibrary.Services;

namespace InstaHashtagUsage.ClassLibrary.Mediatr;

public class ParsingPublicationDoneHandler : INotificationHandler<ParsingPublicationsDoneNotification>
{
	private readonly IParsingNotificationService _notificationService;

	public ParsingPublicationDoneHandler(IParsingNotificationService notificationService)
    {
		_notificationService = notificationService;
	}
    public Task Handle(ParsingPublicationsDoneNotification notification, CancellationToken cancellationToken)
	{
		_notificationService.NewParsingDoneNotification(notification);
		return Task.CompletedTask;
	}
}
