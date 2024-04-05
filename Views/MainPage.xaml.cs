using AI_Graphs.Utils;
using AI_Graphs.ViewModels;

namespace AI_Graphs
{
    public partial class MainPage : ContentPage
	{
		public MainPage(MainPageViewModel viewModel)
		{
			InitializeComponent();

			BindingContext = viewModel;

			viewModel.WorkCompleted += InitiateRedraw;
			viewModel.DisplayMsg += DisplayMessage;
		}

		private void DisplayMessage(object sender, StringEventArgs e)
		{
			DisplayAlert("Path information", "I see", "");
		}

		private void InitiateRedraw(object sender, StringEventArgs e)
		{
			MainCanvas.Invalidate();
		}
	}
}
