using CommunityToolkit.Mvvm.ComponentModel;

namespace AI_Graphs.ViewModels
{
	public partial class ImageProcViewModel : ObservableObject
	{
		byte[] WorkingImage;

        public ImageProcViewModel()
        {
            Image sa = new Image();
        }
    }
}
