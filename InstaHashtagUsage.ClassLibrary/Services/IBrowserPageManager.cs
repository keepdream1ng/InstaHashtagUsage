namespace InstaHashtagUsage.ClassLibrary.Services
{
	public interface IBrowserPageManager
	{
		IBrowserManager BrowserManager { get; }
		bool PageIsInUse { get; set; }
		Task<IPage> GetPageAsync();
		void Dispose();
	}
}