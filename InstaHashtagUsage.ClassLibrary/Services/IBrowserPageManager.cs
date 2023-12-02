namespace InstaHashtagUsage.ClassLibrary.Services
{
	public interface IBrowserPageManager
	{
		IBrowserManager BrowserManager { get; }

		Task<IPage> GetPageAsync();
	}
}