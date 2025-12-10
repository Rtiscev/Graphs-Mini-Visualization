namespace AI_Graphs.Business_Logic.Builder_Pattern
{
    public interface IImageBuilder
    {
        void SetSource(byte[] stream);
        void SetIsOpaque(bool isOpaque);
        void SetAspect(Aspect aspect);
        Image GetResult();
    }
}
