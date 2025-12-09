using AI_Graphs.BusinessLogic.Utils;
using AI_Graphs.FactoryMethodPattern;
using AI_Graphs.SingletonPattern;

namespace AI_Graphs.DrawingMethods
{
    public class GraphicsDrawable : IDrawable
    {
        public bool IsRandom = true;
        public bool IsTraceable = false;
        public bool IsInitialized = false;

        public event EventHandler<ByteArrayEventArgs> SendBytesUp;
        public void ChangeRadius(float _radius)
        {
            DataCollection.GetInstance().Radius = _radius;
        }
        public async void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (IsInitialized)
            {
                DrawingMethodFactory drawingMethodFactory;
                if (IsRandom)
                {
                    drawingMethodFactory = new SimpleDrawFactory();
                    IsRandom = false;
                }
                else if (IsTraceable)
                {
                    drawingMethodFactory = new TraceLinesFactory();
                    IsTraceable = false;
                }
                else
                {
                    drawingMethodFactory = new SaveGraphImageFactory();
                    IsRandom = false;
                }

                IDrawingMethod dMethod = drawingMethodFactory.Create();
                await dMethod.Draw(canvas, dirtyRect);
            }
        }

        private void DrawingMethod_SendBytes(object sender, BusinessLogic.Utils.ByteArrayEventArgs e)
        {
            SendBytesUp?.Invoke(this, new(e.Data));
        }
    }
}