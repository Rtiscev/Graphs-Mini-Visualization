using AI_Graphs.DrawingMethods;

namespace AI_Graphs.FactoryMethodPattern
{
	//Creator (Abstract Class)
	public abstract class DrawingMethod
	{
		public async Task InitiateDraw(ICanvas canvas, RectF rectF)
		{
			IDrawingMethod drawingMethod = CreateDrawMethod();
			await drawingMethod.Draw(canvas, rectF);
		}
		public abstract IDrawingMethod CreateDrawMethod();
	}
}
