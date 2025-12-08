using AI_Graphs.ViewModels;

namespace AI_Graphs.Views
{
    public partial class BenchmarkPage : ContentPage
    {
        public BenchmarkPage()
        {
            InitializeComponent();
            BindingContext = new BenchmarkViewModel();
        }
    }
}
