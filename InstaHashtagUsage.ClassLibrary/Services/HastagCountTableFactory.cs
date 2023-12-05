using InstaHashtagUsage.ClassLibrary.Models;

namespace InstaHashtagUsage.ClassLibrary.Services;

public class HastagCountTableFactory : IHastagCountTableFactory
{
	private readonly ILogger<HastagCountTableFactory> _logger;
	private readonly IConfiguration _configuration;

	public HastagCountTableFactory(
		ILogger<HastagCountTableFactory> logger,
		IConfiguration configuration
		)
	{
		_logger = logger;
		_configuration = configuration;
	}

	public HashtagCountTable GetNewTable()
	{
		int[] thresholds = _configuration.GetSection("hashtagCategoriesThresholds").Get<int[]>();
		_logger.LogInformation("Creating new table with thresholds {thresholdsArray}", thresholds);
		var table = new List<HashtagCountPair>[thresholds.Length + 1];
		for (int i = 0; i < table.Length; i++)
		{
			table[i] = new List<HashtagCountPair>();
		}
		return new HashtagCountTable(thresholds, table);
	}
}
