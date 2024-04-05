using AI_Graphs.ViewModels;

namespace AI_Graphs.Views;

public partial class ImageProc : ContentPage
{
	public ImageProc(ImageProcViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}