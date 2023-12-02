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
		try
		{
			IPage page = await _pageManager.GetPageAsync();
			var navResult = await page.GoToAsync(_loginUrl);
			await page.WaitForSelectorAsync(_loginInputSelector);
			var loginResult = await page.TypeFieldValueAsync(_loginInputSelector, request.Login);
			var passwordResult = await page.TypeFieldValueAsync(_passwordInputSelector, request.Password);
			var submitButton = await page.QuerySelectorAsync(_loginButtonSelector);
			await submitButton.ClickAsync();
			await page.WaitForNavigationAsync(new NavigationOptions { Timeout = 5000, WaitUntil = new WaitUntilNavigation[] {WaitUntilNavigation.Networkidle2} });
			bool result = page.Url != _loginUrl;
			return result;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message, ex);
			return false;
		}
	}
}
