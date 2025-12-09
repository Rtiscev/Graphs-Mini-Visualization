using AI_Graphs.SingletonPattern;
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
            MainCanvas.SizeChanged += MainCanvas_SizeChanged;
        }

		private void DisplayMessage(object sender, StringEventArgs e)
		{
			DisplayAlert("Path information", "I see", "");
		}

        private void MainCanvas_SizeChanged(object sender, EventArgs e)
        {
            var canvas = sender as GraphicsView;
            if (canvas != null && canvas.Width > 0 && canvas.Height > 0)
            {
                DataCollection.GetInstance().CanvasWidth = (float)canvas.Width;
                DataCollection.GetInstance().CanvasHeight = (float)canvas.Height;

                System.Diagnostics.Debug.WriteLine($"Canvas resized: {canvas.Width} x {canvas.Height}");
            }
        }

        private void InitiateRedraw(object sender, StringEventArgs e)
		{
			MainCanvas.Invalidate();
		}
	}
}
