using InstaHashtagUsage.ClassLibrary.Services;

namespace InstaHashtagUsage.ClassLibrary.Mediatr;

public class NewInputHandler : INotificationHandler<NewInputNotification>
{
	private readonly ILogger<NewInputHandler> _logger;
	private readonly IHashtagQueue _hashtagQueue;
	private readonly IMediator _mediatr;

	public NewInputHandler(
        ILogger<NewInputHandler> logger,
		IHashtagQueue hashtagQueue,
		IMediator mediatr
        )
    {
		_logger = logger;
		_hashtagQueue = hashtagQueue;
		_mediatr = mediatr;
	}

	public async Task Handle(NewInputNotification notification, CancellationToken cancellationToken)
	{
		string[] hashtags = notification.NewInput.Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);
		_logger.LogInformation("Input splitted into array {hashtags}", (object)hashtags);
		await _hashtagQueue.AddRangeAsync(hashtags);
		_mediatr.Publish(new ProcessHashtagNotification());
		return;
	}
}
