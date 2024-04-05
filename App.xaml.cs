using AI_Graphs.ViewModels;
using AI_Graphs.Views;

namespace AI_Graphs
{
	public partial class App : Application
	{
		public App(MainPage mainPage, ImageProc imageProc)
		{
			InitializeComponent();

			MainPage = new AppShell();
		}
	}
}
