namespace AI_Graphs.Business_Logic.Builder_Pattern
{
    public class ImageDirector
    {
        private IImageBuilder _builder;

        public ImageDirector(IImageBuilder builder)
        {
            _builder = builder;
        }

        public void SetBuilder(IImageBuilder builder)
        {
            _builder = builder;
        }

        public void ConstructGraphImage(byte[] imageData)
        {
            _builder.SetSource(imageData);
            _builder.SetIsOpaque(false);
            _builder.SetAspect(Aspect.AspectFit);
        }

        public void ConstructBackgroundImage(byte[] imageData)
        {
            _builder.SetSource(imageData);
            _builder.SetIsOpaque(true);
            _builder.SetAspect(Aspect.Fill);
        }

        public void ConstructThumbnailImage(byte[] imageData)
        {
            _builder.SetSource(imageData);
            _builder.SetIsOpaque(false);
            _builder.SetAspect(Aspect.AspectFill);
        }
    }
}
