namespace InstaHashtagUsage.ClassLibrary.Services;

public class HashtagQueue : IHashtagQueue
{
	private Queue<string> _queue = new Queue<string>();
	private readonly object _lock = new object();
	private HashSet<string> _sessionHistory = new HashSet<string>();
	public int Count { get { return _queue.Count; } }

	public async Task AddRangeAsync(IEnumerable<string> hashtags)
	{
		hashtags = RemoveCheckedHashtags(hashtags);
		lock (_lock)
		{
			foreach (string hashtag in hashtags)
			{
				_sessionHistory.Add(hashtag);
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

	private IEnumerable<string> RemoveCheckedHashtags(IEnumerable<string> hashtags)
	{
		return hashtags.Where(hashtag => !_sessionHistory.Contains(hashtag));
	}
}
