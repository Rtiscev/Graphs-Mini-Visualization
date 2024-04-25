namespace AI_Graphs.Business_Logic.Builder_Pattern
{
	public class ImageBuilder : IImageBuilder
	{
		private readonly Image _image = new();

		public ImageBuilder SetSource(byte[] _stream)
		{
			_image.Source = ImageSource.FromStream(() => new MemoryStream(_stream));
			return this;
		}

		public ImageBuilder SetIsOpaque(bool _isOpaque)
		{
			_image.IsOpaque = _isOpaque;
			return this;
		}

		public ImageBuilder SetAspect(Aspect _aspect)
		{
			_image.Aspect = _aspect;
			return this;
		}

		public Image Build()
		{
			return _image;
		}
	}
}
