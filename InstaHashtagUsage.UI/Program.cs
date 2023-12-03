using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using InstaHashtagUsage.ClassLibrary;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Formatting.Compact;
using InstaHashtagUsage.ClassLibrary.Services;

namespace InstaHashtagUsage.UI
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
			{
				Log.Fatal((Exception)args.ExceptionObject, "Unhandled exception");
			};

			ApplicationConfiguration.Initialize();

			var serviceProvider = ConfigureServices();
			// Create main form and inject services
			var mainForm = serviceProvider.GetRequiredService<MainForm>();
			Application.Run(mainForm);

			// Closing the browser.
			var browserManger  = serviceProvider.GetRequiredService<IBrowserManager>();
			browserManger.Dispose();
			// Close and flush the logger when the application exits
			Log.CloseAndFlush();
		}

		private static IServiceProvider ConfigureServices()
		{
			var services = new ServiceCollection();

			// Add configuration
			IConfiguration configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			// Configure Serilog
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.WriteTo.Console()
				.WriteTo.File(new CompactJsonFormatter(), "./log.json", rollingInterval: RollingInterval.Day)
				.CreateLogger();

			// Add services here
			services.AddLogging(cfg => cfg.AddSerilog());
			services.AddSingleton(configuration);
			services.AddSingleton<IBrowserManager, BrowserManager>();
			services.AddSingleton<IBrowserPageManager, BrowserPageManager>();
			services.AddSingleton<IHashtagQueue, HashtagQueue>();
			services.AddWindowsFormsBlazorWebView();
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(ClassLibraryEntryPoint)));
			services.AddSingleton<MainForm>();

			return services.BuildServiceProvider();
		}
	}
}