namespace AI_Graphs.Business_Logic.Builder_Pattern
{
	public interface IImageBuilder
	{
		public ImageBuilder SetSource(byte[] _stream);
		public ImageBuilder SetIsOpaque(bool _isOpaque);
		public ImageBuilder SetAspect(Aspect _aspect);
		public Image Build();
	}
}
