using AI_Graphs.FactoryMethodPattern;
using AI_Graphs.Graphs;
using AI_Graphs.SingletonPattern;
using System.Numerics;

namespace AI_Graphs.DrawingMethods
{
	public class GraphicsDrawable : IDrawable
	{
		public bool IsRandom = true;
		public bool IsTraceable = false;
		public bool IsInitialized = false;

		public void ChangeRadius(float _radius)
		{
			DataCollection.GetInstance().Radius = _radius;
		}
		public async void Draw(ICanvas canvas, RectF dirtyRect)
		{
			if (IsInitialized)
			{
				IDrawingMethod drawingMethod;
				if (IsRandom)
				{
					drawingMethod = new SimpleDrawFactory().CreateMethod();
					await drawingMethod.Draw(canvas, dirtyRect);

					IsRandom = false;
				}
				else if (IsTraceable)
				{
					drawingMethod = new TraceLinesFactory().CreateMethod();
					await drawingMethod.Draw(canvas, dirtyRect);

					IsTraceable = false;
				}
				else
				{
					drawingMethod = new ChangeColorsFactory().CreateMethod();
					await drawingMethod.Draw(canvas, dirtyRect);

					IsRandom = false;
				}
			}
		}
	}
}