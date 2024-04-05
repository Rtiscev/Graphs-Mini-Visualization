using AI_Graphs.Views;
using Microsoft.Extensions.Logging;

namespace AI_Graphs
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

#if DEBUG
			builder.Logging.AddDebug();
#endif
			builder.Services.AddSingleton<ViewModels.ImageProcViewModel>();
			builder.Services.AddSingleton<ViewModels.MainPageViewModel>();

			builder.Services.AddSingleton<MainPage>();
			builder.Services.AddSingleton<ImageProc>();

			return builder.Build();
		}
	}
}
