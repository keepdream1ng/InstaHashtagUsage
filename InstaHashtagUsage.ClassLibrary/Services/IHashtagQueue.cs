
namespace InstaHashtagUsage.ClassLibrary.Services
{
	public interface IHashtagQueue
	{
		int Count { get; }

		Task AddRangeAsync(IEnumerable<string> hashtags);
		Task<string> GetHashtagAsync();
	}
}