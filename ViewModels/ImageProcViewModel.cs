using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageProcessor;
using ImageProcessor.Imaging.Filters.Photo;
using AI_Graphs.Business_Logic.Memento_Pattern;
using AI_Graphs.Business_Logic.Builder_Pattern;

namespace AI_Graphs.ViewModels
{
    public partial class ImageProcViewModel : ObservableObject
    {
        private byte[] workingImage;
        private int currentPosition = 0;
        private readonly Originator originator;
        private Caretaker caretaker;
        private readonly string imagePath;

        [ObservableProperty]
        Image actualImage;

        public ImageProcViewModel()
        {
            workingImage = [];
            originator = new Originator { _workingImage = workingImage };
            caretaker = new Caretaker();

            SaveState();
            imagePath = GetImagePath();
        }

        private static string GetImagePath()
        {
            string rootPath = Environment.ProcessPath;
            for (int i = 0; i < 6; i++)
            {
                rootPath = Directory.GetParent(rootPath)?.FullName ?? rootPath;
            }
            return Path.Combine(rootPath, "Business Logic", "Input", "graph.png");
        }

        private void SaveState()
        {
            originator._workingImage = workingImage;
            Memento memento = originator.CreateMemento();


            caretaker.SetMemento(memento);
            currentPosition = caretaker.GetCount() - 1;
        }

        private void UpdateDisplay()
        {
            if (workingImage == null || workingImage.Length == 0)
            {
                ActualImage = null;
                return;
            }

            ImageBuilder builder = new ImageBuilder();
            builder.SetSource(workingImage)
                   .SetAspect(Aspect.AspectFit)
                   .SetIsOpaque(false);

            ActualImage = builder.Build();
        }

        private void ApplyTransformation(Action<ImageFactory> transformation)
        {
            if (workingImage == null || workingImage.Length == 0)
            {
                return;
            }

            using (MemoryStream inStream = new MemoryStream(workingImage))
            using (MemoryStream outStream = new MemoryStream())
            {
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                {
                    imageFactory.Load(inStream);
                    transformation(imageFactory);
                    imageFactory.Save(outStream);
                }

                workingImage = outStream.ToArray();
            }

            SaveState();
            UpdateDisplay();
        }

        [RelayCommand]
        void LoadImage()
        {
            try
            {
                if (!File.Exists(imagePath))
                {
                    return;
                }

                workingImage = File.ReadAllBytes(imagePath);

                caretaker = new Caretaker();
                currentPosition = 0;

                SaveState();
                UpdateDisplay();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load: {ex.Message}");
            }
        }


        [RelayCommand]
        void Flip()
        {
            ApplyTransformation(factory => factory.Flip());
        }

        [RelayCommand]
        void RotateLeft()
        {
            ApplyTransformation(factory => factory.Rotate(-90));
        }

        [RelayCommand]
        void RotateRight()
        {
            ApplyTransformation(factory => factory.Rotate(90));
        }

        [RelayCommand]
        void Rotate180()
        {
            ApplyTransformation(factory => factory.Rotate(180));
        }


        [RelayCommand]
        void Grayscale()
        {
            ApplyTransformation(factory => factory.Filter(MatrixFilters.GreyScale));
        }

        [RelayCommand]
        void BlackAndWhite()
        {
            ApplyTransformation(factory => factory.Filter(MatrixFilters.BlackWhite));
        }

        [RelayCommand]
        void Sepia()
        {
            ApplyTransformation(factory => factory.Filter(MatrixFilters.Sepia));
        }

        [RelayCommand]
        void Invert()
        {
            ApplyTransformation(factory => factory.Filter(MatrixFilters.Invert));
        }

        [RelayCommand]
        void Polaroid()
        {
            ApplyTransformation(factory => factory.Filter(MatrixFilters.Polaroid));
        }

        [RelayCommand]
        void Comic()
        {
            ApplyTransformation(factory => factory.Filter(MatrixFilters.Comic));
        }

        [RelayCommand]
        void Lomograph()
        {
            ApplyTransformation(factory => factory.Filter(MatrixFilters.Lomograph));
        }


        [RelayCommand]
        void HighContrast()
        {
            ApplyTransformation(factory => factory.Contrast(50));
        }

        [RelayCommand]
        void LowContrast()
        {
            ApplyTransformation(factory => factory.Contrast(-30));
        }

        [RelayCommand]
        void IncreaseBrightness()
        {
            ApplyTransformation(factory => factory.Brightness(30));
        }

        [RelayCommand]
        void DecreaseBrightness()
        {
            ApplyTransformation(factory => factory.Brightness(-30));
        }

        [RelayCommand]
        void HighSaturation()
        {
            ApplyTransformation(factory => factory.Saturation(80));
        }

        [RelayCommand]
        void LowSaturation()
        {
            ApplyTransformation(factory => factory.Saturation(-50));
        }


        [RelayCommand]
        void GaussianBlur()
        {
            ApplyTransformation(factory => factory.GaussianBlur(5));
        }

        [RelayCommand]
        void Sharpen()
        {
            ApplyTransformation(factory => factory.GaussianSharpen(5));
        }

        [RelayCommand]
        void Halftone()
        {
            ApplyTransformation(factory => factory.Halftone());
        }


        [RelayCommand]
        void Pixelate()
        {
            ApplyTransformation(factory => factory.Pixelate(8));
        }

        [RelayCommand]
        void Vignette()
        {
            ApplyTransformation(factory => factory.Vignette());
        }


        [RelayCommand]
        void ExportHighContrast()
        {
            ApplyTransformation(factory =>
                factory.Filter(MatrixFilters.GreyScale)
                       .Contrast(60)
                       .Brightness(10));
        }

        [RelayCommand]
        void ExportPrintReady()
        {
            ApplyTransformation(factory =>
                factory.Filter(MatrixFilters.GreyScale)
                       .Contrast(20)
                       .Quality(100));
        }

        [RelayCommand]
        void ExportVintage()
        {
            ApplyTransformation(factory =>
                factory.Filter(MatrixFilters.Sepia)
                       .Vignette()
                       .Contrast(15));
        }


        [RelayCommand]
        void Undo()
        {
            if (currentPosition > 0)
            {
                currentPosition--;
                originator.SetMemento(caretaker.GetMemento(currentPosition));
                workingImage = originator._workingImage;
                UpdateDisplay();
            }
        }

        [RelayCommand]
        void Redo()
        {
            if (currentPosition < caretaker.GetCount() - 1)
            {
                currentPosition++;
                originator.SetMemento(caretaker.GetMemento(currentPosition));
                workingImage = originator._workingImage;
                UpdateDisplay();
            }
        }

        [RelayCommand]
        void Reset()
        {
            LoadImage();
        }
    }
}
