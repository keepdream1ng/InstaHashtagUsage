namespace InstaHashtagUsage.ClassLibrary.Services
{
	public interface IBrowserManager
	{
		void Dispose();
		Task<IBrowser> GetBrowserAsync();
	}
}