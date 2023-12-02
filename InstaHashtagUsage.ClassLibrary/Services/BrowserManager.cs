namespace InstaHashtagUsage.ClassLibrary.Services;

public class BrowserManager : IDisposable, IBrowserManager
{
	private IBrowser _browser;
	private bool _browserInitialized;
	private readonly object _lock = new object();
	private readonly ILogger<BrowserManager> _logger;
	private readonly IConfiguration _configuration;
	private bool RunBrowserHeadless => _configuration["headlessBrowser"] == "true";

	public BrowserManager(ILogger<BrowserManager> logger, IConfiguration configuration)
	{
		_logger = logger;
		_configuration = configuration;
	}

	public async Task<IBrowser> GetBrowserAsync()
	{
		if (!_browserInitialized)
		{
			await SetBrowserInstanceAsync();
		}
		return _browser;
	}

	private async Task SetBrowserInstanceAsync()
	{
		if (_browser is not null) return;

		// Downloading browser if needed.
		using (var browserFetcher = new BrowserFetcher())
		{
			var browsers = browserFetcher.GetInstalledBrowsers();
			if (!browsers.Any())
			{
				_logger.LogInformation("Downloading browser {status}.", "start");
				await browserFetcher.DownloadAsync();
				_logger.LogInformation("Downloading browser {status}.", "comlete");
			}
		}

		var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = RunBrowserHeadless, DefaultViewport = null });
		lock (_lock)
		{
			// In case some other request came to set browser instance.
			if (_browserInitialized)
			{
				browser.DisposeAsync();
				return;
			}
			_browser = browser;
			_browserInitialized = true;
		}
		_logger.LogInformation("Browser {status}.", "launched");
	}
	public async void Dispose()
	{
		if (_browser is not null)
		{
			await _browser.CloseAsync();
			await _browser.DisposeAsync();
			_logger.LogInformation("Browser {status}.", "closed");
		}
	}
}
