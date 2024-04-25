using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageProcessor;
using AI_Graphs.Business_Logic.Memento_Pattern;
using AI_Graphs.Business_Logic.Builder_Pattern;

namespace AI_Graphs.ViewModels
{
	public partial class ImageProcViewModel : ObservableObject
	{
		byte[] workingImage;
		int currentPosition = 0;


		Originator originator;
		Caretaker caretaker;
		[ObservableProperty]
		Image actualImage;

		string actualPath;
		public ImageProcViewModel()
		{
			workingImage = Enumerable.Repeat((byte)0, 1000).ToArray();
			originator = new() { _workingImage = workingImage };
			caretaker = new();
			Memento memento = originator.CreateMemento();
			caretaker.SetMemento(memento);

			// process file
			string rootPath = Environment.ProcessPath;
			for (int oo = 0; oo < 6; oo++)
			{
				rootPath = Directory.GetParent(rootPath).FullName;
			}
			actualPath = System.IO.Path.Combine(rootPath, "Business Logic\\Input\\image.jpg");
		}

		[RelayCommand]
		void LoadImage()
		{
			workingImage = File.ReadAllBytes(actualPath);
			using (MemoryStream inStream = new MemoryStream(workingImage))
			{
				using (MemoryStream outStream = new MemoryStream())
				{
					// Initialize the ImageFactory using the overload to preserve EXIF metadata.
					using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
					{
						// Load, resize, set the format and quality and save an image.
						imageFactory.Load(inStream)
									.Save(outStream);
					}
					// Do something with the stream.
					using (var memoryStream = new MemoryStream())
					{
						outStream.CopyTo(memoryStream);
						workingImage = memoryStream.ToArray();

						originator._workingImage = workingImage;
						Memento memento = originator.CreateMemento();
						caretaker.SetMemento(memento);
						currentPosition++;
					}

					ImageBuilder builder = new();
					builder.SetSource(workingImage)
						   .SetAspect(Aspect.AspectFit)
						   .SetIsOpaque(false);

					ActualImage = builder.Build();
					//ActualImage = new Image() { Source = ImageSource.FromStream(() => new MemoryStream(workingImage)) };
				}
			}
		}

		[RelayCommand]
		void Flip()
		{
			using (MemoryStream inStream = new MemoryStream(workingImage))
			{
				using (MemoryStream outStream = new MemoryStream())
				{
					// Initialize the ImageFactory using the overload to preserve EXIF metadata.
					using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
					{
						// Load, resize, set the format and quality and save an image.
						imageFactory.Load(inStream)
									.Flip()
									.Save(outStream);
					}
					// Do something with the stream.
					using (var memoryStream = new MemoryStream())
					{
						outStream.CopyTo(memoryStream);
						workingImage = memoryStream.ToArray();

						originator._workingImage = workingImage;
						Memento memento = originator.CreateMemento();
						caretaker.SetMemento(memento);
						currentPosition++;
					}

					ImageBuilder builder = new();
					builder.SetSource(workingImage)
						   .SetAspect(Aspect.AspectFit)
						   .SetIsOpaque(false);

					ActualImage = builder.Build();
					//ActualImage = new Image() { Source = ImageSource.FromStream(() => new MemoryStream(workingImage)) };
				}
			}
		}
		[RelayCommand]
		void Blur()
		{
			using (MemoryStream inStream = new MemoryStream(workingImage))
			{
				using (MemoryStream outStream = new MemoryStream())
				{
					// Initialize the ImageFactory using the overload to preserve EXIF metadata.
					using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
					{
						// Load, resize, set the format and quality and save an image.
						imageFactory.Load(inStream)
									.Halftone()
									.Save(outStream);
					}
					// Do something with the stream.
					using (var memoryStream = new MemoryStream())
					{
						outStream.CopyTo(memoryStream);
						workingImage = memoryStream.ToArray();

						originator._workingImage = workingImage;
						Memento memento = originator.CreateMemento();
						caretaker.SetMemento(memento);
						currentPosition++;
					}

					ImageBuilder builder = new();
					builder.SetSource(workingImage)
						   .SetAspect(Aspect.AspectFit)
						   .SetIsOpaque(false);

					ActualImage = builder.Build();
					//ActualImage = new Image() { Source = ImageSource.FromStream(() => new MemoryStream(workingImage)) };
				}
			}
		}
		[RelayCommand]
		void Undo()
		{
			if (currentPosition > 1)
			{
				currentPosition--;
				originator.SetMemento(caretaker.GetMemento(currentPosition));

				ImageBuilder builder = new();
				builder.SetSource(originator._workingImage)
					   .SetAspect(Aspect.AspectFit)
					   .SetIsOpaque(false);

				ActualImage = builder.Build();
				//ActualImage = new Image() { Source = ImageSource.FromStream(() => new MemoryStream(originator._workingImage)) };
			}

		}
		[RelayCommand]
		void Redo()
		{
			if (currentPosition < caretaker.GetCount() - 1)
			{
				currentPosition++;
				originator.SetMemento(caretaker.GetMemento(currentPosition));
				ImageBuilder builder = new();
				builder.SetSource(originator._workingImage)
					   .SetAspect(Aspect.AspectFit)
					   .SetIsOpaque(false);

				ActualImage = builder.Build();
				//ActualImage = new Image() { Source = ImageSource.FromStream(() => new MemoryStream(originator._workingImage)) };
			}
		}
	}
}
