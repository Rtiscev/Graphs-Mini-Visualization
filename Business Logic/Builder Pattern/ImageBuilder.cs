namespace AI_Graphs.Business_Logic.Builder_Pattern
{
    public class ImageBuilder : IImageBuilder
    {
        private readonly Image _image = new();

        public void SetSource(byte[] stream)
        {
            _image.Source = ImageSource.FromStream(() => new MemoryStream(stream));
        }

        public void SetIsOpaque(bool isOpaque)
        {
            _image.IsOpaque = isOpaque;
        }

        public void SetAspect(Aspect aspect)
        {
            _image.Aspect = aspect;
        }

        public Image GetResult()
        {
            return _image;
        }

    }
}
