﻿using InstaHashtagUsage.ClassLibrary.PuppeteerExtensions;
using InstaHashtagUsage.ClassLibrary.Services;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace InstaHashtagUsage.ClassLibrary.Mediatr;

public class ProcessHashtagHandler : INotificationHandler<ProcessHashtagNotification>
{
	private readonly ILogger<ProcessHashtagHandler> _logger;
	private readonly IMediator _mediator;
	private readonly IBrowserPageManager _browserPageManager;
	private readonly IHashtagQueue _hashtagQueue;
	private readonly IConfiguration _configuration;
	private string _searchCssSelector => _configuration["searchBarSelector"];
	private int _inputDelay => _configuration.GetValue<int>("inputDelay");

	public ProcessHashtagHandler(
		ILogger<ProcessHashtagHandler> logger,
		IMediator mediator,
		IBrowserPageManager browserPageManager,
		IHashtagQueue hashtagQueue,
		IConfiguration configuration
		)
	{
		_logger = logger;
		_mediator = mediator;
		_browserPageManager = browserPageManager;
		_hashtagQueue = hashtagQueue;
		_configuration = configuration;
	}
	public async Task Handle(ProcessHashtagNotification notification, CancellationToken cancellationToken)
	{
		// If another chain of handlers already processing the page this wont start second chain.
		if (_browserPageManager.PageIsInUse || (_hashtagQueue.Count < 1)) return;

		_browserPageManager.PageIsInUse = true;
		string hashtagToCheck = await _hashtagQueue.GetHashtagAsync();
		try
		{
			var page = await _browserPageManager.GetPageAsync();
			var searchInput = await GetSearchInput(page);
			bool deleteResult = await page.RemoveFieldValueAsync(_searchCssSelector, _inputDelay);
			bool inputResult = await page.TypeFieldValueAsync(_searchCssSelector, $"#{hashtagToCheck}", _inputDelay);
			await Task.Delay(_inputDelay * 5);
			await page.WaitForXPathAsync($"//span[normalize-space(.)='#{hashtagToCheck}']/ancestor::span/following-sibling::span/span/span/span");
			var result = await page.XPathAsync($"//span[normalize-space(.)='#{hashtagToCheck}']/ancestor::span/following-sibling::span/span/span/span");
			var innerHtml = await result[0].GetPropertyAsync("innerHTML").Result.JsonValueAsync<string>();
			int number = ParseToInt(innerHtml);
			_logger.LogInformation("Success for parsing #{hashtag}: {publications}", hashtagToCheck, number);

			_browserPageManager.PageIsInUse = false;
			await _mediator.Publish(new ProcessHashtagNotification());
		}
		catch (Exception ex)
		{
			_browserPageManager.PageIsInUse = false;
			_logger.LogError("Exception processing #{hashtag}: {message}", hashtagToCheck, ex.Message);
			throw;
		}
	}

	private async Task<IElementHandle> GetSearchInput(IPage page)
	{
		IElementHandle searchInput = await page.QuerySelectorAsync(_searchCssSelector);
		if (searchInput is null)
		{
			int tabsCount = _configuration.GetValue<int>("neededTabsToSearch");
			for (int i = 0; i < tabsCount; i++)
			{
				await page.Keyboard.PressAsync("Tab");
				await Task.Delay(_inputDelay);
			}
			await page.Keyboard.PressAsync("Enter");
			string cssClassName = _searchCssSelector.Substring(1);
			await page.EvaluateFunctionAsync(@"
                (className) => {
                    const focusedInput = document.querySelector(':focus');
                    if (focusedInput) {
                        focusedInput.classList.add(className);
                    }
                }
            ", cssClassName);
			await Task.Delay(_inputDelay);
			searchInput = await page.QuerySelectorAsync(_searchCssSelector);
			if (searchInput is null)
			{
				throw new Exception("Cannot find input field");
			}
		}
		return searchInput;
	}
	private int ParseToInt(string value)
	{
		// Use regular expression to remove non-numeric characters
		string cleanedString = Regex.Replace(value, "[^0-9]", "");

		try
		{
			// Parse the cleaned string into an integer
			int parsedNumber = int.Parse(cleanedString);
			return parsedNumber;
		}
		catch (Exception ex)
		{
			_logger.LogError("Error parsing {value}: {message}", value, ex.Message);
			return 0;
		}
	}
}
