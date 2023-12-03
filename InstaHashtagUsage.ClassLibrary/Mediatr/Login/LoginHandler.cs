using InstaHashtagUsage.ClassLibrary.PuppeteerExtensions;
using InstaHashtagUsage.ClassLibrary.Services;

namespace InstaHashtagUsage.ClassLibrary.Mediatr;

public class LoginHandler : IRequestHandler<LoginRequest, bool>
{
	private readonly ILogger<LoginHandler> _logger;
	private readonly IBrowserPageManager _pageManager;
	private readonly IConfiguration _configuration;
	private string _loginUrl => _configuration["loginUrl"];
	private string _loginInputSelector => _configuration["loginInputSelector"];
	private string _passwordInputSelector => _configuration["passwordInputSelector"];
	private string _loginButtonSelector => _configuration["loginButtonSelector"];
	private int _inputDelay => _configuration.GetValue<int>("inputDelay");

	public LoginHandler(
		ILogger<LoginHandler> logger,
		IBrowserPageManager pageManager,
		IConfiguration configuration
		)
	{
		_logger = logger;
		_pageManager = pageManager;
		_configuration = configuration;
	}
	public async Task<bool> Handle(LoginRequest request, CancellationToken cancellationToken)
	{
		// Protection from using same page by multiple handlers at the same time.
		if (_pageManager.PageIsInUse) return false;
		try
		{
			_pageManager.PageIsInUse = true;
			IPage page = await _pageManager.GetPageAsync();
			var navResult = await page.GoToAsync(_loginUrl);
			await page.WaitForSelectorAsync(_loginInputSelector);
			bool loginResult = await page.TypeFieldValueAsync(_loginInputSelector, request.Login, _inputDelay);
			bool passwordResult = await page.TypeFieldValueAsync(_passwordInputSelector, request.Password, _inputDelay);
			var submitButton = await page.QuerySelectorAsync(_loginButtonSelector);
			await submitButton.ClickAsync();
			await page.WaitForNavigationAsync(new NavigationOptions {Timeout = 7000});
			bool result = page.Url != _loginUrl;
			_pageManager.PageIsInUse = false;
			return result;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message, ex);
			_pageManager.PageIsInUse = false;
			return false;
		}
	}
}
