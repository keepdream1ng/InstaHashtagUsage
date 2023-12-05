using InstaHashtagUsage.ClassLibrary.Models;

namespace InstaHashtagUsage.ClassLibrary.Services
{
	public interface IHastagCountTableFactory
	{
		HashtagCountTable GetNewTable();
	}
}