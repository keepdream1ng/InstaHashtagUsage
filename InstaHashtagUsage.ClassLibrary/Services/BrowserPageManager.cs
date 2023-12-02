namespace InstaHashtagUsage.ClassLibrary.Services;

public class BrowserPageManager : IBrowserPageManager
{
	private IPage _page;
	private readonly ILogger<BrowserPageManager> _logger;
	private bool _pageInitialized;
	private readonly object _lock = new object();
	public IBrowserManager BrowserManager { get; private set; }

	public BrowserPageManager(ILogger<BrowserPageManager> logger, IBrowserManager browserManager)
	{
		_logger = logger;
		BrowserManager = browserManager;
	}

	public async Task<IPage> GetPageAsync()
	{
		if (!_pageInitialized) await SetPageInstanceAsync();
		return _page;
	}

	private async Task SetPageInstanceAsync()
	{
		var browser = await BrowserManager.GetBrowserAsync();
		IPage page = await browser.NewPageAsync();
		await ConfigurePage(page);

		lock (_lock)
		{
			// In case other request came for the same page object to set new page.
			if (_pageInitialized)
			{
				page.DisposeAsync();
				return;
			}
			_page = page;
			_pageInitialized = true;
		}
		_logger.LogInformation("Page {status}.", "created");
	}

	private async Task ConfigurePage(IPage page)
	{
		_logger.LogInformation("Page {status}.", "configuring");
		await page.SetRequestInterceptionAsync(true);
		// Set up request interception handler to reduce traffic (hopefully).
		page.Request += async (sender, e) =>
		{
			if ((e.Request.ResourceType == ResourceType.Image)
				|| (e.Request.ResourceType == ResourceType.Media))
			{
				// Block requests
				await e.Request.AbortAsync();
			}
			else
			{
				// Allow other requests
				await e.Request.ContinueAsync();
			}
		};
	}
}
