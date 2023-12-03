namespace InstaHashtagUsage.ClassLibrary.Services;

public class HashtagQueue : IHashtagQueue
{
	private Queue<string> _queue = new Queue<string>();
	private readonly object _lock = new object();
	public int Count { get { return _queue.Count; } }

	public async Task AddRangeAsync(IEnumerable<string> hashtags)
	{
		lock (_lock)
		{
			foreach (string hashtag in hashtags)
			{
				_queue.Enqueue(hashtag);
			}
		}
	}

	public async Task<string> GetHashtagAsync()
	{
		lock (_lock)
		{
			return _queue.Dequeue();
		}
	}
}
