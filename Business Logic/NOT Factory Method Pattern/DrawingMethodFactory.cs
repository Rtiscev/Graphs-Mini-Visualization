using AI_Graphs.DrawingMethods;

namespace AI_Graphs.FactoryMethodPattern
{
	//Creator (Abstract Class)
	public abstract class DrawingMethodFactory
	{
		//public async Task InitiateDraw(ICanvas canvas, RectF rectF)
		//{
		//	IDrawingMethod drawingMethod = Create();
		//	await drawingMethod.Draw(canvas, rectF);
		//}
		public abstract IDrawingMethod Create();
	}
}
