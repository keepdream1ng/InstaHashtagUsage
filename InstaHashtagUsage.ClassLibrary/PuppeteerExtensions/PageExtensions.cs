using PuppeteerSharp.Input;

namespace InstaHashtagUsage.ClassLibrary.PuppeteerExtensions;

public static class PageExtensions
{
	/// <summary>
	/// Extension for IPage to input stuff like a user.
	/// </summary>
	public static async Task<bool> TypeFieldValueAsync(this IPage page, string fieldSelector, string value, int delay = 100)
	{
		await page.FocusAsync(fieldSelector);
		await page.TypeAsync(fieldSelector, value, new TypeOptions { Delay = delay });
		await page.Keyboard.PressAsync("Tab");
		await Task.Delay(delay);
		var result = await page.QuerySelectorWithContentAsync(fieldSelector, $"^{value}$");
		return result is not null;
	}

	public static async Task<bool> RemoveFieldValueAsync(this IPage page, string fieldSelector, int delay = 100)
	{
		await page.FocusAsync(fieldSelector);
		string value = await page.QuerySelectorAsync(fieldSelector).Result.ValueAsync();
		foreach (char c in value)
		{
			await page.Keyboard.PressAsync("Backspace");
			await Task.Delay(delay);
		}
		await page.Keyboard.PressAsync("Tab");
		var result = await page.QuerySelectorAsync(fieldSelector).Result.ValueAsync();
		return string.IsNullOrEmpty(result);
	}

}
