
using InstaHashtagUsage.ClassLibrary.Services;

namespace InstaHashtagUsage.ClassLibrary.Mediatr;

public class QueueIsProcessedHandler : INotificationHandler<QueueIsProcessedNotification>
{
	private readonly IParsingNotificationService _notificationService;
	private readonly ILogger<QueueIsProcessedHandler> _logger;

	public QueueIsProcessedHandler(
		IParsingNotificationService notificationService,
		ILogger<QueueIsProcessedHandler> logger
		)
    {
		_notificationService = notificationService;
		_logger = logger;
	}
    public Task Handle(QueueIsProcessedNotification notification, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Queue processing {status}", "done");
		_notificationService.QueueIsProcessed(notification, EventArgs.Empty);
		return Task.CompletedTask;
	}
}
