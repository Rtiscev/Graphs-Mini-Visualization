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
				DrawingMethod drawingMethod;
				if (IsRandom)
				{
					drawingMethod = new SimpleDrawFactory();
					IsRandom = false;
				}
				else if (IsTraceable)
				{
					drawingMethod = new TraceLinesFactory();
					IsTraceable = false;
				}
				else
				{
					drawingMethod = new ChangeColorsFactory();
					//drawingMethod.SendBytes += DrawingMethod_SendBytes;

					IsRandom = false;
				}
				await drawingMethod.InitiateDraw(canvas, dirtyRect);
			}
		}

		private void DrawingMethod_SendBytes(object sender, BusinessLogic.Utils.ByteArrayEventArgs e)
		{
			SendBytesUp?.Invoke(this, new(e.Data));
		}
	}
}